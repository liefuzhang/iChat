﻿using iChat.Api.Constants;
using iChat.Api.Models;

namespace iChat.Api.Dtos {
    public class UserDto {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserStatus Status { get; set; }
        public string DisplayName { get; set; }
        public string IdenticonPath { get; set; }
        public int WorkspaceId { get; private set; }
    }
}