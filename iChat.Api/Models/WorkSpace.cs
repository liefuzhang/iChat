using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
