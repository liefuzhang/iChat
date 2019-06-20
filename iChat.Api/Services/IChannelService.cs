﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IChannelService {
        Task AddDefaultChannelsToNewWorkplaceAsync(int workspaceId);
        Task AddUserToChannelAsync(int channelId, int userId, int workspaceId);
        Task RemoveUserFromChannelAsync(int channelId, int userId);
        Task AddUserToDefaultChannelsAsync(int userId, int workspaceId);
        Task<int> CreateChannelAsync(string channelName, int workspaceId, string topic = "");
        bool IsUserSubscribedToChannel(int channelId, int userId);
        Task<ChannelDto> GetChannelByIdAsync(int id, int userId, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId);
        Task<int> GetDefaultChannelGeneralIdAsync(int workspaceId);
        Task<IEnumerable<int>> GetAllChannelUserIdsAsync(int channelId);
        Task NotifyTypingAsync(int channelId, int currentUserId, int workspaceId);
    }
}