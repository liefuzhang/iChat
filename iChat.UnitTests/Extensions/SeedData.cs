using iChat.Api.Data;
using iChat.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace iChat.UnitTests.Extensions {
    public static class SeedData {
        public static string TestUser1Email { get; } = "email1@test.com";
        public static string TestUser1Name { get; } = "user1";
        public static int TestUser1Id { get; private set; }
        public static string TestUser2Email { get; } = "email2@test.com";
        public static string TestUser2Name { get; } = "user2";
        public static int TestUser2Id { get; private set; }

        public static int TestWorkspaceId { get; } = 1;

        public static int TestChannel1Id { get; private set; }
        public static string TestChannel1Name { get; } = "channel1";
        public static int TestChannel2Id { get; private set; }
        public static string TestChannel2Name { get; } = "channel2";

        public static void SeedTestData(this iChatContext context) {
            var user1 = context.Users.Add(new User(TestUser1Email, "testpwd", TestWorkspaceId, TestUser1Name, Guid.NewGuid()));
            var user2 = context.Users.Add(new User(TestUser2Email, "testpwd", TestWorkspaceId, TestUser2Name, Guid.NewGuid()));
            context.SaveChanges();
            TestUser1Id = user1.Entity.Id;
            TestUser2Id = user2.Entity.Id;


            var channel1 = context.Channels.Add(new Channel(TestChannel1Name, TestUser1Id, TestWorkspaceId, "topic1"));
            var channel2 = context.Channels.Add(new Channel(TestChannel2Name, TestUser2Id, TestWorkspaceId, "topic2"));
            context.SaveChanges();
            TestChannel1Id = channel1.Entity.Id;
            TestChannel2Id = channel2.Entity.Id;

            context.ChannelSubscriptions.Add(new ChannelSubscription(TestChannel1Id, TestUser1Id));

            context.SaveChanges();
        }
    }
}
