using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesForChannelAsync(int channelId, int workspaceId);
        Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId, int currentUserId, int workspaceId);
        Task PostMessageToUserAsync(string newMessage, int userId, int currentUserId, int workspaceId);
        Task PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId);
    }
}