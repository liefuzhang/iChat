using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class UserInvitation {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int InvitedByUserId { get; set; }
        public User InvitedByUser { get; set; }
        public int WorkspaceId { get; set; }
        public DateTime InviteDate { get; set; }
        public bool Acceptted { get; set; }
        public Guid InvitationCode { get; set; }
    }
}
