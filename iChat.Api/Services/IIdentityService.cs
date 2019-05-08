using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IIdentityService
    {
        User Authenticate(string email, string password);
        void Register(User user, string password);
    }
}