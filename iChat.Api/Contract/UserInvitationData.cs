using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Contract {
    public class UserInvitationData {
        public string ReceiverAddress { get; set; }
        public string InviterName { get; set; }
        public string InviterEmail { get; set; }
        public string WorkspaceName { get; set; }
        public Guid InvitationCode { get; set; }
    }
}
