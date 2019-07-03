using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Contract {
    public class ChannelUnreadItem {
        public ChannelUnreadItem(int channelId) {
            ChannelId = channelId;
            UnreadMessages = new List<(int messageId, bool userMentioned)>();
        }

        public int ChannelId { get; set; }
        public List<(int messageId, bool userMentioned)> UnreadMessages { get; set; }
        public int UnreadMentionCount => UnreadMessages.Count(um => um.userMentioned);
        public int UnreadMessageCount => UnreadMessages.Count();

        public void AddUnreadMessage((int, bool) messageItem) {
            UnreadMessages.Add(messageItem);
        }

        public void ClearUnreadMessageId(int messageId) {
            var message = UnreadMessages.SingleOrDefault(um => um.messageId == messageId);
            if (message.messageId > 0) {
                UnreadMessages.Remove(message);
            }
        }
    }
}
