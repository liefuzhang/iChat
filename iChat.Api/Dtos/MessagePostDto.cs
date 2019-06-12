using System;
using System.Collections.Generic;
using iChat.Api.Models;

namespace iChat.Api.Dtos
{
    public class MessagePostDto
    {
        public string Message { get; set; }
        public List<int> MentionUserIds { get; set; }
    }
}