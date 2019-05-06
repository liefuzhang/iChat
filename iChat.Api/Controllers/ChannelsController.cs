using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {

        private readonly iChatContext _context;
        private readonly INotificationService _notificationService;
        private IMessageParsingService _messageParsingService;

        public ChannelsController(iChatContext context,
            INotificationService notificationService,
            IMessageParsingService messageParsingService)
        {
            _context = context;
            _notificationService = notificationService;
            _messageParsingService = messageParsingService;
        }

        // GET api/channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetAsync()
        {
            var channels = await _context.Channels.AsNoTracking().ToListAsync();
            return channels;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
