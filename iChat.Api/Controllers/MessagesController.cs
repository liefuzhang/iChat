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
    public class MessagesController : ControllerBase {
        private readonly INotificationService _notificationService;
        private readonly IMessageService _messageService;
        private readonly IChannelService _channelService;
        private readonly IConversationService _conversationService;
        private readonly ICacheService _cacheService;

        public MessagesController(iChatContext context,
            INotificationService notificationService,
            IMessageService messageService,
            IChannelService channelService,
            IConversationService conversationService,
            ICacheService cacheService) {
            _notificationService = notificationService;
            _messageService = messageService;
            _channelService = channelService;
            _conversationService = conversationService;
            _cacheService = cacheService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForChannelAsync(int id) {
            if (id == iChatConstants.DefaultChannelIdInRequest) {
                id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkplaceId());
            }

            var messageGroups = await _messageService.GetMessagesForChannelAsync(id, User.GetWorkplaceId());
            await _cacheService.RemoveUnreadChannelIdForUserAsync(id, User.GetUserId(), User.GetWorkplaceId());
            _notificationService.SendUpdateChannelListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // GET api/messages/conversation/1
        [HttpGet("conversation/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForConversationAsync(int id) {
            var messageGroups = await _messageService.GetMessagesForConversationAsync(id, User.GetWorkplaceId());
            await _cacheService.ClearUnreadMessageForUserAsync(id, User.GetUserId(), User.GetWorkplaceId());
            _notificationService.SendUpdateConversationListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // POST api/messages/conversation/1
        [HttpPost("conversation/{id}")]
        public async Task<IActionResult> PostMessageToConversationAsync(int id, [FromBody] string newMessage) {
            await _messageService.PostMessageToConversationAsync(newMessage, id, User.GetUserId(), User.GetWorkplaceId());

            var userIds = await _conversationService.GetAllConversationUserIdsAsync(id);
            await _cacheService.AddNewUnreadMessageForUsersAsync(id, userIds, User.GetWorkplaceId());
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, id);

            return Ok();
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, [FromBody] string newMessage) {
            if (id == iChatConstants.DefaultChannelIdInRequest) {
                id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkplaceId());
            }

            await _messageService.PostMessageToChannelAsync(newMessage, id, User.GetUserId(), User.GetWorkplaceId());

            var userIds = await _channelService.GetAllChannelUserIdsAsync(id);
            await _cacheService.AddUnreadChannelIdForUsersAsync(id, userIds, User.GetWorkplaceId());
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, id);

            return Ok();
        }
    }
}
