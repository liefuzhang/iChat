using System;

namespace iChat.Api.Models
{
    public class MessageFileAttachment
    {
        public MessageFileAttachment() { }

        public MessageFileAttachment(Message message, File file)
        {
            if (message == null || file == null)
            {
                throw new ArgumentException("Invalid input");
            }

            Message = message;
            File = file;
        }

        public int MessageId { get; private set; }
        public Message Message { get; private set; }
        public int FileId { get; private set; }
        public File File { get; private set; }
    }
}