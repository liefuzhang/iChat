using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class ChannelCommandService : IChannelCommandService
    {
        private readonly iChatContext _context;
        private readonly IUserQueryService _userQueryService;
        private readonly IChannelQueryService _channelQueryService;
        private readonly INotificationService _notificationService;
        private readonly IMessageCommandService _messageCommandService;

        public ChannelCommandService(iChatContext context, IUserQueryService userQueryService,
            IChannelQueryService channelQueryService, INotificationService notificationService, IMessageCommandService messageCommandService)
        {
            _context = context;
            _userQueryService = userQueryService;
            _channelQueryService = channelQueryService;
            _notificationService = notificationService;
            _messageCommandService = messageCommandService;
        }

        public async Task<int> CreateChannelAsync(string channelName, int userId, int workspaceId, string topic = "")
        {
            if (await _context.Channels.AnyAsync(c => c.WorkspaceId == workspaceId && c.Name == channelName))
            {
                throw new ArgumentException($"Channel \"{channelName}\" already exists.");
            }

            var channel = new Channel(channelName, userId, workspaceId, topic);

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return channel.Id;
        }
        public async Task DeleteChannelAsync(int channelId, int userId)
        {

            var channel = await _context.Channels.FindAsync(channelId);
            
            if (channel == null) {
                throw new ArgumentException("Cannot channel");
            }
            if (channel.CreatedByUserId != userId)
            {
                throw new ArgumentException("Channel is not created by current user");
            }
            channel.IsDeleted = true;
        }

        public async Task AddUserToChannelAsync(int channelId, int userId, int workspaceId)
        {
            var user = await _userQueryService.GetUserByIdAsync(userId, workspaceId);
            var channel = await _channelQueryService.GetChannelByIdAsync(channelId, workspaceId);

            if (user == null || channel == null)
            {
                throw new ArgumentException("Cannot find user or channel");
            }

            await AddUsersToChannelAsync(new List<int>(new[] { userId }), channelId, workspaceId);
        }

        public async Task RemoveUserFromChannelAsync(int channelId, int userId)
        {
            var channelSubscription = _context.ChannelSubscriptions
                .Single(cs => cs.ChannelId == channelId && cs.UserId == userId);
            _context.ChannelSubscriptions.Remove(channelSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task InviteOtherMembersToChannelAsync(int channelId, List<int> userIds, int userId, int workspaceId)
        {
            if (userIds == null || userIds.Count < 1)
            {
                throw new ArgumentException("Invalid users");
            }

            if (!_channelQueryService.IsUserSubscribedToChannel(channelId, userId))
            {
                throw new ArgumentException("User is not subsribed to channel.");
            }

            await AddUsersToChannelAsync(userIds, channelId, workspaceId);
        }

        private async Task AddUsersToChannelAsync(List<int> userIds, int channelId, int workspaceId)
        {
            userIds.ForEach(id =>
            {
                var channelSubscription = new ChannelSubscription(channelId, id);
                _context.ChannelSubscriptions.Add(channelSubscription);
            });
            await _context.SaveChangesAsync();

            await _messageCommandService.PostJoinChannelSystemMessageAsync(channelId, userIds, workspaceId);
        }

        public async Task AddDefaultChannelsToNewWorkplaceAsync(int userId, int workspaceId)
        {
            if (userId < 1 || workspaceId < 1)
            {
                throw new ArgumentException("Invalid input");
            }

            await CreateChannelAsync(iChatConstants.DefaultChannelGeneral, userId, workspaceId, "Anything that's talkable");
            await CreateChannelAsync(iChatConstants.DefaultChannelRandom, userId, workspaceId, "Another random channel");
        }

        public async Task AddUserToDefaultChannelsAsync(int userId, int workspaceId)
        {
            if (userId < 1 || workspaceId < 1)
            {
                throw new ArgumentException("Invalid input");
            }

            var defaultChannelGeneral = await _channelQueryService.GetChannelByNameAsync(iChatConstants.DefaultChannelGeneral, workspaceId);
            var defaultChannelRandom = await _channelQueryService.GetChannelByNameAsync(iChatConstants.DefaultChannelRandom, workspaceId);

            await AddUserToChannelAsync(defaultChannelGeneral.Id, userId, workspaceId);
            await AddUserToChannelAsync(defaultChannelRandom.Id, userId, workspaceId);
        }

        public async Task NotifyTypingAsync(int channelId, int currentUserId, bool isFinished)
        {
            var currentUser = await _userQueryService.GetUserByIdAsync(currentUserId);
            if (currentUser == null)
            {
                return;
            }

            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            userIds.Remove(currentUserId);

            if (isFinished)
            {
                await _notificationService.SendUserFinishedTypingNotificationAsync(userIds, currentUser.DisplayName, true, channelId);
            }
            else
            {
                await _notificationService.SendUserTypingNotificationAsync(userIds, currentUser.DisplayName, true, channelId);
            }
        }
    }
}