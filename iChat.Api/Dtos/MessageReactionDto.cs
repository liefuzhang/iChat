using iChat.Api.Dtos;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class MessageReactionDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string EmojiColons { get; set; }
        public List<UserDto> Users { get; set; }
    }
}