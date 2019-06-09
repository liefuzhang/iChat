﻿using AutoMapper;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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

        public ChannelService(iChatContext context, IUserService userService,
            ICacheService cacheService, IMapper mapper) {
            _context = context;
            _userService = userService;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId) {
            var unreadChannelIds = await _cacheService.GetUnreadChannelIdsForUserAsync(userId, workspaceId);
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == c.Id))
                .ToListAsync();

            var channelDtos = channels.Select(c => {
                var dto = _mapper.Map<ChannelDto>(c);
                dto.HasUnreadMessage = unreadChannelIds.Contains(c.Id);
                return dto;
            });

            return channelDtos;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllChannelsAsync(int workspaceId) {
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId)
                .ToListAsync();

            var channelDtos = channels.Select(async c => {
                var dto = _mapper.Map<ChannelDto>(c);
                return dto;
            });

            return await Task.WhenAll(channelDtos);
        }

        public async Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId) {
            var allChannels = await GetAllChannelsAsync(workspaceId);
            var subscribedChannels = await GetChannelsForUserAsync(userId, workspaceId);
            return allChannels.Where(c => !subscribedChannels.Any(sc => sc.Id == c.Id));
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
                throw new ArgumentException("User and channel are not in the same workspace");
            }

            var channelSubscription = new ChannelSubscription(channelId, userId);

            _context.ChannelSubscriptions.Add(channelSubscription);
            await _context.SaveChangesAsync();
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
    }
}