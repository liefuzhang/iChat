using iChat.Api.Constants;
using iChat.Api.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Helpers {
    public interface IEmailHelper {
        Task SendUserInvitationEmailAsync(UserInvitationData data);
        Task SendResetPasswordEmailAsync(string receiverAddress, Guid resetCode);
    }
}