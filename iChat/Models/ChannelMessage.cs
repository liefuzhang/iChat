namespace iChat.Models {
    public class ChannelMessage : Message {
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}