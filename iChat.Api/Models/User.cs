using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int WorkSpaceId { get; set; }
        public WorkSpace WorkSpace { get; set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; set; }
    }
}
