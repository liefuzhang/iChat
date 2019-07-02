namespace iChat.Api.Models
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsSelfConversation { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedDateString { get; set; }
        public string Name { get; set; }
        public int UnreadMessageCount { get; set; }
        public int UserCount { get; set; }
        public bool IsTheOtherUserOnline { get; set; }
    }
}
