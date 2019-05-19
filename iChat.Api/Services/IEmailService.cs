using iChat.Api.Constants;
using iChat.Api.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public interface IEmailService {
        Task SendUserInvitationEmailAsync(UserInvitationData data);
    }
}