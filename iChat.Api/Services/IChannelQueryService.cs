using iChat.Api.Dtos;
using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IChannelQueryService
    {
        bool IsUserSubscribedToChannel(int channelId, int userId);
        Task<ChannelDto> GetChannelByIdAsync(int id, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetChannelsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<ChannelDto>> GetAllUnsubscribedChannelsForUserAsync(int userId, int workspaceId);
        Task<Channel> GetChannelByNameAsync(string name, int workspaceId);
        Task<int> GetDefaultChannelGeneralIdAsync(int workspaceId);
        Task<IEnumerable<int>> GetAllChannelUserIdsAsync(int channelId);
        Task<IEnumerable<UserDto>> GetAllChannelUsersAsync(int channelId);
    }
}