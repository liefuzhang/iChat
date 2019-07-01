using iChat.Api.Dtos;
using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Constants;

namespace iChat.Api.Services
{
    public interface IUserQueryService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id, int workspaceId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int workspaceId);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<string> GetUserStatus(int userId, int workspaceId);
    }
}