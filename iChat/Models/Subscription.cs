using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Models
{
    public class Subscription
    {
        public int ChannelID { get; set; }
        public Channel Channel { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
