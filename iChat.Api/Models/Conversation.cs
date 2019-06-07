using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Conversation
    {
        public Conversation(string name, int workspaceId) : this()
        {
            if (string.IsNullOrWhiteSpace(name) || workspaceId < 1)
            {
                throw new ArgumentException("Invalid argument");
            }

            Name = name;
            WorkspaceId = workspaceId;

        }

        protected Conversation()
        {
            ConversationUsers = new HashSet<ConversationUser>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ConversationUser> ConversationUsers { get; private set; }

    }
}
