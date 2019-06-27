using iChat.Api.Dtos;
using iChat.Api.Extensions;
using iChat.Api.Helpers;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public MessagesController(INotificationService notificationService,
            IMessageService messageService,
            IChannelService channelService,
            IConversationService conversationService,
            ICacheService cacheService, IFileHelper fileHelper) {
            _notificationService = notificationService;
            _messageService = messageService;
            _channelService = channelService;
            _conversationService = conversationService;
            _cacheService = cacheService;
        }

        // GET api/messages/channel/1
        [HttpGet("channel/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForChannelAsync(int id) {
            var messageGroups = await _messageService.GetMessagesForChannelAsync(id, User.GetUserId(), User.GetWorkspaceId());

            await _cacheService.RemoveUnreadChannelForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            await _cacheService.SetActiveSidebarItemAsync(true, id, User.GetUserId(), User.GetWorkspaceId());

            _notificationService.SendUpdateChannelListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // GET api/messages/conversation/1
        [HttpGet("conversation/{id}")]
        public async Task<ActionResult<IEnumerable<MessageGroupDto>>> GetMessagesForConversationAsync(int id) {
            var messageGroups = await _messageService.GetMessagesForConversationAsync(id, User.GetUserId(), User.GetWorkspaceId());

            if (!await _conversationService.IsSelfConversationAsync(id, User.GetUserId())) {
                await _cacheService.ClearUnreadMessageForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            }
            await _cacheService.SetActiveSidebarItemAsync(false, id, User.GetUserId(), User.GetWorkspaceId());

            _notificationService.SendUpdateConversationListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageGroups.ToList();
        }

        // POST api/messages/conversation/1
        [HttpPost("conversation/{id}")]
        public async Task<IActionResult> PostMessageToConversationAsync(int id, MessagePostDto messagePostDto) {
            await _messageService.PostMessageToConversationAsync(messagePostDto.MessageContent, id, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(id)).ToList();
            if (!await _conversationService.IsSelfConversationAsync(id, User.GetUserId())) {
                await _cacheService.AddNewUnreadMessageForUsersAsync(id, userIds, User.GetWorkspaceId());
            }
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, id);

            return Ok();
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, MessagePostDto messagePostDto) {
            await _messageService.PostMessageToChannelAsync(messagePostDto.MessageContent, id, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(id)).ToList();
            await _cacheService.AddUnreadChannelForUsersAsync(id, userIds, User.GetWorkspaceId(), messagePostDto.MentionUserIds);
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, id);

            return Ok();
        }

        // POST api/messages/conversation/1/update/1
        [HttpPost("conversation/{conversationId}/update/{messageId}")]
        public async Task<IActionResult> UpdateMessageInConversationAsync(int conversationId, int messageId, MessagePostDto messagePostDto) {
            await _messageService.UpdateMessageInConversationAsync(messagePostDto.MessageContent, conversationId, messageId, User.GetUserId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/update/1
        [HttpPost("channel/{channelId}/update/{messageId}")]
        public async Task<IActionResult> UpdateMessageInChannelAsync(int channelId, int messageId, MessagePostDto messagePostDto) {
            await _messageService.UpdateMessageInChannelAsync(messagePostDto.MessageContent, channelId, messageId, User.GetUserId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }

        // POST api/messages/conversation/1/uploadFile
        [HttpPost("conversation/{conversationId}/uploadFile")]
        public async Task<IActionResult> UploadFilesToConversationAsync(IList<IFormFile> files, int conversationId) {
            await _messageService.PostFileMessageToConversationAsync(files, conversationId, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/uploadFile
        [HttpPost("channel/{channelId}/uploadFile")]
        public async Task<IActionResult> UploadFilesToChannelAsync(IList<IFormFile> files, int channelId) {
            await _messageService.PostFileMessageToChannelAsync(files, channelId, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }

        // GET api/messages/downloadFile/1
        [HttpGet("downloadFile/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync(int fileId) {
            var fileTuple = await _messageService.DownloadFileAsync(fileId, User.GetUserId(), User.GetWorkspaceId());
            return File(fileTuple.stream, fileTuple.contentType);
        }

        // POST api/messages/conversation/1/shareFile/1
        [HttpPost("conversation/{conversationId}/shareFile/{fileId}")]
        public async Task<IActionResult> ShareFileToConversationAsync(int conversationId, int fileId)
        {
            await _messageService.ShareFileToConversationAsync(conversationId, fileId, User.GetUserId(), User.GetWorkspaceId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/shareFile/1
        [HttpPost("channel/{channelId}/shareFile/{fileId}")]
        public async Task<IActionResult> ShareFileToChannelAsync(int channelId, int fileId)
        {
            await _messageService.ShareFileToChannelAsync(channelId, fileId, User.GetUserId(), User.GetWorkspaceId());


            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }

        // POST api/messages/conversation/1/deleteMessage
        [HttpPost("conversation/{conversationId}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromConversationAsync(int conversationId, [FromBody]int messageId) {
            await _messageService.DeleteMessageAsync(messageId, User.GetUserId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/deleteMessage
        [HttpPost("channel/{channelId}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromChannelAsync(int channelId, [FromBody]int messageId) {
            await _messageService.DeleteMessageAsync(messageId, User.GetUserId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }

        // POST api/messages/conversation/1/addReaction/1
        [HttpPost("conversation/{conversationId}/addReaction/{messageId}")]
        public async Task<IActionResult> AddReactionToMessageInConversationAsync(int conversationId, int messageId, [FromBody]string emojiColons) {
            await _messageService.AddReactionToMessageAsync(messageId, emojiColons, User.GetUserId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/addReaction/1
        [HttpPost("channel/{channelId}/addReaction/{messageId}")]
        public async Task<IActionResult> AddReactionToMessageInChannelAsync(int channelId, int messageId, [FromBody]string emojiColons) {
            await _messageService.AddReactionToMessageAsync(messageId, emojiColons, User.GetUserId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }

        // POST api/messages/conversation/1/removeReaction/1
        [HttpPost("conversation/{conversationId}/removeReaction/{messageId}")]
        public async Task<IActionResult> RemoveReactionToMessageInConversationAsync(int conversationId, int messageId, [FromBody]string emojiColons) {
            await _messageService.RemoveReactionToMessageAsync(messageId, emojiColons, User.GetUserId());

            var userIds = (await _conversationService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            _notificationService.SendNewConversationMessageNotificationAsync(userIds, conversationId);

            return Ok();
        }

        // POST api/messages/channel/1/removeReaction/1
        [HttpPost("channel/{channelId}/removeReaction/{messageId}")]
        public async Task<IActionResult> RemoveReactionToMessageInChannelAsync(int channelId, int messageId, [FromBody]string emojiColons) {
            await _messageService.RemoveReactionToMessageAsync(messageId, emojiColons, User.GetUserId());

            var userIds = (await _channelService.GetAllChannelUserIdsAsync(channelId)).ToList();
            _notificationService.SendNewChannelMessageNotificationAsync(userIds, channelId);

            return Ok();
        }
    }
}
