using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IWorkspaceCommandService
    {
        Task<int> RegisterAsync(string name);
        Task UpdateOwnerIdAsync(int workspaceId, int userId);
    }
}