using System.Threading.Tasks;
using iChat.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace iChat.Api.Services {
    public class NotificationService : INotificationService {
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext) {
            _hubContext = hubContext;
        }

        public async Task SendUpdateChannelNotification(int channelId) {
            await _hubContext.Clients.Group(channelId.ToString()).SendAsync("UpdateChannel", channelId);
        }

        public async Task SendDirectMessageNotification(int userId) {
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessage");
        }
    }
}
