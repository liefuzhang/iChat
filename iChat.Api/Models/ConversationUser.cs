using System;

namespace iChat.Api.Models
{
    public class ConversationUser
    {
        protected ConversationUser()
        {
        }

        public ConversationUser(int conversationId, int userId)
        {
            if (conversationId < 1 || userId < 1)
            {
                throw new ArgumentException("Invalid input");
            }

            ConversationId = conversationId;
            UserId = userId;
        }

        public int ConversationId { get; private set; }
        public Conversation Conversation { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
    }
}
