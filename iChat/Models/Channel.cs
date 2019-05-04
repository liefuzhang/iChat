using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public ICollection<ChannelMessage> ChannelMessages { get; set; }
        public ICollection<Subscription> ChannelSubscriptions { get; set; }
    }
}
