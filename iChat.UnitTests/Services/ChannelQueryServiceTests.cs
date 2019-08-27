using AutoMapper;
using iChat.Api.Contract;
using iChat.Api.Data;
using iChat.Api.Helpers;
using iChat.Api.Services;
using iChat.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.UnitTests.Services
{
    [TestClass]
    public class ChannelQueryServiceTests
    {
        private iChatContext _context;
        private ChannelQueryService _channelQueryService;
        private Mock<ICacheService> _cacheService;

        [TestInitialize]
        public void Initilize()
        {
            _context = new DbContextFactory().CreateNewContext();
            var userQueryService = new Mock<IUserQueryService>();
            _cacheService = new Mock<ICacheService>();
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            var notificationService = new Mock<INotificationService>();
            _channelQueryService = new ChannelQueryService(_context, userQueryService.Object,
                _cacheService.Object, mapper, notificationService.Object);
        }

        [TestMethod]
        public async Task GetChannelsAsync_WhenCalled_ReturnChannels()
        {
            _context.SeedTestData();
            _cacheService.Setup(c=>c.GetUnreadChannelsForUserAsync(SeedData.TestUser1Id, SeedData.TestWorkspaceId))
                .Returns(Task.FromResult(new List<ChannelUnreadItem>()));

            var result = await _channelQueryService.GetChannelsForUserAsync(SeedData.TestUser1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task GetChannelByIdAsync_WhenCalled_ReturnChannel()
        {
            _context.SeedTestData();

            var result = await _channelQueryService.GetChannelByIdAsync(SeedData.TestChannel1Id, SeedData.TestWorkspaceId);

            Assert.AreEqual(SeedData.TestChannel1Id, result.Id);
        }
    }
}
