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
        private readonly IChannelService _channelService;
        private readonly INotificationService _notificationService;

        public ChannelsController(IChannelService channelService, INotificationService notificationService)
        {
            _channelService = channelService;
            _notificationService = notificationService;
        }

        // GET api/channels/forUser
        [HttpGet("forUser")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetChannelsForUserAsync()
        {
            var channels = await _channelService
                .GetChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/allUnsubscribed
        [HttpGet("allUnsubscribed")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetAllUnsubscribedChannelsAsync()
        {
            var channels = await _channelService
                .GetAllUnsubscribedChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelDto>> GetAsync(int id)
        {
            var channel = await _channelService.GetChannelByIdAsync(id, User.GetUserId(), User.GetWorkspaceId());
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
            var id = await _channelService.CreateChannelAsync(channelCreateDto.Name, User.GetWorkspaceId(), channelCreateDto.Topic);
            await _channelService.AddUserToChannelAsync(id, User.GetUserId(), User.GetWorkspaceId());

            return Ok(id);
        }

        // POST api/channels/join
        [HttpPost("join")]
        public async Task<IActionResult> JoinChannelAsync([FromBody] int channelId)
        {
            await _channelService.AddUserToChannelAsync(channelId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/channels/notifyTyping
        [HttpPost("notifyTyping")]
        public async Task<IActionResult> NotifyTypingAsync([FromBody]int channelId)
        {
            await _channelService.NotifyTypingAsync(channelId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/channels/leave
        [HttpPost("leave")]
        public async Task<IActionResult> LeaveChannelAsync([FromBody] int channelId)
        {
            await _channelService.RemoveUserFromChannelAsync(channelId, User.GetUserId());

            return Ok();
        }

        // GET api/channels/1/userIds
        [HttpGet("{id}/userIds")]
        public async Task<ActionResult<IEnumerable<int>>> GetAllChannelUserIdsAsync(int id)
        {
            var userIds = await _channelService.GetAllChannelUserIdsAsync(id);

            return userIds.ToList();
        }

        // Post api/channels/1/inviteOtherMembers
        [HttpPost("{id}/inviteOtherMembers")]
        public async Task<IActionResult> InviteOtherMembersToChannelAsync(int id, List<int> userIds)
        {
            await _channelService.InviteOtherMembersToChannelAsync(id, userIds, User.GetUserId(), User.GetWorkspaceId());

            var allChannelUserIds = await _channelService.GetAllChannelUserIdsAsync(id);
            _notificationService.SendUpdateChannelDetailsNotificationAsync(allChannelUserIds, id);

            return Ok();
        }
    }
}
