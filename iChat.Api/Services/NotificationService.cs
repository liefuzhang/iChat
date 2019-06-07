using iChat.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendUpdateChannelNotificationAsync(IEnumerable<int> userIds, int channelId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateChannel", channelId);
            }
        }

        public async Task SendUpdateConversationNotificationAsync(IEnumerable<int> userIds, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateConversation", conversationId);
            }
        }
    }
}
