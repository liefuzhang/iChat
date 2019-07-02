using iChat.Api.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface INotificationService
    {
        Task SendChannelMessageItemChangeNotificationAsync(IEnumerable<int> userIds, int channelId,
            MessageChangeType type, int messageId);
        Task SendConversationMessageItemChangeNotificationAsync(IEnumerable<int> userIds, int conversationId,
            MessageChangeType type, int messageId);
        Task SendUpdateChannelListNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendUpdateConversationListNotificationAsync(IEnumerable<int> userIds, int conversationId);
        Task SendUserTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName, bool inChannel,
            int id);
        Task SendUserFinishedTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName,
            bool isChannel, int conversationId); Task SendUpdateChannelDetailsNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendUpdateConversationDetailsNotificationAsync(IEnumerable<int> userIds, int conversationId);
    }
}