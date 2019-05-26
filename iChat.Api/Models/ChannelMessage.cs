using System;

namespace iChat.Api.Models {
    public class ChannelMessage : Message {
        protected ChannelMessage() { }
        public ChannelMessage(int channelId, string content, int senderId, int workspaceId) {
            if (string.IsNullOrWhiteSpace(content)) {
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
            CreatedDate = DateTime.Now;
        }
        public int ChannelId { get; protected set; }
        public Channel Channel { get; protected set; }
    }
}