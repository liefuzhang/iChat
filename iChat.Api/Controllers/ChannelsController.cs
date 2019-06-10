using iChat.Api.Extensions;
using iChat.Api.Models;
using iChat.Api.Services;
using iChat.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Dtos;
using iChat.Api.Constants;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase {
        private IChannelService _channelService;
        private ICacheService _cacheService;

        public ChannelsController(IChannelService channelService,
            ICacheService cacheService) {
            _channelService = channelService;
            _cacheService = cacheService;
        }

        // GET api/channels/forUser
        [HttpGet("forUser")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetChannelsForUserAsync() {
            var channels = await _channelService
                .GetChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/allUnsubscribed
        [HttpGet("allUnsubscribed")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetAllUnsubscribedChannelsAsync() {
            var channels = await _channelService
                .GetAllUnsubscribedChannelsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return channels.ToList();
        }

        // GET api/channels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelDto>> GetAsync(int id) {
            if (id == iChatConstants.DefaultChannelIdInRequest) {
                id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkspaceId());
            }

            var channel = await _channelService.GetChannelByIdAsync(id, User.GetWorkspaceId());
            if (channel == null) {
                return NotFound();
            }

            return channel;
        }

        // POST api/channels
        [HttpPost]
        public async Task<ActionResult<int>> CreateChannelAsync(ChannelCreateDto channelCreateDto) {
            var id = await _channelService.CreateChannelAsync(channelCreateDto.Name, User.GetWorkspaceId(), channelCreateDto.Topic);
            await _channelService.AddUserToChannelAsync(id, User.GetUserId(), User.GetWorkspaceId());

            return Ok(id);
        }

        // POST api/channels/join
        [HttpPost("join")]
        public async Task<IActionResult> JoinChannelAsync([FromBody] int channelId) {
            await _channelService.AddUserToChannelAsync(channelId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }
    }
}
