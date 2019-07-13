using iChat.Api.Dtos;
using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Constants;

namespace iChat.Api.Services {
    public interface IUserQueryService {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByIdAsync(int id, int workspaceId);
        Task<string> GetUserNamesAsync(List<int> userIds, int workspaceId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(int workspaceId);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<string> GetUserStatus(int userId, int workspaceId);
    }
}