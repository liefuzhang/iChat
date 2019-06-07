using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Conversation
    {
        public Conversation(int workspaceId) : this()
        {
            if (workspaceId < 1)
            {
                throw new ArgumentException("Invalid argument");
            }

            WorkspaceId = workspaceId;

        }

        protected Conversation()
        {
            ConversationUsers = new HashSet<ConversationUser>();
        }

        public int Id { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ConversationUser> ConversationUsers { get; private set; }

    }
}
