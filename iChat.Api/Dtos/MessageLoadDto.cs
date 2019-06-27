using System.Collections.Generic;

namespace iChat.Api.Dtos {
    public class MessageLoadDto {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<MessageGroupDto> MessageGroupDtos { get; set; }
    }
}