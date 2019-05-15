using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IWorkspaceService {
        Task<int> RegisterAsync(string name);
        Task UpdateOwnerIdAsync(int workspaceId, int userId);
        Task<Workspace> GetWorkspaceByIdAsync(int workspaceId);
    }
}