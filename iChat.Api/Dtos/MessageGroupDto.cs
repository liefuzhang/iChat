using System.Collections.Generic;

namespace iChat.Api.Dtos
{
    public class MessageGroupDto
    {
        public string DateString { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
    }
}