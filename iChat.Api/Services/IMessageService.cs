using iChat.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageGroupDto>> GetMessagesForChannelAsync(int channelId, int workspaceId);
        Task<IEnumerable<MessageGroupDto>> GetMessagesForConversationAsync(int conversationId, int workspaceId);
        Task PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId, int workspaceId);
        Task PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId);
    }
}