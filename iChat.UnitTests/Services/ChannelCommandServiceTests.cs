using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Services;
using iChat.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iChat.UnitTests.Services {
    [TestClass]
    public class ChannelCommandServiceTests {
        private iChatContext _context;
        private ChannelCommandService _channelCommandService;

        [TestInitialize]
        public void Initilize() {
            _context = new DbContextFactory().CreateNewContext();
            var userQueryService = new Mock<IUserQueryService>();
            var channelQueryService = new Mock<IChannelQueryService>();
            var mapper = new Mock<IMapper>();
            var notificationService = new Mock<INotificationService>();
            var messageCommandService = new Mock<IMessageCommandService>();
            _channelCommandService = new ChannelCommandService(_context, userQueryService.Object,
                channelQueryService.Object, notificationService.Object, messageCommandService.Object);
        }

        [TestMethod]
        public async Task CreateChannelAsync_WhenCalled_ReturnNewChannelId() {
            const string NewChannel = "channel new";
            const string NewTopic = "topic new";
            var result = await _channelCommandService.CreateChannelAsync(NewChannel, SeedData.TestUser1Id, SeedData.TestWorkspaceId, NewTopic);

            var channel = _context.Channels.Single();
            Assert.AreEqual(result, channel.Id);
            Assert.AreEqual(NewChannel, channel.Name);
            Assert.AreEqual(NewTopic, channel.Topic);
            Assert.AreEqual(SeedData.TestWorkspaceId, channel.WorkspaceId);
        }

        [TestMethod]
        public async Task CreateChannelAsync_WhenChannelNameExists_ThrowArgumentException() {
            _context.SeedTestData();

            string NewChannel = SeedData.TestChannel1Name;
            const string NewTopic = "topic new";

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _channelCommandService.CreateChannelAsync(NewChannel, SeedData.TestUser1Id, SeedData.TestWorkspaceId, NewTopic));
        }
    }
}
