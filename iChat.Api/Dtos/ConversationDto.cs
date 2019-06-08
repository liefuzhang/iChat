using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class ConversationDto {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public string Name { get; set; }
        public int UnreadMessageCount { get; set; }
    }
}
