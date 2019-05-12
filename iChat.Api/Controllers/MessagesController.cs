using System;
using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Extensions;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase {

        private readonly iChatContext _context;
        private readonly INotificationService _notificationService;
        private IMessageParsingService _messageParsingService;

        public MessagesController(iChatContext context,
            INotificationService notificationService,
            IMessageParsingService messageParsingService) {
            _context = context;
            _notificationService = notificationService;
            _messageParsingService = messageParsingService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForChannelAsync(int id) {
            try {
                var messages = await _context.ChannelMessages
                        .Include(m => m.Sender)
                        .Where(m => m.ChannelId == id)
                        .OrderBy(m => m.CreatedDate)
                        .Cast<Message>()
                        .ToListAsync();
                return messages;
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/messages/user/1
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForUserAsync(int id) {
            try {
                var messages = await _context.DirectMessages
                        .Include(m => m.Sender)
                        .Where(m => m.ReceiverId == id &&
                                    m.SenderId == User.GetUserId() ||
                                    m.ReceiverId == User.GetUserId() &&
                                    m.SenderId == id)
                        .OrderBy(m => m.CreatedDate)
                        .Cast<Message>()
                        .ToListAsync();
                return messages;
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/messages/user/1
        [HttpPost("user/{id}")]
        public async Task<IActionResult> PostMessageToUserAsync(int id, [FromBody] string newMessage) {
            try {
                if (id < 1)
                    throw new ArgumentException("invalid user id");

                var message = new DirectMessage {
                    ReceiverId = id,
                    Content = _messageParsingService.Parse(newMessage),
                    CreatedDate = DateTime.Now,
                    SenderId = User.GetUserId()
                };

                _context.DirectMessages.Add(message);

                await _context.SaveChangesAsync();

                _notificationService.SendDirectMessageNotification(User.GetUserId());
                _notificationService.SendDirectMessageNotification(id);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, [FromBody] string newMessage) {
            try {
                if (id < 1)
                    throw new ArgumentException("invalid channel id");

                var message = new ChannelMessage {
                    ChannelId = id,
                    Content = _messageParsingService.Parse(newMessage),
                    CreatedDate = DateTime.Now,
                    SenderId = User.GetUserId()
                };

                _context.ChannelMessages.Add(message);
                await _context.SaveChangesAsync();

                _notificationService.SendUpdateChannelNotification(id);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
