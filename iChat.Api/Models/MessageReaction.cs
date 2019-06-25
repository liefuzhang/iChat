using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class MessageReaction
    {
        protected MessageReaction()
        {
        }

        public MessageReaction(int messageId, string emojiColons)
        {
            if (messageId < 1)
            {
                throw new Exception("Invalid message");
            }
            if (string.IsNullOrWhiteSpace(emojiColons))
            {
                throw new Exception("Invalid emoji");
            }

            MessageId = messageId;
            EmojiColons = emojiColons;
            CreatedDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public int MessageId { get; private set; }
        public Message Message { get; private set; }
        public string EmojiColons { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public ICollection<MessageReactionUser> MessageReactionUsers { get; private set; }
    }
}