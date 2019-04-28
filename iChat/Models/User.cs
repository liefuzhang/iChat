using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Models
{
    public class User
    {
        public int ID { get; set; }
        public UserStatus Status { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
    }
}
