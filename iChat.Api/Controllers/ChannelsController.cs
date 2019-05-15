using iChat.Api.Extensions;
using iChat.Api.Models;
using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase {
        private IChannelService _channelService;

        public ChannelsController(iChatContext context,
            INotificationService notificationService,
            IMessageParsingService messageParsingService,
            IChannelService channelService) {
            _channelService = channelService;
        }

        // GET api/channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetAsync() {
            try {
                var channels = await _channelService.GetChannelsAsync(User.GetUserId(), User.GetWorkplaceId());
                return channels.ToList();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET api/channels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Channel>> GetAsync(int id) {
            try {
                var channel = await _channelService.GetChannelByIdAsync(id, User.GetUserId());
                if (channel == null) {
                    return NotFound();
                }

                return channel;
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
