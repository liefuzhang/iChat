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
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationCommandService _conversationCommandService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly INotificationService _notificationService;

        public ConversationsController(IConversationCommandService conversationCommandService,
            IConversationQueryService conversationQueryService, INotificationService notificationService)
        {
            _conversationCommandService = conversationCommandService;
            _conversationQueryService = conversationQueryService;
            _notificationService = notificationService;
        }

        // GET api/conversations/recent
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> GetRecentConversationsAsync()
        {
            var conversations = await _conversationQueryService
                .GetRecentConversationsForUserAsync(User.GetUserId(), User.GetWorkspaceId());
            return conversations.ToList();
        }

        // GET api/conversations/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationDto>> GetAsync(int id)
        {
            var conversation = await _conversationQueryService.GetConversationByIdAsync(id, User.GetUserId(), User.GetWorkspaceId());
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
            var id = await _conversationCommandService.StartConversationWithOthersAsync(withUserIds, User.GetUserId(), User.GetWorkspaceId());

            return Ok(id);
        }


        // POST api/conversations/notifyTyping
        [HttpPost("notifyTyping")]
        public async Task<IActionResult> NotifyTypingAsync([FromBody]int conversationId)
        {
            await _conversationCommandService.NotifyTypingAsync(conversationId, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // GET api/conversations/1/userIds
        [HttpGet("{id}/userIds")]
        public async Task<ActionResult<IEnumerable<int>>> GetAllConversationUserIdsAsync(int id)
        {
            var userIds = await _conversationQueryService.GetAllConversationUserIdsAsync(id);

            return userIds.ToList();
        }

        // GET api/conversations/1/users
        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllConversationUsersAsync(int id)
        {
            var users = await _conversationQueryService.GetAllConversationUsersAsync(id);

            return users.ToList();
        }

        // Post api/conversations/1/inviteOtherMembers
        [HttpPost("{id}/inviteOtherMembers")]
        public async Task<IActionResult> InviteOtherMembersToConversationAsync(int id, List<int> userIds)
        {
            int conversationId = await _conversationCommandService.InviteOtherMembersToConversationAsync(id, userIds, User.GetUserId(), User.GetWorkspaceId());

            var allConversationUserIds = await _conversationQueryService.GetAllConversationUserIdsAsync(id);
            await _notificationService.SendUpdateConversationDetailsNotificationAsync(allConversationUserIds, id);

            return Ok(conversationId);
        }
    }
}
