using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Models;
using Microsoft.EntityFrameworkCore;

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

            var addUsersScript =
                $"SET IDENTITY_INSERT [dbo].[User] ON\r\n" +
                $"INSERT INTO [dbo].[User] ([Id], [Status], [DisplayName], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (1, 0, NULL, 0, N\'e5767433-8e39-4525-94f7-ef8db488c0e0\', N\'liefu@test.com\', 0, 1, NULL, N\'LIEFU@TEST.COM\', N\'LIEFU@TEST.COM\', N\'AQAAAAEAACcQAAAAEOD1Vx3/76pCyWDtxxHcfzlLaEaTEJ9kBwc0lLteLBjzJFV8q9b/dJkbKomMJsA7PA==\', NULL, 0, N\'PPQJWV4KNQCFIY4NYZB4JRGKXYBWNLZY\', 0, N\'liefu@test.com\')\r\n" +
                $"INSERT INTO [dbo].[User] ([Id], [Status], [DisplayName], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (2, 0, NULL, 0, N\'30f16c28-2873-469b-9be0-06ca12c81725\', N\'lina@test.com\', 0, 1, NULL, N\'LINA@TEST.COM\', N\'LINA@TEST.COM\', N\'AQAAAAEAACcQAAAAEPN7rGTRYxwI8P6ucQFLETRKbDrD/HbOdwti+DmgKgASU3Oxjy4JtGqWokTQh7tkKw==\', NULL, 0, N\'ZV6FV75VXWS55CQ7FDH7DMGVEJMOSHKS\', 0, N\'lina@test.com\')\r\n" +
                $"SET IDENTITY_INSERT [dbo].[User] OFF\r\n";
            context.Database.ExecuteSqlCommand(addUsersScript);
        }
    }
}