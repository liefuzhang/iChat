﻿using iChat.Api.Contract;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class UserService : IUserService
    {
        private readonly iChatContext _context;
        private readonly IEmailHelper _emailService;
        private readonly IIdentityService _identityService;
        private readonly IUserIdenticonHelper _userIdenticonHelper;

        public UserService(iChatContext context, IIdentityService identityService,
            IEmailHelper emailService, IUserIdenticonHelper userIdenticonHelper)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _userIdenticonHelper = userIdenticonHelper;
        }

        // when workspace is not available (e.g. onTokenValidated)
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, int workspaceId)
        {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email, int workspaceId)
        {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int workspaceId)
        {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .ToListAsync();

            return users;
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<int> RegisterAsync(string email, string password, int workspaceId, string name = null)
        {
            if (await IsEmailRegisteredAsync(email))
            {
                throw new Exception($"Email \"{email}\" is already taken");
            }

            var identiconGuid = await _userIdenticonHelper.GenerateUserIdenticon(email);
            var user = new User(email, password, workspaceId, name, identiconGuid);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task InviteUsersAsync(User user, Workspace workspace, List<string> emails)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            validateEmails(emails.Distinct(), workspace);

            foreach (var email in emails.Distinct())
            {
                await InviteUserAsync(user, workspace, email);
            }
        }

        private async Task InviteUserAsync(User user, Workspace workspace, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var newUserInvitation = new UserInvitation(email, user.Id, workspace.Id);
            _context.UserInvitations.Add(newUserInvitation);
            await _context.SaveChangesAsync();

            await _emailService.SendUserInvitationEmailAsync(new UserInvitationData
            {
                ReceiverAddress = email,
                InviterName = user.DisplayName,
                InviterEmail = user.Email,
                WorkspaceName = workspace.Name,
                InvitationCode = newUserInvitation.InvitationCode
            });
        }

        private void validateEmails(IEnumerable<string> emails, Workspace workspace)
        {
            foreach (var email in emails)
            {
                if (_context.UserInvitations.Any(ui => ui.UserEmail == email && ui.WorkspaceId == workspace.Id))
                {
                    // user has already been invited
                    throw new Exception($"User with email \"{email}\" has already been invited.");
                }

                if (_context.Users.Any(u => u.Email == email && u.WorkspaceId == workspace.Id))
                {
                    // user has already joined workspace
                    throw new Exception($"User with email \"{email}\" already exists.");
                }
            }
        }

        public async Task<int> AcceptInvitationAsync(UserInvitationDto userInvitationDto)
        {
            if (userInvitationDto.Email == null)
            {
                throw new ArgumentNullException(nameof(userInvitationDto.Email));
            }

            if (userInvitationDto.Password == null)
            {
                throw new ArgumentNullException(nameof(userInvitationDto.Password));
            }

            if (!Guid.TryParse(userInvitationDto.Code, out Guid code))
            {
                throw new ArgumentException("Invalid code.");
            }

            var invitation = await _context.UserInvitations.SingleOrDefaultAsync(ui =>
                ui.UserEmail == userInvitationDto.Email &&
                ui.InvitationCode == code &&
                ui.Acceptted == false);

            if (invitation == null)
            {
                throw new Exception("Invalid invitation.");
            }

            var userId = await RegisterAsync(userInvitationDto.Email, userInvitationDto.Password,
                invitation.WorkspaceId, userInvitationDto.Name);

            invitation.Accept();
            await _context.SaveChangesAsync();

            return userId;
        }

        public async Task SetUserStatusAsync(int userId, int workplaceId, UserStatus status)
        {
            var user = await GetUserByIdAsync(userId, workplaceId);
            if (user == null)
            {
                throw new Exception("User cannot be found.");
            }

            user.SetStatus(status);
            await _context.SaveChangesAsync();
        }
    }
}