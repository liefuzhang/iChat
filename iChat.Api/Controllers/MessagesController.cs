﻿using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Extensions;
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
    public class MessagesController : ControllerBase
    {
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
            ICacheService cacheService)
        {
            _notificationService = notificationService;
            _messageService = messageService;
            _channelService = channelService;
            _conversationService = conversationService;
            _cacheService = cacheService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForChannelAsync(int id)
        {
            if (id == iChatConstants.DefaultChannelIdInRequest)
            {
                id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkspaceId());
            }

            var messageGroups = await _messageService.GetMessagesForChannelAsync(id, User.GetWorkspaceId());
            await _cacheService.RemoveUnreadChannelForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            _notificationService.SendUpdateChannelListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // GET api/messages/conversation/1
        [HttpGet("conversation/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForConversationAsync(int id)
        {
            var messageGroups = await _messageService.GetMessagesForConversationAsync(id, User.GetWorkspaceId());
            if (!await _conversationService.IsSelfConversationAsync(id, User.GetUserId()))
            {
                await _cacheService.ClearUnreadMessageForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            }
            _notificationService.SendUpdateConversationListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // POST api/messages/conversation/1
        [HttpPost("conversation/{id}")]
        public async Task<IActionResult> PostMessageToConversationAsync(int id, MessagePostDto messagePostDto)
        {
            await _messageService.PostMessageToConversationAsync(messagePostDto.Message, id, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(id)).ToList();
            if (!await _conversationService.IsSelfConversationAsync(id, User.GetUserId()))
            {
                await _cacheService.AddNewUnreadMessageForUsersAsync(id, userIds, User.GetWorkspaceId());
            }
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, id);

            return Ok();
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, MessagePostDto messagePostDto)
        {
            if (id == iChatConstants.DefaultChannelIdInRequest)
            {
                id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkspaceId());
            }

            await _messageService.PostMessageToChannelAsync(messagePostDto.Message, id, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(id)).ToList();
            await _cacheService.AddUnreadChannelForUsersAsync(id, userIds, User.GetWorkspaceId(), messagePostDto.MentionUserIds);
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, id);

            return Ok();
        }
    }
}
