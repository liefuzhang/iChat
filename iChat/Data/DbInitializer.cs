using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Models;

namespace iChat.Data {
    public static class DbInitializer {
        public static void Initialize(iChatContext context) {
            //context.Database.EnsureCreated();

            if (context.Channels.Any()) {
                return; // DB has been seeded
            }

            var channels = new Channel[] {
                new Channel {Name= "general", Topic = "Anything that's talkable"},
                new Channel {Name= "random", Topic = "Another random channel"}
            };
            context.Channels.AddRange(channels);
            context.SaveChanges();

            var users = new User[] {
                new User {
                    DisplayName = "Liefu",
                    Phone = "0223334444",
                    Status = UserStatus.Active
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}