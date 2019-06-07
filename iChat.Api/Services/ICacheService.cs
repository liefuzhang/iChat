using iChat.Api.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public interface ICacheService {
        Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int userId, int workspaceId);
        Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int userId, int workspaceId);
        Task AddRecentConversationIdForUserAsync(int conversationId, int userId, int workspaceId);
        Task<List<int>> GetRecentConversationIdsForUserAsync(int userId, int workspaceId);
    }
}