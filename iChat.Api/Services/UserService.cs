using AutoMapper;
using iChat.Api.Contract;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class UserService : IUserService {
        private readonly iChatContext _context;
        private readonly IEmailHelper _emailService;
        private readonly IUserIdenticonHelper _userIdenticonHelper;
        private readonly IMapper _mapper;

        public UserService(iChatContext context, IEmailHelper emailService,
            IUserIdenticonHelper userIdenticonHelper, IMapper mapper) {
            _context = context;
            _emailService = emailService;
            _userIdenticonHelper = userIdenticonHelper;
            _mapper = mapper;
        }

        // when workspace is not available (e.g. onTokenValidated)
        public async Task<User> GetUserByIdAsync(int id) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, int workspaceId) {
            var user = await _context.Users
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int workspaceId) {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .Select(u => _mapper.Map<UserDto>(u))
                .ToListAsync();

            return users;
        }

        public async Task<bool> IsEmailRegisteredAsync(string email) {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<int> RegisterAsync(string email, string password, string displayName, int workspaceId) {
            if (await IsEmailRegisteredAsync(email)) {
                throw new Exception($"Email \"{email}\" is already taken");
            }

            var identiconGuid = await _userIdenticonHelper.GenerateUserIdenticon(email);
            var user = new User(email, password, workspaceId, displayName, identiconGuid);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task InviteUsersAsync(User user, Workspace workspace, List<string> emails) {
            if (user == null) {
                throw new ArgumentNullException(nameof(user));
            }

            if (workspace == null) {
                throw new ArgumentNullException(nameof(workspace));
            }

            if (emails == null || !emails.Any()) {
                throw new ArgumentNullException(nameof(emails));
            }

            foreach (var email in emails) {
                if (_context.Users.Any(u => u.Email == email)) {
                    throw new Exception($"User with email \"{email}\" already exists.");
                }
            }

            foreach (var email in emails.Distinct()) {
                await InviteUserAsync(user, workspace, email);
            }
        }

        private async Task InviteUserAsync(User user, Workspace workspace, string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                return;
            }

            var existingInvitation = await _context.UserInvitations
                .SingleOrDefaultAsync(ui => ui.Acceptted == false && ui.Cancelled == false &&
                    ui.UserEmail == email && ui.WorkspaceId == workspace.Id);
            if (existingInvitation != null) {
                existingInvitation.Cancel();
            }

            var newUserInvitation = new UserInvitation(email, user.Id, workspace.Id);
            _context.UserInvitations.Add(newUserInvitation);
            await _context.SaveChangesAsync();

            await _emailService.SendUserInvitationEmailAsync(new UserInvitationData {
                ReceiverAddress = email,
                InviterName = user.DisplayName,
                InviterEmail = user.Email,
                WorkspaceName = workspace.Name,
                InvitationCode = newUserInvitation.InvitationCode
            });
        }

        public async Task<int> AcceptInvitationAsync(UserInvitationDto userInvitationDto) {
            if (userInvitationDto.Email == null) {
                throw new ArgumentNullException(nameof(userInvitationDto.Email));
            }

            if (userInvitationDto.Password == null) {
                throw new ArgumentNullException(nameof(userInvitationDto.Password));
            }

            if (!Guid.TryParse(userInvitationDto.Code, out Guid code)) {
                throw new ArgumentException("Invalid code.");
            }

            var invitation = await _context.UserInvitations.SingleOrDefaultAsync(ui =>
                ui.UserEmail == userInvitationDto.Email &&
                ui.InvitationCode == code &&
                ui.Acceptted == false &&
                ui.Cancelled == false);

            if (invitation == null) {
                throw new Exception("Invalid invitation.");
            }

            var userId = await RegisterAsync(userInvitationDto.Email, userInvitationDto.Password,
                userInvitationDto.Name, invitation.WorkspaceId);

            invitation.Accept();
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task ForgotPasswordAsync(string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                return;
            }

            var user = await GetUserByEmailAsync(email);
            if (user == null) {
                return;
            }

            var existingRequest = await _context.ResetPasswordRequsets
                .SingleOrDefaultAsync(rpr => rpr.Resetted == false && rpr.Cancelled == false &&
                    rpr.Email == email);
            if (existingRequest != null) {
                existingRequest.Cancel();
            }

            var resetPasswordRequset = new ResetPasswordRequset(email);
            _context.ResetPasswordRequsets.Add(resetPasswordRequset);
            await _context.SaveChangesAsync();

            await _emailService.SendResetPasswordEmailAsync(email, resetPasswordRequset.ResetCode);
        }
        
        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto) {
            if (resetPasswordDto.Email == null) {
                throw new ArgumentNullException(nameof(resetPasswordDto.Email));
            }

            if (resetPasswordDto.Password == null) {
                throw new ArgumentNullException(nameof(resetPasswordDto.Password));
            }

            if (!Guid.TryParse(resetPasswordDto.Code, out Guid code)) {
                throw new ArgumentException("Invalid code.");
            }

            var request = await _context.ResetPasswordRequsets.SingleOrDefaultAsync(rpr =>
                rpr.Email == resetPasswordDto.Email &&
                rpr.ResetCode == code &&
                rpr.Resetted == false &&
                rpr.Cancelled == false);

            if (request == null) {
                throw new Exception("Invalid Reset.");
            }

            var user = await GetUserByEmailAsync(resetPasswordDto.Email);

            if (user == null) {
                throw new Exception("Cannot find user.");
            }

            user.SetPassword(resetPasswordDto.Password);

            request.Process();
            await _context.SaveChangesAsync();
        }

        public async Task SetUserStatusAsync(int userId, int workspaceId, UserStatus status) {
            var user = await GetUserByIdAsync(userId, workspaceId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.SetStatus(status);
            await _context.SaveChangesAsync();
        }

        public async Task ClearUserStatusAsync(int userId, int workspaceId) {
            var user = await GetUserByIdAsync(userId, workspaceId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.SetStatus(UserStatus.Active);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetUserStatus(int userId, int workspaceId) {
            var user = await GetUserByIdAsync(userId, workspaceId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            return user.Status.ToString();
        }

        public async Task EditProfile(UserEditDto userDto, int userId, int workspaceId) {
            var user = await GetUserByIdAsync(userId, workspaceId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.UpdateDetails(userDto.DisplayName, userDto.PhoneNumber);
            await _context.SaveChangesAsync();
        }

    }
}