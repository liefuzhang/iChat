using System;

namespace iChat.Api.Models {
    public class DirectMessage : Message {
        protected DirectMessage() { }
        public DirectMessage(int receiverId, string content, int senderId, int workspaceId) {
            if (string.IsNullOrWhiteSpace(content)) {
                throw new Exception("Content cannot be empty");
            }
            if (receiverId < 1) {
                throw new Exception("Invalid receiver");
            }
            if (senderId < 1) {
                throw new Exception("Invalid sender");
            }
            if (workspaceId < 1) {
                throw new Exception("Invalid workspace");
            }

            ReceiverId = receiverId;
            Content = content;
            SenderId = senderId;
            WorkspaceId = workspaceId;
            CreatedDate = DateTime.Now;
        }

        public int ReceiverId { get; private set; }
        public User Receiver { get; private set; }
    }
}