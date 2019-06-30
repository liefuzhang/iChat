using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IChannelService
    {
        Task AddDefaultChannelsToNewWorkplaceAsync(int userId, int workspaceId);
        Task AddUserToChannelAsync(int channelId, int userId, int workspaceId);
        Task RemoveUserFromChannelAsync(int channelId, int userId);
        Task AddUserToDefaultChannelsAsync(int userId, int workspaceId);
        Task<int> CreateChannelAsync(string channelName, int userId, int workspaceId, string topic = "");
        bool IsUserSubscribedToChannel(int channelId, int userId);
        Task<ChannelDto> GetChannelByIdAsync(int id, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId);
        Task<int> GetDefaultChannelGeneralIdAsync(int workspaceId);
        Task<IEnumerable<int>> GetAllChannelUserIdsAsync(int channelId);
        Task NotifyTypingAsync(int channelId, int currentUserId, int workspaceId);
        Task InviteOtherMembersToChannelAsync(int id, List<int> userIds, int userId);
    }
}