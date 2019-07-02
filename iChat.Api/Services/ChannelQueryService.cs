using AutoMapper;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class ChannelQueryService : IChannelQueryService
    {
        private readonly iChatContext _context;
        private readonly IUserQueryService _userQueryService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ChannelQueryService(iChatContext context, IUserQueryService userQueryService,
            ICacheService cacheService, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _userQueryService = userQueryService;
            _cacheService = cacheService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId)
        {
            var unreadChannels = await _cacheService.GetUnreadChannelForUserAsync(userId, workspaceId);
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == c.Id))
                .ToListAsync();

            var channelDtos = channels.Select(c =>
            {
                var dto = _mapper.Map<ChannelDto>(c);
                var unreadChannel = unreadChannels.SingleOrDefault(uc => uc.ChannelId == c.Id);
                dto.HasUnreadMessage = unreadChannel != null;
                dto.UnreadMentionCount = unreadChannel?.UnreadMentionCount ?? 0;
                return dto;
            });

            return channelDtos;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllChannelsAsync(int workspaceId)
        {
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId)
                .ToListAsync();

            var channelDtos = channels.Select(c =>
            {
                var dto = _mapper.Map<ChannelDto>(c);
                return dto;
            });

            return channelDtos;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId)
        {
            var allChannels = await GetAllChannelsAsync(workspaceId);
            var subscribedChannels = await GetChannelsForUserAsync(userId, workspaceId);
            return allChannels.Where(c => !subscribedChannels.Any(sc => sc.Id == c.Id));
        }

        public bool IsUserSubscribedToChannel(int channelId, int userId)
        {
            return _context.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == channelId);
        }

        public async Task<ChannelDto> GetChannelByIdAsync(int id, int workspaceId)
        {
            var channel = await _context.Channels.AsNoTracking()
                    .Where(c => c.WorkspaceId == workspaceId &&
                        c.Id == id)
                    .SingleOrDefaultAsync();

            return _mapper.Map<ChannelDto>(channel);
        }

        public async Task<IEnumerable<int>> GetAllChannelUserIdsAsync(int channelId)
        {
            return await _context.ChannelSubscriptions
                .Where(cs => cs.ChannelId == channelId)
                .Select(cs => cs.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllChannelUsersAsync(int channelId)
        {
            var userIds = await GetAllChannelUserIdsAsync(channelId);
            var users = await _context.Users.Where(u => userIds.Contains(u.Id))
                .Select(u => _mapper.Map<UserDto>(u)).ToListAsync();
            return users;
        }

        public async Task<Channel> GetChannelByNameAsync(string name, int workspaceId)
        {
            var channel = await _context.Channels.AsNoTracking()
                .Where(c => c.Name == name &&
                    c.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync();

            return channel;
        }

        public async Task<int> GetDefaultChannelGeneralIdAsync(int workspaceId)
        {
            if (workspaceId < 1)
            {
                throw new ArgumentException("Invalid input");
            }

            var defaultChannelGeneral = await GetChannelByNameAsync(iChatConstants.DefaultChannelGeneral, workspaceId);
            return defaultChannelGeneral.Id;
        }
    }
}