using System;
using System.Collections.Generic;
using System.Globalization;

namespace iChat.Api.Models
{
    public abstract class Message
    {
        protected Message()
        {
            MessageFileAttachments = new HashSet<MessageFileAttachment>();
        }

        public int Id { get; protected set; }
        public int SenderId { get; protected set; }
        public User Sender { get; protected set; }
        public string Content { get; protected set; }
        public int WorkspaceId { get; protected set; }
        public Workspace Workspace { get; private set; }
        public DateTime CreatedDate { get; protected set; }
        public bool HasFileAttachments { get; protected set; }
        public bool ContentEdited { get; protected set; }
        //public bool IsSystemMessage { get; protected set; }
        public ICollection<MessageFileAttachment> MessageFileAttachments { get; protected set; }
        public ICollection<MessageReaction> MessageReactions { get; protected set; }

        public string DateString => CreatedDate.ToString("dddd, MMM dd", CultureInfo.InvariantCulture);
        public string TimeString => CreatedDate.ToString("h:mm tt", CultureInfo.InvariantCulture);

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Content cannot be empty");
            }

            Content = content;
            ContentEdited = true;
        }

        public void AddMessageFileAttachment(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            MessageFileAttachments.Add(new MessageFileAttachment(this, file));
        }
    }
}
