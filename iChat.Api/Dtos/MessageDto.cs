using System;
using System.Collections.Generic;
using iChat.Api.Models;

namespace iChat.Api.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public UserDto Sender { get; set; }
        public string Content { get; set; }
        public int WorkspaceId { get; set; }
        public string TimeString { get; set; }
    }
}