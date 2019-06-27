using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class UserInvitation {
        protected UserInvitation() { }

        public UserInvitation(string userEmail, int invitedByUserId,
            int workspaceId) {
            if (string.IsNullOrWhiteSpace(userEmail)) {
                throw new Exception("Email cannot be empty");
            }
            if (invitedByUserId < 1) {
                throw new Exception("Invalid invitedByUser");
            }
            if (workspaceId < 1) {
                throw new Exception("Invalid workspaceId");
            }

            InvitedByUserId = invitedByUserId;
            WorkspaceId = workspaceId;
            UserEmail = userEmail;
            InviteDate = DateTime.Now;
            InvitationCode = Guid.NewGuid();
        }

        public int Id { get; private set; }
        public string UserEmail { get; private set; }
        public int InvitedByUserId { get; private set; }
        public User InvitedByUser { get; private set; }
        public int WorkspaceId { get; private set; }
        public DateTime InviteDate { get; private set; }
        public bool Acceptted { get; private set; }
        public bool Cancelled { get; private set; }
        public Guid InvitationCode { get; private set; }

        public void Accept() {
            Acceptted = true;
        }

        public void Cancel() {
            Cancelled = true;
        }
    }
}
