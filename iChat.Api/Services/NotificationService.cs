using iChat.Api.Constants;
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

        public async Task SendChannelMessageItemChangeNotificationAsync(IEnumerable<int> userIds, int channelId, MessageChangeType type, int messageId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ChannelMessageItemChange", channelId, type, messageId);
            }
        }

        public async Task SendConversationMessageItemChangeNotificationAsync(IEnumerable<int> userIds, int conversationId, MessageChangeType type, int messageId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ConversationMessageItemChange", conversationId, type, messageId);
            }
        }

        public async Task SendUpdateChannelListNotificationAsync(IEnumerable<int> userIds, int channelId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateChannelList", channelId);
            }
        }

        public async Task SendUpdateConversationListNotificationAsync(IEnumerable<int> userIds, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateConversationList", conversationId);
            }
        }

        public async Task SendUserTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName,
            bool isChannel, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UserTyping", currentUserName, isChannel, conversationId);
            }
        }

        public async Task SendUpdateChannelDetailsNotificationAsync(IEnumerable<int> userIds, int channelId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateChannelDetails", channelId);
            }
        }

        public async Task SendUpdateConversationDetailsNotificationAsync(IEnumerable<int> userIds, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateConversationDetails", conversationId);
            }
        }
    }
}
