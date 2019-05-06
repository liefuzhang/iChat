namespace iChat.Api.Models
{
    public class ChannelSubscription
    {
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
