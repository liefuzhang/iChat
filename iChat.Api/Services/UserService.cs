using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Contract;
using iChat.Api.Dtos;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services {
    public class UserService : IUserService {
        private readonly iChatContext _context;
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public UserService(iChatContext context, IIdentityService identityService, 
            IEmailService emailService) {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
        }

        // when workspace is not available (e.g. onTokenValidated)
        public async Task<User> GetUserByIdAsync(int id) {
            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, int workspaceId) {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email, int workspaceId) {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int workspaceId) {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .ToListAsync();

            return users;
        }

        public async Task InviteUsersAsync(User user, Workspace workspace, List<string> emails) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (workspace == null)
                throw new ArgumentNullException(nameof(workspace));

            foreach (var email in emails.Distinct()) {
                if (string.IsNullOrWhiteSpace(email))
                    continue;

                if (_context.UserInvitations.Any(ui => ui.UserEmail == email && ui.WorkspaceId == workspace.Id)) {
                    // user has already been invited
                    continue;
                }

                if (_context.Users.Any(u => u.Email == email && u.WorkspaceId == workspace.Id)) {
                    // user has already joined workspace
                    continue;
                }

                var invitationCode = Guid.NewGuid();
                var newUserInvitation = new UserInvitation {
                    InvitedByUserId = user.Id,
                    WorkspaceId = workspace.Id,
                    UserEmail = email,
                    InviteDate = DateTime.Now,
                    InvitationCode = invitationCode,
                    Acceptted = false
                };
                _context.UserInvitations.Add(newUserInvitation);
                await _context.SaveChangesAsync();

                await _emailService.SendUserInvitationEmailAsync(new UserInvitationData {
                    ReceiverAddress = email,
                    InviterName = user.DisplayName,
                    InviterEmail = user.Email,
                    WorkspaceName = workspace.Name,
                    InvitationCode = invitationCode
                });
            }
        }

        public async Task<int> AcceptInvitationAsync(UserInvitationDto userInvitationDto) {
            if (userInvitationDto.Email == null)
                throw new ArgumentNullException(nameof(userInvitationDto.Email));

            if (userInvitationDto.Password == null)
                throw new ArgumentNullException(nameof(userInvitationDto.Password));

            if (!Guid.TryParse(userInvitationDto.Code, out Guid code))
                throw new ArgumentException("Invalid code.");

            var invitation = await _context.UserInvitations.SingleOrDefaultAsync(ui =>
                ui.UserEmail == userInvitationDto.Email &&
                ui.InvitationCode == code &&
                ui.Acceptted == false);

            if (invitation == null)
                throw new Exception("Invalid invitation.");

            var userId = await _identityService.RegisterAsync(userInvitationDto.Email, userInvitationDto.Password,
                invitation.WorkspaceId, userInvitationDto.Name);

            invitation.Acceptted = true;
            await _context.SaveChangesAsync();

            return userId;
        }
    }
}