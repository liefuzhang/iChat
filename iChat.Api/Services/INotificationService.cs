using iChat.Api.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface INotificationService
    {
        Task SendChannelMessageItemChangedNotificationAsync(IEnumerable<int> userIds, int channelId,
            MessageChangeType type, int messageId);
        Task SendConversationMessageItemChangedNotificationAsync(IEnumerable<int> userIds, int conversationId,
            MessageChangeType type, int messageId);
        Task SendUnreadChannelRemovedNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendUnreadConversationClearedNotificationAsync(IEnumerable<int> userIds, int conversationId);
        Task SendUserTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName, bool inChannel,
            int id);
        Task SendUserFinishedTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName,
            bool isChannel, int conversationId); Task SendChannelUserListChangedNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendConversationUserListChangedNotificationAsync(IEnumerable<int> userIds, int conversationId);
        Task SendUserOnlineNotificationAsync(IEnumerable<int> userIds);
    }
}