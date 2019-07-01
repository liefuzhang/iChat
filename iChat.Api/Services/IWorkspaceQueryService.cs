using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IWorkspaceQueryService
    {
        Task<Workspace> GetWorkspaceByIdAsync(int workspaceId);
    }
}