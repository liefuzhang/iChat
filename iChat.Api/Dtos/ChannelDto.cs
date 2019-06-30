using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class ChannelDto {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Topic { get; private set; }
        public int CreatedByUserId { get; set; }
        public string CreatedDateString { get; set; }
        public bool HasUnreadMessage { get; set; }
        public int UnreadMentionCount { get; set; }
    }
}
