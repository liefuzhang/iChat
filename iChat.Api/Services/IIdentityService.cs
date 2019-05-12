using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IIdentityService
    {
        User Authenticate(string email, string password);
        int Register(string email, string password, int workspaceId);
        void ValidateUserEmailAndPassword(string email, string password);
    }
}