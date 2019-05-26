using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Dtos;
using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id, int workspaceId);
        Task<User> GetUserByEmailAsync(string email, int workspaceId);
        Task<IEnumerable<User>> GetAllUsersAsync(int workspaceId);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<int> RegisterAsync(string email, string password, int workspaceId, string name = null);
        Task InviteUsersAsync(User user, Workspace workspace, List<string> emails);
        Task<int> AcceptInvitationAsync(UserInvitationDto userInvitationDto);
    }
}