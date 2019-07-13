using iChat.Api.Dtos;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IIdentityService
    {
        Task<UserProfileDto> AuthenticateAsync(string email, string password);
        Task<UserProfileDto> GetUserProfileAsync(int userId);
    }
}