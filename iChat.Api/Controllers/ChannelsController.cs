using iChat.Api.Dtos;
using iChat.Api.Extensions;
using iChat.Api.Models;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelCommandService _channelCommandService;
        private readonly IChannelQueryService _channelQueryService;
        private readonly INotificationService _notificationService;

        public ChannelsController(IChannelCommandService channelCommandService,
            IChannelQueryService channelQueryService, INotificationService notificationService)
        {
            _channelCommandService = channelCommandService;
            _channelQueryService = channelQueryService;
            _notificationService = notificationService;
        }

        // GET api/channels/forUser
        [HttpGet("forUser")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetChannelsForUserAsync()
        {
            var channels = await _channelQueryService
                .GetChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/allUnsubscribed
        [HttpGet("allUnsubscribed")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetAllUnsubscribedChannelsAsync()
        {
            var channels = await _channelQueryService
                .GetAllUnsubscribedChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelDto>> GetAsync(int id)
        {
            var channel = await _channelQueryService.GetChannelByIdAsync(id, User.GetWorkspaceId());
            if (channel == null)
            {
                return NotFound();
            }

            return channel;
        }

        // POST api/channels
        [HttpPost]
        public async Task<ActionResult<int>> CreateChannelAsync(ChannelCreateDto channelCreateDto)
        {
            var id = await _channelCommandService.CreateChannelAsync(channelCreateDto.Name, User.GetUserId(), User.GetWorkspaceId(), channelCreateDto.Topic);
            await _channelCommandService.AddUserToChannelAsync(id, User.GetUserId(), User.GetWorkspaceId());

            return Ok(id);
        }

        // POST api/channels/join
        [HttpPost("join")]
        public async Task<IActionResult> JoinChannelAsync([FromBody] int channelId)
        {
            await _channelCommandService.AddUserToChannelAsync(channelId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/channels/notifyTyping
        [HttpPost("notifyTyping")]
        public async Task<IActionResult> NotifyTypingAsync([FromBody]int channelId)
        {
            await _channelCommandService.NotifyTypingAsync(channelId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/channels/leave
        [HttpPost("leave")]
        public async Task<IActionResult> LeaveChannelAsync([FromBody] int channelId)
        {
            await _channelCommandService.RemoveUserFromChannelAsync(channelId, User.GetUserId());

            return Ok();
        }

        // GET api/channels/1/userIds
        [HttpGet("{id}/userIds")]
        public async Task<ActionResult<IEnumerable<int>>> GetAllChannelUserIdsAsync(int id)
        {
            var userIds = await _channelQueryService.GetAllChannelUserIdsAsync(id);

            return userIds.ToList();
        }

        // Post api/channels/1/inviteOtherMembers
        [HttpPost("{id}/inviteOtherMembers")]
        public async Task<IActionResult> InviteOtherMembersToChannelAsync(int id, List<int> userIds)
        {
            await _channelCommandService.InviteOtherMembersToChannelAsync(id, userIds, User.GetUserId());

            var allChannelUserIds = await _channelQueryService.GetAllChannelUserIdsAsync(id);
            _notificationService.SendUpdateChannelDetailsNotificationAsync(allChannelUserIds, id);

            return Ok();
        }
    }
}
