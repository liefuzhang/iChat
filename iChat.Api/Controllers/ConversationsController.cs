﻿using iChat.Api.Extensions;
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
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        // GET api/conversations/recent
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> GetRecentConversationsAsync()
        {
            var conversations = await _conversationService
                .GetRecentConversationsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return conversations.ToList();
        }

        // GET api/conversations/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationDto>> GetAsync(int id)
        {
            var conversation = await _conversationService.GetConversationByIdAsync(id, User.GetUserId(), User.GetWorkspaceId());
            if (conversation == null)
            {
                return NotFound();
            }

            return conversation;
        }

        // POST api/conversations
        [HttpPost("start")]
        public async Task<ActionResult<int>> StartConversationAsync(List<int> withUserIds)
        {
            var id = await _conversationService.StartConversationWithOthersAsync(withUserIds, User.GetUserId(), User.GetWorkspaceId());

            return Ok(id);
        }


        // POST api/conversations/notifyTyping
        [HttpPost("notifyTyping")]
        public async Task<IActionResult> NotifyTypingAsync([FromBody]int conversationId)
        {
            await _conversationService.NotifyTypingAsync(conversationId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }
    }
}
