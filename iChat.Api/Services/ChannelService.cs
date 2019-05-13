﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Constants;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services {
    public class ChannelService : IChannelService {
        private readonly iChatContext _context;
        private readonly IUserService _userService;

        public ChannelService(iChatContext context, IUserService userService) {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<Channel>> GetChannelsAsync(int userId, int workspaceId) {
            var channels = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ChannelSubscriptions.Any(cs => cs.UserId == userId &&
                        cs.ChannelId == c.Id))
                .ToListAsync();

            return channels;
        }

        public async Task<Channel> GetChannelByIdAsync(int id, int workspaceId) {
            var channel = await _context.Channels.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.Id == id)
                .SingleOrDefaultAsync();

            return channel;
        }

        public async Task CreateChannelAsync(string channelName, int workspaceId) {
            if (string.IsNullOrWhiteSpace(channelName) || workspaceId < 1) {
                throw new ArgumentException("Invalid input");
            }

            var channel = new Channel {
                Name = channelName,
                WorkspaceId = workspaceId
            };

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToChannelAsync(int channelId, int userId, int workspaceId) {
            if (channelId < 1 || userId < 1) {
                throw new ArgumentException("Invalid input");
            }

            var user = await _userService.GetUserByIdAsync(userId, workspaceId);
            var channel = await GetChannelByIdAsync(channelId, workspaceId);

            if (user == null || channel == null) {
                throw new ArgumentException("User and channel are not in the same workplace");
            }

            var channelSubscription = new ChannelSubscription {
                ChannelId = channelId,
                UserId = userId
            };

            _context.ChannelSubscriptions.Add(channelSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task AddDefaultChannelsToNewWorkplaceAsync(int workspaceId) {
            if (workspaceId < 1) {
                throw new ArgumentException("Invalid input");
            }

            await CreateChannelAsync(iChatConstants.DefaultChannelGeneral, workspaceId);
            await CreateChannelAsync(iChatConstants.DefaultChannelRandom, workspaceId);
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

        private async Task<User> GetChannelByNameAsync(string name, int workspaceId) {
            var user = await _context.Users.AsNoTracking()
                .Where(c => c.DisplayName == name &&
                    c.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}