using AutoMapper;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class ChannelService : IChannelService {
        private readonly iChatContext _context;
        private readonly IUserService _userService;
        private ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ChannelService(iChatContext context, IUserService userService,
            ICacheService cacheService, IMapper mapper, INotificationService notificationService) {
            _context = context;
            _userService = userService;
            _cacheService = cacheService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId) {
            var unreadChannels = await _cacheService.GetUnreadChannelForUserAsync(userId, workspaceId);
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == c.Id))
                .ToListAsync();

            var channelDtos = channels.Select(c => {
                var dto = _mapper.Map<ChannelDto>(c);
                var unreadChannel = unreadChannels.SingleOrDefault(uc => uc.ChannelId == c.Id);
                dto.HasUnreadMessage = unreadChannel != null;
                dto.UnreadMentionCount = unreadChannel?.UnreadMentionCount ?? 0;
                return dto;
            });

            return channelDtos;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllChannelsAsync(int workspaceId) {
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId)
                .ToListAsync();

            var channelDtos = channels.Select(c => {
                var dto = _mapper.Map<ChannelDto>(c);
                return dto;
            });

            return channelDtos;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId) {
            var allChannels = await GetAllChannelsAsync(workspaceId);
            var subscribedChannels = await GetChannelsForUserAsync(userId, workspaceId);
            return allChannels.Where(c => !subscribedChannels.Any(sc => sc.Id == c.Id));
        }

        public bool IsUserSubscribedToChannel(int channelId, int userId) {
            return _context.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == channelId);
        }

        public async Task<ChannelDto> GetChannelByIdAsync(int id, int workspaceId) {
            var channel = await _context.Channels.AsNoTracking()
                    .Where(c => c.WorkspaceId == workspaceId &&
                        c.Id == id)
                    .SingleOrDefaultAsync();

            return _mapper.Map<ChannelDto>(channel);
        }

        public async Task<int> CreateChannelAsync(string channelName, int workspaceId, string topic = "") {
            if (await _context.Channels.AnyAsync(c => c.WorkspaceId == workspaceId && c.Name == channelName)) {
                throw new ArgumentException($"Channel \"{channelName}\" already exists.");
            }

            var channel = new Channel(channelName, workspaceId, topic);

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return channel.Id;
        }

        public async Task<IEnumerable<int>> GetAllChannelUserIdsAsync(int channelId) {
            return await _context.ChannelSubscriptions
                .Where(cs => cs.ChannelId == channelId)
                .Select(cs => cs.UserId)
                .ToListAsync();
        }

        public async Task AddUserToChannelAsync(int channelId, int userId, int workspaceId) {
            var user = await _userService.GetUserByIdAsync(userId, workspaceId);
            var channel = await GetChannelByIdAsync(channelId, workspaceId);

            if (user == null || channel == null) {
                throw new ArgumentException("Cannot find user or channel");
            }

            AddUsersToChannel(new List<int>(new[] { userId }), channelId);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromChannelAsync(int channelId, int userId) {
            var channelSubscription = _context.ChannelSubscriptions
                .Single(cs => cs.ChannelId == channelId && cs.UserId == userId);
            _context.ChannelSubscriptions.Remove(channelSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task InviteOtherMembersToChannelAsync(int channelId, List<int> userIds, int userId) {
            if (userIds == null || userIds.Count < 1) {
                throw new ArgumentException("Invalid users");
            }

            if (!IsUserSubscribedToChannel(channelId, userId)) {
                throw new ArgumentException("User is not subsribed to channel.");
            }

            AddUsersToChannel(userIds, channelId);
            await _context.SaveChangesAsync();
        }

        private void AddUsersToChannel(List<int> userIds, int channelId) {
            userIds.ForEach(id => {
                var channelSubscription = new ChannelSubscription(channelId, id);
                _context.ChannelSubscriptions.Add(channelSubscription);
            });
        }

        public async Task AddDefaultChannelsToNewWorkplaceAsync(int workspaceId) {
            if (workspaceId < 1) {
                throw new ArgumentException("Invalid input");
            }

            await CreateChannelAsync(iChatConstants.DefaultChannelGeneral, workspaceId, "Anything that's talkable");
            await CreateChannelAsync(iChatConstants.DefaultChannelRandom, workspaceId, "Another random channel");
        }

        public async Task AddUserToDefaultChannelsAsync(int userId, int workspaceId) {
            if (userId < 1 || workspaceId < 1) {
                throw new ArgumentException("Invalid input");
            }

            var defaultChannelGeneral = await GetChannelByNameAsync(iChatConstants.DefaultChannelGeneral, workspaceId);
            var defaultChannelRandom = await GetChannelByNameAsync(iChatConstants.DefaultChannelRandom, workspaceId);

            await AddUserToChannelAsync(defaultChannelGeneral.Id, userId, workspaceId);
            await AddUserToChannelAsync(defaultChannelRandom.Id, userId, workspaceId);
        }

        private async Task<Channel> GetChannelByNameAsync(string name, int workspaceId) {
            var channel = await _context.Channels.AsNoTracking()
                .Where(c => c.Name == name &&
                    c.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync();

            return channel;
        }

        public async Task<int> GetDefaultChannelGeneralIdAsync(int workspaceId) {
            if (workspaceId < 1) {
                throw new ArgumentException("Invalid input");
            }

            var defaultChannelGeneral = await GetChannelByNameAsync(iChatConstants.DefaultChannelGeneral, workspaceId);
            return defaultChannelGeneral.Id;
        }

        public async Task NotifyTypingAsync(int channelId, int currentUserId, int workspaceId) {
            var currentUser = await _userService.GetUserByIdAsync(currentUserId, workspaceId);
            if (currentUser == null) {
                return;
            }

            var userIds = (await GetAllChannelUserIdsAsync(channelId)).ToList();
            userIds.Remove(currentUserId);

            _notificationService.SendUserTypingNotificationAsync(userIds, currentUser.DisplayName, true, channelId);
        }
    }
}