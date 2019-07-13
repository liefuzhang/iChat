using iChat.Api.Dtos;
using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Constants;

namespace iChat.Api.Services
{
    public interface IUserCommandService
    {
        Task<int> RegisterAsync(string email, string password, string displayName, int workspaceId);
        Task InviteUsersAsync(UserDto user, Workspace workspace, List<string> emails);
        Task<int> AcceptInvitationAsync(UserInvitationDto userInvitationDto);
        Task ForgotPasswordAsync(string email);
        Task SetUserStatusAsync(int userId, int workspaceId, UserStatus status);
        Task ClearUserStatusAsync(int userId, int workspaceId);
        Task EditProfile(UserEditDto userDto, int userId);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task SetUserOnlineAsync(int userId, int workspaceId);
    }
}