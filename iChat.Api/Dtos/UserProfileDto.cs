﻿using iChat.Api.Models;

namespace iChat.Api.Dtos {
    public class UserProfileDto {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkspaceName { get; set; }
        public string IdenticonPath { get; set; }
        public int DefaultChannelId { get; set; }
        public string Token { get; set; }
    }
}