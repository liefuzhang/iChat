//using System.Linq;
//using iChat.Api.Models;
//using iChat.Api.Data;
//using Microsoft.EntityFrameworkCore;

//namespace iChat.Api.Data {
//    public static class DbInitializer {
//        public static void Initialize(iChatContext context) {
//            if (context.Channels.Any()) {
//                return; // DB has been seeded
//            }

//            var addWorkspaceScript =
//                $"SET IDENTITY_INSERT [dbo].[Workspaces] ON; " +
//                $"INSERT INTO[dbo].[Workspaces] ([Id], [Name], [OwnerId], [CreatedDate]) VALUES(1, N'Workspace1', 2, N'2019-05-11');" +
//                $"SET IDENTITY_INSERT[dbo].[Workspaces] OFF";
//            context.Database.ExecuteSqlCommand(addWorkspaceScript);

//            var channels = new Channel[] {
//                new Channel {Name= "general", Topic = "Anything that's talkable", WorkspaceId = 1},
//                new Channel {Name= "random", Topic = "Another random channel", WorkspaceId = 1}
//            };
//            context.Channels.AddRange(channels);
//            context.SaveChanges();

//            var addUsersScript =
//                $"SET IDENTITY_INSERT [dbo].[Users] ON; " +
//                $"INSERT INTO[dbo].[Users] ([Id], [Email], [Status], [WorkspaceId], [DisplayName], [PasswordHash], [PasswordSalt]) VALUES(2, N'liefu@test.com', 0, 1, N'Leif', CONVERT(VARBINARY(256), '0xE7CF33D78119573CA3B62086D2C4A1C1ED53C0A04FB9F62E653F249D30CE0D8A4CF5FE4A8967F1462D0ADFB36558AFFD6F9FCBC05E7CCB7BD1DD2E4CBF1634F2', 1) , CONVERT(VARBINARY(256), '0x461762CD0CCEDFEAD2765F00D4B3CB529C3124143E9191CE835312AC88DD5C7DDE85210C8322CA34E689EB5D85960167FCB5C0B4682650FADC257E005D54936F35908E79FEB14E69CEFF7EA002556F2E455513C4CA6BEE360C06C86F47A06BB9AB3C8E3933F9055EB85BA475C450E69565D3747EBB6D8B6382B65214D664F83E', 1) );" +
//                $"INSERT INTO[dbo].[Users] ([Id], [Email], [Status], [WorkspaceId], [DisplayName], [PasswordHash], [PasswordSalt]) VALUES(3, N'lina@test.com', 0, 1, N'lina', CONVERT(VARBINARY(256), '0xE7CF33D78119573CA3B62086D2C4A1C1ED53C0A04FB9F62E653F249D30CE0D8A4CF5FE4A8967F1462D0ADFB36558AFFD6F9FCBC05E7CCB7BD1DD2E4CBF1634F2', 1) , CONVERT(VARBINARY(256), '0x461762CD0CCEDFEAD2765F00D4B3CB529C3124143E9191CE835312AC88DD5C7DDE85210C8322CA34E689EB5D85960167FCB5C0B4682650FADC257E005D54936F35908E79FEB14E69CEFF7EA002556F2E455513C4CA6BEE360C06C86F47A06BB9AB3C8E3933F9055EB85BA475C450E69565D3747EBB6D8B6382B65214D664F83E', 1) );" +
//                $"SET IDENTITY_INSERT[dbo].[Users] OFF";
//            context.Database.ExecuteSqlCommand(addUsersScript);
//        }
//    }
//}