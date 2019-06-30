using System;
using System.Collections.Generic;
using System.Globalization;

namespace iChat.Api.Models {
    public class Conversation {
        public Conversation(int userId, int workspaceId) : this() {
            if (userId < 1 || workspaceId < 1) {
                throw new ArgumentException("Invalid argument");
            }

            CreatedByUserId = userId;
            WorkspaceId = workspaceId;
            CreatedDate = DateTime.Now;
        }

        protected Conversation() {
            ConversationUsers = new HashSet<ConversationUser>();
        }

        public int Id { get; private set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ConversationUser> ConversationUsers { get; private set; }
        public string CreatedDateString => CreatedDate.ToString("dddd, MMM d", CultureInfo.InvariantCulture);
    }
}
