using System;

namespace iChat.Api.Models {
    public class ChannelMessage : Message {
        protected ChannelMessage() { }
        public ChannelMessage(int channelId, string content, int senderId, int workspaceId,
            bool hasFileAttachments = false, bool isSystemMessage = false) {
            if (string.IsNullOrWhiteSpace(content) && !hasFileAttachments) {
                throw new Exception("Content cannot be empty");
            }
            if (channelId < 1) {
                throw new Exception("Invalid channel");
            }
            if (senderId < 1) {
                throw new Exception("Invalid sender");
            }
            if (workspaceId < 1) {
                throw new Exception("Invalid workspace");
            }

            ChannelId = channelId;
            Content = content;
            SenderId = senderId;
            WorkspaceId = workspaceId;
            HasFileAttachments = hasFileAttachments;
            IsSystemMessage = isSystemMessage;
            CreatedDate = DateTime.UtcNow;
        }
        public int ChannelId { get; private set; }
        public Channel Channel { get; private set; }
    }
}