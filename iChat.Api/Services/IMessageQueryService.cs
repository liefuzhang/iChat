using iChat.Api.Dtos;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IMessageQueryService
    {
        Task<MessageLoadDto> GetMessagesForChannelAsync(int channelId, int userId, int workspaceId, int? currentMessageId);
        Task<MessageLoadDto> GetMessagesForConversationAsync(int conversationId, int userId, int workspaceId, int? currentMessageId);
        Task<MessageGroupDto> GetSingleMessageForChannelAsync(int channelId, int messageId, int userId);
        Task<MessageGroupDto> GetSingleMessagesForConversationAsync(int conversationId, int messageId, int userId);
    }
}