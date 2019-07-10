using iChat.Api.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface ICacheService
    {
        Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int userId, int workspaceId);
        Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int userId, int workspaceId);
        Task AddRecentConversationForUserAsync(int conversationId, int userId, int workspaceId,
            bool addUnreadMessage = false, int messageId = 0);
        Task AddUnreadConversationMessageForUsersAsync(int conversationId,
            int messageId, IEnumerable<int> userIds, int workspaceId);
        Task<List<ConversationUnreadItem>> GetRecentConversationItemsForUserAsync(int userId, int workspaceId);
        Task ClearAllUnreadConversationMessageIdsForUserAsync(int conversationId, int userId, int workspaceId);
        Task AddUnreadChannelMessageForUsersAsync(int channelId, int messageId, IEnumerable<int> userIds, int workspaceId,
            List<int> mentionUserIds = null);
        Task ClearUnreadChannelForUserAsync(int channelId, int userId, int workspaceId);
        Task ClearUnreadChannelMessageForUserAsync(int channelId, int messageId, int userId, int workspaceId);
        Task<List<ChannelUnreadItem>> GetUnreadChannelsForUserAsync(int userId, int workspaceId);
        Task SetUserOnlineAsync(int userId, int workspaceId);
        Task<bool> GetUserOnlineAsync(int userId, int workspaceId);
        Task ClearUnreadConversationMessageIdForUserAsync(int conversationId, int messageId, int userId, int workspaceId);

        Task UpdateUnreadChannelMessageMentionForUsersAsync(int channelId, int messageId,
            IEnumerable<int> userIds, int workspaceId, List<int> mentionUserIds);
    }
}