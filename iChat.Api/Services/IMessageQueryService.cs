using iChat.Api.Dtos;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IMessageQueryService
    {
        Task<MessageLoadDto> GetMessagesForChannelAsync(int channelId, int userId, int workspaceId, int currentPage);
        Task<MessageLoadDto> GetMessagesForConversationAsync(int conversationId, int userId, int workspaceId, int currentPage);
        Task<MessageGroupDto> GetSingleMessagesForChannelAsync(int channelId, int messageId, int userId);
        Task<MessageGroupDto> GetSingleMessagesForConversationAsync(int conversationId, int messageId, int userId);
    }
}