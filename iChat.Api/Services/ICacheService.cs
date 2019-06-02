using iChat.Api.Contract;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public interface ICacheService {
        Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int workspaceId, int userId);
        Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int workspaceId, int userId);
    }
}