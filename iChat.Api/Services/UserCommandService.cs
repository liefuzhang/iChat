using iChat.Api.Constants;
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
    public class UserCommandService : IUserCommandService {
        private readonly iChatContext _context;
        private readonly IEmailHelper _emailService;
        private readonly IUserIdenticonHelper _userIdenticonHelper;
        private readonly IUserQueryService _userQueryService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;

        public UserCommandService(iChatContext context, IEmailHelper emailService,
            IUserIdenticonHelper userIdenticonHelper, IUserQueryService userQueryService,
            ICacheService cacheService, INotificationService notificationService,
            IConversationQueryService conversationQueryService) {
            _context = context;
            _emailService = emailService;
            _userIdenticonHelper = userIdenticonHelper;
            _userQueryService = userQueryService;
            _cacheService = cacheService;
            _notificationService = notificationService;
            _conversationQueryService = conversationQueryService;
        }

        public async Task<int> RegisterAsync(string email, string password, string displayName, int workspaceId) {
            if (await _userQueryService.IsEmailRegisteredAsync(email)) {
                throw new Exception($"Email \"{email}\" is already taken");
            }

            var identiconGuid = await _userIdenticonHelper.GenerateUserIdenticon(email);
            var user = new User(email, password, workspaceId, displayName, identiconGuid);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task InviteUsersAsync(UserDto user, Workspace workspace, List<string> emails) {
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

        private async Task InviteUserAsync(UserDto user, Workspace workspace, string email) {
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

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email);
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

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null) {
                throw new Exception("Cannot find user.");
            }

            user.SetPassword(resetPasswordDto.Password);

            request.Process();
            await _context.SaveChangesAsync();
        }

        private async Task SendUserStatusChangedNotificationAsync(int userId, int workspaceId) {
            var userIds = (await _conversationQueryService.GetOtherUserIdsInPrivateConversationAsync(userId, workspaceId))
                .ToList();
            userIds.Add(userId);
            await _notificationService.SendUserStatusChangedNotificationAsync(userIds);
        }

        public async Task SetUserStatusAsync(int userId, int workspaceId, UserStatus status) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.SetStatus(status);
            await _context.SaveChangesAsync();

            await SendUserStatusChangedNotificationAsync(userId, workspaceId);
        }

        public async Task ClearUserStatusAsync(int userId, int workspaceId) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.SetStatus(UserStatus.Active);
            await _context.SaveChangesAsync();

            await SendUserStatusChangedNotificationAsync(userId, workspaceId);
        }

        public async Task EditProfile(UserEditDto userDto, int userId) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            user.UpdateDetails(userDto.DisplayName, userDto.PhoneNumber);
            await _context.SaveChangesAsync();
        }

        public async Task SetUserOnlineAsync(int userId, int workspaceId) {
            var online = await _cacheService.GetUserOnlineAsync(userId, workspaceId);
            await _cacheService.SetUserOnlineAsync(userId, workspaceId);

            if (!online) {
                var userIds = await _conversationQueryService.GetOtherUserIdsInPrivateConversationAsync(userId, workspaceId);
                await _notificationService.SendUserOnlineNotificationAsync(userIds);
            }
        }
    }
}