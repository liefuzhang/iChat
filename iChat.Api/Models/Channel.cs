using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public ICollection<ChannelMessage> ChannelMessages { get; set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; set; }
    }
}
