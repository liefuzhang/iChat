using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace iChat.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendUpdateChannelNotification(int channelID)
        {
            await _hubContext.Clients.All.SendAsync("UpdateChannel", channelID);
        }
    }
}
