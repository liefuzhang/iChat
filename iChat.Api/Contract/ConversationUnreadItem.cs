using System.Collections.Generic;

namespace iChat.Api.Contract
{
    public class ConversationUnreadItem
    {
        public ConversationUnreadItem(int conversationId)
        {
            ConversationId = conversationId;
            UnreadMessageIds = new List<int>();
        }

        public int ConversationId { get; set; }
        public List<int> UnreadMessageIds { get; set; }
        public int UnreadMessageCount => UnreadMessageIds.Count;

        public void AddUnreadMessageId(int messageId)
        {
            UnreadMessageIds.Add(messageId);
        }

        public void ClearAllUnreadMessageIds()
        {
            UnreadMessageIds.Clear();
        }

        public void ClearUnreadMessageId(int messageId)
        {
            UnreadMessageIds.Remove(messageId);
        }
    }
}
