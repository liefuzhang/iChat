using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Models;
using iChat.Api.Services;
using iChat.UnitTests.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.UnitTests.Services {
    [TestClass]
    public class ChannelServiceTests {
        private iChatContext _context;
        private ChannelService _channelService;

        [TestInitialize]
        public void Initilize() {
            _context = new DbContextFactory().CreateNewContext();
            var userService = new Mock<IUserService>();
            var cacheService = new Mock<ICacheService>();
            var mapper = new Mock<IMapper>();
            var notificationService = new Mock<INotificationService>();
            _channelService = new ChannelService(_context, userService.Object,
                cacheService.Object, mapper.Object, notificationService.Object);
        }

        [TestMethod]
        public async Task GetChannelsAsync_WhenCalled_ReturnChannels() {
            _context.SeedTestData();

            var result = await _channelService.GetChannelsForUserAsync(SeedData.TestUser1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task GetChannelByIdAsync_WhenCalled_ReturnChannel() {
            _context.SeedTestData();

            var result = await _channelService.GetChannelByIdAsync(SeedData.TestChannel1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(SeedData.TestChannel1Name, result.Name);
        }

        [TestMethod]
        public async Task CreateChannelAsync_WhenCalled_ReturnNewChannelId() {
            const string NewChannel = "channel new";
            const string NewTopic = "topic new";
            var result = await _channelService.CreateChannelAsync(NewChannel, SeedData.TestUser1Id, SeedData.TestWorkspaceId, NewTopic);

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

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _channelService.CreateChannelAsync(NewChannel, SeedData.TestUser1Id, SeedData.TestWorkspaceId, NewTopic));
        }
    }
}
