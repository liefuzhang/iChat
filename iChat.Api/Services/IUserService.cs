using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
    }
}