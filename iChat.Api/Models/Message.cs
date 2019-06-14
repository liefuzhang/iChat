using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public abstract class Message
    {
        public int Id { get; protected set; }
        public int SenderId { get; protected set; }
        public User Sender { get; protected set; }
        public string Content { get; protected set; }
        public int WorkspaceId { get; protected set; }
        public Workspace Workspace { get; private set; }
        public DateTime CreatedDate { get; protected set; }
        public bool HasFileAttachments { get; set; }
        public ICollection<MessageFileAttachment> MessageFileAttachments { get; set; }
    }
}
