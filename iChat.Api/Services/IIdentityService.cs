using System;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IIdentityService {
        Task<User> AuthenticateAsync(string email, string password);
       string GenerateAccessToken(int userId);
    }
}