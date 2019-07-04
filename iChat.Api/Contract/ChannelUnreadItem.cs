using System.Collections.Generic;
using System.Linq;

namespace iChat.Api.Contract
{
    public class ChannelUnreadItem
    {
        public ChannelUnreadItem(int channelId)
        {
            ChannelId = channelId;
            UnreadMessages = new List<(int messageId, bool userMentioned)>();
        }

        public int ChannelId { get; set; }
        public List<(int messageId, bool userMentioned)> UnreadMessages { get; set; }
        public int UnreadMentionCount => UnreadMessages.Count(um => um.userMentioned);
        public int UnreadMessageCount => UnreadMessages.Count();

        public void AddUnreadMessage(int messageId, bool userMentioned)
        {
            UnreadMessages.Add((messageId, userMentioned));
        }

        public void UpdateUnreadMessageMention(int messageId, bool userMentioned)
        {
            var index = UnreadMessages.FindIndex(um => um.messageId == messageId);
            if (index > -1)
            {
                // Note tuple is a value type, so replace the whole item here
                UnreadMessages[index] = (messageId, userMentioned);
            }
        }

        public void ClearUnreadMessageId(int messageId)
        {
            var message = UnreadMessages.SingleOrDefault(um => um.messageId == messageId);
            if (message.messageId > 0)
            {
                UnreadMessages.Remove(message);
            }
        }
    }
}
