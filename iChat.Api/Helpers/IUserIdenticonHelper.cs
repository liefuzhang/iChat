using System;
using System.Threading.Tasks;

namespace iChat.Api.Helpers {
    public interface IUserIdenticonHelper {
        Task<Guid> GenerateUserIdenticon(string email);
    }
}