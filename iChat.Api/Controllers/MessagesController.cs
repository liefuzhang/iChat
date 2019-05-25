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

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private IMessageService _messageService;
        private IChannelService _channelService;

        public MessagesController(iChatContext context,
            INotificationService notificationService,
            IMessageService messageService, IChannelService channelService)
        {
            _notificationService = notificationService;
            _messageService = messageService;
            _channelService = channelService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForChannelAsync(int id)
        {
            try
            {
                if (id == iChatConstants.DefaultChannelIdInRequest) {
                    id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkplaceId());
                }
                
                var messageGroups = await _messageService.GetMessagesForChannelAsync(id, User.GetWorkplaceId());
                return messageGroups.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/messages/user/1
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForUserAsync(int id)
        {
            try
            {
                var messages = await _messageService.GetMessagesForUserAsync(id, User.GetUserId(), User.GetWorkplaceId());
                return messages.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/messages/user/1
        [HttpPost("user/{id}")]
        public async Task<IActionResult> PostMessageToUserAsync(int id, [FromBody] string newMessage)
        {
            try
            {
                await _messageService.PostMessageToUserAsync(newMessage, id, User.GetUserId(), User.GetWorkplaceId());

                _notificationService.SendDirectMessageNotificationAsync(User.GetUserId());
                _notificationService.SendDirectMessageNotificationAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, [FromBody] string newMessage)
        {
            try
            {
                if (id == iChatConstants.DefaultChannelIdInRequest) {
                    id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkplaceId());
                }

                await _messageService.PostMessageToChannelAsync(newMessage, id, User.GetUserId(), User.GetWorkplaceId());

                var userIds = await _channelService.GetAllChannelUserIdsAsync(id);
                _notificationService.SendUpdateChannelNotificationAsync(userIds, id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
