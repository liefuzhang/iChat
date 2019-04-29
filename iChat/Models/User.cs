using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace iChat.Models
{
    public class User : IdentityUser<int>
    {
        public UserStatus Status { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
    }
}
