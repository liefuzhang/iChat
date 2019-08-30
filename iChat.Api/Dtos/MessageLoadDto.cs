using System.Collections.Generic;

namespace iChat.Api.Dtos {
    public class MessageLoadDto {
        public MessageChannelDescriptionDto MessageChannelDescriptionDto { get; set; }
        public bool AllMessagesLoaded { get; set; }
        public IEnumerable<MessageGroupDto> MessageGroupDtos { get; set; }
    }
}