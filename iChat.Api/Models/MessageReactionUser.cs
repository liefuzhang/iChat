using System;

namespace iChat.Api.Models {
    public class MessageReactionUser {
        protected MessageReactionUser() {
        }
        public MessageReactionUser(int messageReactionId, int userId) {
            if (messageReactionId < 1) {
                throw new Exception("Invalid message reaction");
            }
            if (userId < 1) {
                throw new Exception("Invalid user");
            }

            MessageReactionId = messageReactionId;
            UserId = userId;
        }

        public int MessageReactionId { get; private set; }
        public MessageReaction MessageReaction { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
    }
}