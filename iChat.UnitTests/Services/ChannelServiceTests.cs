using iChat.Api.Data;
using iChat.Api.Models;
using iChat.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.UnitTests.Services
{
    [TestClass]
    public class ChannelServiceTests
    {
        [TestMethod]
        public async Task GetChannelsAsync_WhenCalled_ReturnChannels()
        {
            var options = new DbContextOptionsBuilder<iChatContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var userService = new Mock<IUserService>();

            using (var context = new iChatContext(options))
            {
                context.Users.Add(new User("email1@test.com", "testpwd", 1, "user1", new Guid()));
                context.Users.Add(new User("email2@test.com", "testpwd", 1, "user2", new Guid()));
                var channel1 = new Channel("channel1", 1, "topic1");
                var channel2 = new Channel("channel1", 1, "topic1");
                context.Channels.Add(new Channel("channel1", 1, "topic1"));
                context.Channels.Add(new Channel("channel2", 1, "topic2"));
                context.ChannelSubscriptions.Add(new ChannelSubscription(1, 1));
                await context.SaveChangesAsync();

                var service = new ChannelService(context, userService.Object);
                var result = await service.GetChannelsAsync(1, 1);
                Assert.AreEqual(1, result.Count());
            }
        }
    }
}
