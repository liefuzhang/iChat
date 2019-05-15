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

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private IMessageService _messageService;

        public MessagesController(iChatContext context,
            INotificationService notificationService,
            IMessageService messageService)
        {
            _notificationService = notificationService;
            _messageService = messageService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForChannelAsync(int id)
        {
            try
            {
                var messages = await _messageService.GetMessagesForChannelAsync(id, User.GetWorkplaceId());
                return messages.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/messages/user/1
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForUserAsync(int id)
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

                _notificationService.SendDirectMessageNotification(User.GetUserId());
                _notificationService.SendDirectMessageNotification(id);

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
                await _messageService.PostMessageToChannelAsync(newMessage, id, User.GetUserId(), User.GetWorkplaceId());

                _notificationService.SendUpdateChannelNotification(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
