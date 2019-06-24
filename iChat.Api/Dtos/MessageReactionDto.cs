using iChat.Api.Dtos;
using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class MessageReactionDto {
        public int Id { get; private set; }
        public int MessageId { get; private set; }
        public string EmojiColons { get; private set; }
        public List<UserDto> Users { get; private set; }
    }
}