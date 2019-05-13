﻿using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IChannelService {
        Task AddDefaultChannelsToNewWorkplaceAsync(int workspaceId);
        Task AddUserToChannelAsync(int channelId, int userId, int workspaceId);
        Task AddUserToDefaultChannelsAsync(int userId, int workspaceId);
        Task CreateChannelAsync(string channelName, int workspaceId);
        Task<Channel> GetChannelByIdAsync(int id, int workspaceId);
        Task<IEnumerable<Channel>> GetChannelsAsync(int userId, int workspaceId);
    }
}