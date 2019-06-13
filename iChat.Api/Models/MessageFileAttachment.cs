using System;

namespace iChat.Api.Models {
    public class MessageFileAttachment {
        protected MessageFileAttachment() {
        }

        public MessageFileAttachment(int messageId, int fileId) {
            if (messageId < 1 || fileId < 1) {
                throw new ArgumentException("Invalid input");
            }

            MessageId = messageId;
            FileId = fileId;
        }

        public int MessageId { get; private set; }
        public Message Message { get; private set; }
        public int FileId { get; private set; }
        public File File { get; private set; }
    }
}