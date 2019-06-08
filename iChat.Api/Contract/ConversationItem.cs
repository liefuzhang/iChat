using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Contract {
    public class ConversationItem {
        public ConversationItem(int conversationId) {
            ConversationId = conversationId;
        }

        public int ConversationId { get; set; }
        public int UnreadMessageCount { get; set; }

        public void IncrementUnreadMessage() {
            UnreadMessageCount++;
        }
    }
}
