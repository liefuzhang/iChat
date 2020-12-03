using iChat.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace iChat.Api.Models
{
    public class Conversation
    {
        public Conversation(int userId, int workspaceId, bool isPrivate = false, bool isSelfConversation = false) : this()
        {
            if (userId < 1 || workspaceId < 1)
            {
                throw new ArgumentException("Invalid argument");
            }

            CreatedByUserId = userId;
            WorkspaceId = workspaceId;
            IsPrivate = isPrivate;
            IsSelfConversation = isSelfConversation;
            CreatedDate = DateTime.UtcNow;
        }

        protected Conversation()
        {
            ConversationUsers = new HashSet<ConversationUser>();
        }

        public int Id { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public int CreatedByUserId { get; private set; }
        public User CreatedByUser { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public bool IsPrivate { get; private set; }
        public bool IsSelfConversation { get; private set; }
        public ICollection<ConversationUser> ConversationUsers { get; private set; }
        public string CreatedDateString => CreatedDate.ConvertToNzTimeZone().ToString("dddd, MMM dd", CultureInfo.InvariantCulture);
    }
}
