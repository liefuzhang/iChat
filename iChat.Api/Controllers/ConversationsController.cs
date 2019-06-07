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
    public class ConversationsController : ControllerBase {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService) {
            _conversationService = conversationService;
        }

        // GET api/conversations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetAsync() {
            var conversations = await _conversationService
                .GetConversationsForUserAsync(User.GetUserId(), User.GetWorkplaceId());
            return conversations.ToList();
        }

        // GET api/conversations/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> GetAsync(int id) {
            var conversation = await _conversationService.GetConversationByIdAsync(id, User.GetWorkplaceId());
            if (conversation == null)
            {
                return NotFound();
            }

            return conversation;
        }

        // POST api/conversation
        [HttpPost("start")]
        public async Task<ActionResult<int>> StartConversationAsync(List<int> withUserIds)
        {
            var userIds = withUserIds;
            userIds.Add(User.GetUserId());
            var id = await _conversationService.StartConversationAsync(userIds, User.GetWorkplaceId());

            return Ok(id);
        }        
    }
}
