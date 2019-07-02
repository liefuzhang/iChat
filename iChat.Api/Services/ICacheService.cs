using iChat.Api.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface ICacheService
    {
        Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int userId, int workspaceId);
        Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int userId, int workspaceId);
        Task AddRecentConversationForUserAsync(int conversationId, int userId, int workspaceId, bool incrementUnreadMessage = false);
        Task AddNewUnreadMessageForUsersAsync(int conversationId, IEnumerable<int> userIds, int workspaceId);
        Task<List<ConversationUnreadItem>> GetRecentConversationItemsForUserAsync(int userId, int workspaceId);
        Task ClearUnreadConversationMessageForUserAsync(int conversationId, int userId, int workspaceId);
        Task AddUnreadChannelForUsersAsync(int channelId, IEnumerable<int> userIds, int workspaceId,
            List<int> mentionUserIds = null);
        Task RemoveUnreadChannelForUserAsync(int channelId, int userId, int workspaceId);
        Task<List<ChannelUnreadItem>> GetUnreadChannelForUserAsync(int userId, int workspaceId);
        Task SetUserOnlineAsync(int userId, int workspaceId);
        Task<bool> GetUserOnlineAsync(int userId, int workspaceId);
    }
}