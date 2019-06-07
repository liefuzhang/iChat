using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface INotificationService
    {
        Task SendUpdateChannelNotificationAsync(IEnumerable<int> userIds, int channelId);
        Task SendUpdateConversationNotificationAsync(IEnumerable<int> userIds, int conversationId);
    }
}