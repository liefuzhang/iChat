using System.Threading.Tasks;
using iChat.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace iChat.Api.Services {
    public class NotificationService : INotificationService {
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext) {
            _hubContext = hubContext;
        }

        public async Task SendUpdateChannelNotification(int channelID) {
            await _hubContext.Clients.Group(channelID.ToString()).SendAsync("UpdateChannel", channelID);
        }

        public async Task SendDirectMessageNotification(int selectedUserId) {
            await _hubContext.Clients.User(selectedUserId.ToString()).SendAsync("ReceiveMessage");
        }
    }
}
