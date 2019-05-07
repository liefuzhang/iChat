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

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly iChatContext _context;
        private readonly INotificationService _notificationService;
        private IMessageParsingService _messageParsingService;

        public MessagesController(iChatContext context,
            INotificationService notificationService,
            IMessageParsingService messageParsingService)
        {
            _context = context;
            _notificationService = notificationService;
            _messageParsingService = messageParsingService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForChannelAsync(int id)
        {
            var messages = await _context.ChannelMessages
                .Include(m => m.Sender)
                .Where(m => m.ChannelId == id)
                .OrderBy(m => m.CreatedDate)
                .Cast<Message>()
                .ToListAsync();
            return messages;
        }

        // GET api/messages/user/1
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForUserAsync(int id)
        {
            var messages = await _context.DirectMessages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == id &&
                            m.SenderId == 1 || // todo change here
                            m.ReceiverId == id &&
                            m.SenderId == 1)
                .OrderBy(m => m.CreatedDate)
                .Cast<Message>()
                .ToListAsync();
            return messages;
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task PostMessageToChannelAsync(int id, [FromBody] string newMessage)
        {
            if (id < 1)
                throw new ArgumentException("invalid channel id");

            var message = new ChannelMessage
            {
                ChannelId = id,
                Content = _messageParsingService.Parse(newMessage),
                CreatedDate = DateTime.Now,
                SenderId = 1
            };
            _context.ChannelMessages.Add(message);

            await _context.SaveChangesAsync();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
