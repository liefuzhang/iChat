using System;
using System.Collections.Generic;
using System.Globalization;
using iChat.Api.Extensions;

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
        public bool IsSystemMessage { get; protected set; }
        public ICollection<MessageFileAttachment> MessageFileAttachments { get; protected set; }
        public ICollection<MessageReaction> MessageReactions { get; protected set; }

        public DateTime LocalizedCreatedDate => CreatedDate.ConvertToNzTimeZone();

        public string DateString
        {
            get
            {
                if (LocalizedCreatedDate.Date == DateTime.UtcNow.ConvertToNzTimeZone().Date)
                {
                    return "Today";
                }

                if (LocalizedCreatedDate.AddDays(1).Date == DateTime.UtcNow.ConvertToNzTimeZone().Date)
                {
                    return "Yesterday";
                }                

                return LocalizedCreatedDate.ToString("dddd, MMM dd", CultureInfo.InvariantCulture);
            }
        }

        public string TimeString => LocalizedCreatedDate.ToString("h:mm tt", CultureInfo.InvariantCulture);
            
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
