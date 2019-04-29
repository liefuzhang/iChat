using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace iChat.Hubs
{
    public class ChatHub : Hub
    {
        //public async Task SendMessage(int channelID)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", channelID);
        //}
    }
}
