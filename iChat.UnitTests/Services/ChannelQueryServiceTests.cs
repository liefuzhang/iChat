using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Services;
using iChat.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.UnitTests.Services
{
    [TestClass]
    public class ChannelQueryServiceTests
    {
        private iChatContext _context;
        private ChannelQueryService _channelQueryService;

        [TestInitialize]
        public void Initilize()
        {
            _context = new DbContextFactory().CreateNewContext();
            var userQueryService = new Mock<IUserQueryService>();
            var cacheService = new Mock<ICacheService>();
            var mapper = new Mock<IMapper>();
            var notificationService = new Mock<INotificationService>();
            _channelQueryService = new ChannelQueryService(_context, userQueryService.Object,
                cacheService.Object, mapper.Object, notificationService.Object);
        }

        [TestMethod]
        public async Task GetChannelsAsync_WhenCalled_ReturnChannels()
        {
            _context.SeedTestData();

            var result = await _channelQueryService.GetChannelsForUserAsync(SeedData.TestUser1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task GetChannelByIdAsync_WhenCalled_ReturnChannel()
        {
            _context.SeedTestData();

            var result = await _channelQueryService.GetChannelByIdAsync(SeedData.TestChannel1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(SeedData.TestChannel1Name, result.Name);
        }
    }
}
