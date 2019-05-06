using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace iChat.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        //public async Task SendMessage(int channelID)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", channelID);
        //}

        public async Task AddToChannelGroup(string channelID) {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelID);
        }
    }
}
