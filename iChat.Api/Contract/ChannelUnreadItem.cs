using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Contract {
    public class ChannelUnreadItem
    {
        public ChannelUnreadItem(int channelId) {
            ChannelId = channelId;
        }

        public int ChannelId { get; set; }
        public int UnreadMentionCount { get; set; }

        public void IncrementUnreadMention() {
            UnreadMentionCount++;
        }

        internal void ClearUnreadMention() {
            UnreadMentionCount = 0;
        }
    }
}
