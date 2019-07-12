namespace iChat.Api.Dtos {
    public class MessageChannelDescriptionDto {
        public UserDto CreatedByUser { get; set; }
        public string CreatedDateString { get; set; }
        public string MessageChannelName { get; set; }
        public bool IsSelfConversation { get; set; }
    }
}