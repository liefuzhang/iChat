using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface INotificationService
    {
        Task SendNewChannelMessageNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendNewConversationMessageNotificationAsync(IEnumerable<int> userIds, int conversationId);
        Task SendUpdateChannelListNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendUpdateConversationListNotificationAsync(IEnumerable<int> userIds, int conversationId);
    }
}