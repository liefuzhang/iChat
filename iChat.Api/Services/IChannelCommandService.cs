using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IChannelCommandService
    {
        Task AddDefaultChannelsToNewWorkplaceAsync(int userId, int workspaceId);
        Task AddUserToChannelAsync(int channelId, int userId, int workspaceId);
        Task RemoveUserFromChannelAsync(int channelId, int userId);
        Task AddUserToDefaultChannelsAsync(int userId, int workspaceId);
        Task<int> CreateChannelAsync(string channelName, int userId, int workspaceId, string topic = "");
        Task NotifyTypingAsync(int channelId, int currentUserId, int workspaceId);
        Task InviteOtherMembersToChannelAsync(int id, List<int> userIds, int userId, int workspaceId);
    }
}