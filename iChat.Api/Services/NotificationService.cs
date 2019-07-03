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

        public async Task SendChannelMessageItemChangedNotificationAsync(IEnumerable<int> userIds, int channelId, MessageChangeType type, int messageId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ChannelMessageItemChanged", channelId, type, messageId);
            }
        }

        public async Task SendConversationMessageItemChangedNotificationAsync(IEnumerable<int> userIds, int conversationId, MessageChangeType type, int messageId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ConversationMessageItemChanged", conversationId, type, messageId);
            }
        }

        public async Task SendUnreadChannelRemovedNotificationAsync(IEnumerable<int> userIds, int channelId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UnreadChannelRemoved", channelId);
            }
        }

        public async Task SendUnreadConversationClearedNotificationAsync(IEnumerable<int> userIds, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UnreadConversationCleared", conversationId);
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

        public async Task SendUserFinishedTypingNotificationAsync(IEnumerable<int> userIds, string currentUserName,
            bool isChannel, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UserFinishedTyping", currentUserName, isChannel, conversationId);
            }
        }

        public async Task SendChannelUserListChangedNotificationAsync(IEnumerable<int> userIds, int channelId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ChannelUserListChanged", channelId);
            }
        }

        public async Task SendConversationUserListChangedNotificationAsync(IEnumerable<int> userIds, int conversationId)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ConversationUserListChanged", conversationId);
            }
        }

        public async Task SendUserOnlineNotificationAsync(IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("UserWentOnline");
            }
        }
    }
}
