using iChat.Api.Constants;

namespace iChat.Api.Dtos
{
    public class MessageItemChangeDto
    {
        public MessageChangeType Type { get; set; }
        public int MessageId { get; set; }
    }
}