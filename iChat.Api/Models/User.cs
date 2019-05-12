using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class User {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedDate { get; set; }
        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; set; }
    }
}
