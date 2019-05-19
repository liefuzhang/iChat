using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Contract
{
    public class UserInvitationData
    {
        public List<string> ReceiverAddresses { get; set; }
        public string InviterName { get; set; }
        public string InviterEmail { get; set; }
        public string WorkspaceName{ get; set; }
        public string JoinUrl { get; set; }
    }
}
