using iChat.Api.Constants;
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

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMessageQueryService _messageQueryService;
        private readonly IMessageCommandService _messageCommandService;
        private readonly IChannelQueryService _channelQueryService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly ICacheService _cacheService;

        public MessagesController(INotificationService notificationService,
            IMessageQueryService messageQueryService,
            IMessageCommandService messageCommandService,
            IChannelQueryService channelQueryService,
            IConversationQueryService conversationQueryService,
            ICacheService cacheService, IFileHelper fileHelper)
        {
            _notificationService = notificationService;
            _messageQueryService = messageQueryService;
            _messageCommandService = messageCommandService;
            _channelQueryService = channelQueryService;
            _conversationQueryService = conversationQueryService;
            _cacheService = cacheService;
        }

        // GET api/messages/channel/1/1
        [HttpGet("channel/{id}/{currentPage}")]
        public async Task<ActionResult<MessageLoadDto>> GetMessagesForChannelAsync(int id, int currentPage)
        {
            var messageLoadDto = await _messageQueryService.GetMessagesForChannelAsync(id, User.GetUserId(), User.GetWorkspaceId(), currentPage);

            await _cacheService.RemoveUnreadChannelForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            await _cacheService.SetActiveSidebarItemAsync(true, id, User.GetUserId(), User.GetWorkspaceId());

            await _notificationService.SendUpdateChannelListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageLoadDto;
        }

        // GET api/messages/conversation/1/1
        [HttpGet("conversation/{id}/{currentPage}")]
        public async Task<ActionResult<MessageLoadDto>> GetMessagesForConversationAsync(int id, int currentPage)
        {
            var messageLoadDto = await _messageQueryService.GetMessagesForConversationAsync(id, User.GetUserId(), User.GetWorkspaceId(), currentPage);

            if (!await _conversationQueryService.IsSelfConversationAsync(id, User.GetUserId()))
            {
                await _cacheService.ClearUnreadConversationMessageForUserAsync(id, User.GetUserId(), User.GetWorkspaceId());
            }
            await _cacheService.SetActiveSidebarItemAsync(false, id, User.GetUserId(), User.GetWorkspaceId());

            await _notificationService.SendUpdateConversationListNotificationAsync(new[] { User.GetUserId() }, id);

            return messageLoadDto;
        }

        // GET api/messages/channel/1/singleMessage/1
        [HttpGet("channel/{channelId}/singleMessage/{messageId}")]
        public async Task<ActionResult<MessageGroupDto>> GetSingleMessagesForChannelAsync(int channelId, int messageId)
        {
            var messageGroupDto = await _messageQueryService.GetSingleMessagesForChannelAsync(channelId, messageId, User.GetUserId());

            await _cacheService.RemoveUnreadChannelForUserAsync(channelId, User.GetUserId(), User.GetWorkspaceId());
            await _notificationService.SendUpdateChannelListNotificationAsync(new[] { User.GetUserId() }, channelId);

            return messageGroupDto;
        }

        // GET api/messages/conversation/1/singleMessage/1
        [HttpGet("conversation/{conversationId}/singleMessage/{messageId}")]
        public async Task<ActionResult<MessageGroupDto>> GetSingleMessagesForConversationAsync(int conversationId, int messageId)
        {
            var messageGroupDto = await _messageQueryService.GetSingleMessagesForConversationAsync(conversationId, messageId, User.GetUserId());

            if (!await _conversationQueryService.IsSelfConversationAsync(conversationId, User.GetUserId()))
            {
                await _cacheService.ClearUnreadConversationMessageForUserAsync(conversationId, User.GetUserId(), User.GetWorkspaceId());
            }
            await _notificationService.SendUpdateConversationListNotificationAsync(new[] { User.GetUserId() }, conversationId);

            return messageGroupDto;
        }

        private async Task SendConversationMessageItemChangeNotificationAsync(int conversationId, int messageId, MessageChangeType type)
        {
            var userIds = (await _conversationQueryService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            await _notificationService.SendConversationMessageItemChangeNotificationAsync(userIds, conversationId, type, messageId);
        }

        private async Task SendChannelMessageItemChangeNotificationAsync(int channelId, int messageId, MessageChangeType type)
        {
            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            await _notificationService.SendChannelMessageItemChangeNotificationAsync(userIds, channelId, type, messageId);
        }
        
        // POST api/messages/conversation/1
        [HttpPost("conversation/{id}")]
        public async Task<IActionResult> PostMessageToConversationAsync(int id, MessagePostDto messagePostDto)
        {
            await _messageCommandService.PostMessageToConversationAsync(messagePostDto.MessageContent, id, User.GetUserId(), User.GetWorkspaceId());
            return Ok();
        }

        // POST api/messages/channel/1
        [HttpPost("channel/{id}")]
        public async Task<IActionResult> PostMessageToChannelAsync(int id, MessagePostDto messagePostDto)
        {
            await _messageCommandService.PostMessageToChannelAsync(messagePostDto.MessageContent, id, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/messages/conversation/1/update/1
        [HttpPost("conversation/{conversationId}/update/{messageId}")]
        public async Task<IActionResult> UpdateMessageInConversationAsync(int conversationId, int messageId, MessagePostDto messagePostDto)
        {
            await _messageCommandService.UpdateMessageInConversationAsync(messagePostDto.MessageContent, conversationId, messageId, User.GetUserId());
            await SendConversationMessageItemChangeNotificationAsync(conversationId, messageId, MessageChangeType.Edited);

            return Ok();
        }

        // POST api/messages/channel/1/update/1
        [HttpPost("channel/{channelId}/update/{messageId}")]
        public async Task<IActionResult> UpdateMessageInChannelAsync(int channelId, int messageId, MessagePostDto messagePostDto)
        {
            await _messageCommandService.UpdateMessageInChannelAsync(messagePostDto.MessageContent, channelId, messageId, User.GetUserId());
            await SendChannelMessageItemChangeNotificationAsync(channelId, messageId, MessageChangeType.Edited);

            return Ok();
        }

        // POST api/messages/conversation/1/uploadFile
        [HttpPost("conversation/{conversationId}/uploadFile")]
        public async Task<IActionResult> UploadFilesToConversationAsync(IList<IFormFile> files, int conversationId)
        {
            await _messageCommandService.PostFileMessageToConversationAsync(files, conversationId, User.GetUserId(), User.GetWorkspaceId());
            return Ok();
        }

        // POST api/messages/channel/1/uploadFile
        [HttpPost("channel/{channelId}/uploadFile")]
        public async Task<IActionResult> UploadFilesToChannelAsync(IList<IFormFile> files, int channelId)
        {
            await _messageCommandService.PostFileMessageToChannelAsync(files, channelId, User.GetUserId(), User.GetWorkspaceId());
            return Ok();
        }

        // GET api/messages/downloadFile/1
        [HttpGet("downloadFile/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync(int fileId)
        {
            var (stream, contentType) = await _messageCommandService.DownloadFileAsync(fileId, User.GetUserId(), User.GetWorkspaceId());
            return File(stream, contentType);
        }

        // POST api/messages/conversation/1/shareFile/1
        [HttpPost("conversation/{conversationId}/shareFile/{fileId}")]
        public async Task<IActionResult> ShareFileToConversationAsync(int conversationId, int fileId)
        {
            await _messageCommandService.ShareFileToConversationAsync(conversationId, fileId, User.GetUserId(), User.GetWorkspaceId());
            return Ok();
        }

        // POST api/messages/channel/1/shareFile/1
        [HttpPost("channel/{channelId}/shareFile/{fileId}")]
        public async Task<IActionResult> ShareFileToChannelAsync(int channelId, int fileId)
        {
            await _messageCommandService.ShareFileToChannelAsync(channelId, fileId, User.GetUserId(), User.GetWorkspaceId());
            return Ok();
        }

        // POST api/messages/conversation/1/deleteMessage
        [HttpPost("conversation/{conversationId}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromConversationAsync(int conversationId, [FromBody]int messageId)
        {
            await _messageCommandService.DeleteMessageAsync(messageId, User.GetUserId());
            await SendConversationMessageItemChangeNotificationAsync(conversationId, messageId, MessageChangeType.Deleted);

            return Ok();
        }

        // POST api/messages/channel/1/deleteMessage
        [HttpPost("channel/{channelId}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromChannelAsync(int channelId, [FromBody]int messageId)
        {
            await _messageCommandService.DeleteMessageAsync(messageId, User.GetUserId());
            await SendChannelMessageItemChangeNotificationAsync(channelId, messageId, MessageChangeType.Deleted);

            return Ok();
        }

        // POST api/messages/conversation/1/addReaction/1
        [HttpPost("conversation/{conversationId}/addReaction/{messageId}")]
        public async Task<IActionResult> AddReactionToMessageInConversationAsync(int conversationId, int messageId, [FromBody]string emojiColons)
        {
            await _messageCommandService.AddReactionToMessageAsync(messageId, emojiColons, User.GetUserId());
            await SendConversationMessageItemChangeNotificationAsync(conversationId, messageId, MessageChangeType.Edited);

            return Ok();
        }

        // POST api/messages/channel/1/addReaction/1
        [HttpPost("channel/{channelId}/addReaction/{messageId}")]
        public async Task<IActionResult> AddReactionToMessageInChannelAsync(int channelId, int messageId, [FromBody]string emojiColons)
        {
            await _messageCommandService.AddReactionToMessageAsync(messageId, emojiColons, User.GetUserId());
            await SendChannelMessageItemChangeNotificationAsync(channelId, messageId, MessageChangeType.Edited);

            return Ok();
        }

        // POST api/messages/conversation/1/removeReaction/1
        [HttpPost("conversation/{conversationId}/removeReaction/{messageId}")]
        public async Task<IActionResult> RemoveReactionToMessageInConversationAsync(int conversationId, int messageId, [FromBody]string emojiColons)
        {
            await _messageCommandService.RemoveReactionToMessageAsync(messageId, emojiColons, User.GetUserId());
            await SendConversationMessageItemChangeNotificationAsync(conversationId, messageId, MessageChangeType.Edited);

            return Ok();
        }

        // POST api/messages/channel/1/removeReaction/1
        [HttpPost("channel/{channelId}/removeReaction/{messageId}")]
        public async Task<IActionResult> RemoveReactionToMessageInChannelAsync(int channelId, int messageId, [FromBody]string emojiColons)
        {
            await _messageCommandService.RemoveReactionToMessageAsync(messageId, emojiColons, User.GetUserId());
            await SendChannelMessageItemChangeNotificationAsync(channelId, messageId, MessageChangeType.Edited);

            return Ok();
        }
    }
}
