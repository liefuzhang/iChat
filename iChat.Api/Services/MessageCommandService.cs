using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = iChat.Api.Models.File;

namespace iChat.Api.Services {
    public class MessageCommandService : IMessageCommandService {
        private readonly iChatContext _context;
        private readonly IMessageParsingHelper _messageParsingHelper;
        private readonly IChannelQueryService _channelQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly IFileHelper _fileHelper;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;

        public MessageCommandService(iChatContext context, IMessageParsingHelper messageParsingHelper, IFileHelper fileHelper, IChannelQueryService channelQueryService,
            IUserQueryService userQueryService, IConversationQueryService conversationQueryService, ICacheService cacheService, INotificationService notificationService) {
            _context = context;
            _messageParsingHelper = messageParsingHelper;
            _fileHelper = fileHelper;
            _channelQueryService = channelQueryService;
            _userQueryService = userQueryService;
            _conversationQueryService = conversationQueryService;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        private async Task SendConversationMessageItemChangedNotificationAsync(int conversationId, int messageId, MessageChangeType type) {
            var userIds = (await _conversationQueryService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            await _notificationService.SendConversationMessageItemChangedNotificationAsync(userIds, conversationId, type, messageId);
        }

        private async Task SendChannelMessageItemChangedNotificationAsync(int channelId, int messageId, MessageChangeType type) {
            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            await _notificationService.SendChannelMessageItemChangedNotificationAsync(userIds, channelId, type, messageId);
        }

        private async Task NotifyForNewConversationMessageAsync(int conversationId, int messageId, int userId, int workspaceId) {
            if (!await _conversationQueryService.IsSelfConversationAsync(conversationId, userId)) {
                var userIds = (await _conversationQueryService.GetAllConversationUserIdsAsync(conversationId)).ToList();
                await _cacheService.AddUnreadConversationMessageForUsersAsync(conversationId, messageId, userIds, workspaceId);
            }

            await SendConversationMessageItemChangedNotificationAsync(conversationId, messageId, MessageChangeType.Added);
        }

        private async Task NotifyForNewChannelMessageAsync(int channelId, int messageId, int workspaceId, List<int> mentionUserIds = null) {
            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            await _cacheService.AddUnreadChannelMessageForUsersAsync(channelId, messageId, userIds, workspaceId, mentionUserIds);
            await SendChannelMessageItemChangedNotificationAsync(channelId, messageId, MessageChangeType.Added);
        }

        private async Task PostMessageToConversationCommonAsync(ConversationMessage conversationMessage) {
            _context.ConversationMessages.Add(conversationMessage);
            await _context.SaveChangesAsync();

            await NotifyForNewConversationMessageAsync(conversationMessage.ConversationId, conversationMessage.Id,
                conversationMessage.SenderId, conversationMessage.WorkspaceId);
        }

        private async Task PostMessageToChannelCommonAsync(ChannelMessage channelMessage, List<int> mentionUserIds = null) {
            _context.ChannelMessages.Add(channelMessage);
            await _context.SaveChangesAsync();

            await NotifyForNewChannelMessageAsync(channelMessage.ChannelId, channelMessage.Id, channelMessage.WorkspaceId, mentionUserIds);
        }

        public async Task PostMessageToConversationAsync(string newMessageContent, int conversationId, int currentUserId,
            int workspaceId) {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ConversationMessage(conversationId, content, currentUserId, workspaceId);

            await PostMessageToConversationCommonAsync(message);
        }

        public async Task PostMessageToChannelAsync(string newMessageContent, int channelId, int currentUserId,
            int workspaceId, List<int> mentionUserIds) {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ChannelMessage(channelId, content, currentUserId, workspaceId);

            await PostMessageToChannelCommonAsync(message, mentionUserIds);
        }

        public async Task PostJoinConversationSystemMessageAsync(int conversationId, List<int> userIds, int invitedByUserId, int workspaceId) {
            var userNames = await _userQueryService.GetUserNamesAsync(userIds.Skip(1).ToList(), workspaceId);
            var invitedByUserName = await _userQueryService.GetUserNamesAsync(new List<int> { invitedByUserId }, workspaceId);
            var content = $"joined conversation" + (string.IsNullOrEmpty(userNames) ? "" : $" along with {userNames}") +
                    $" on the invitation of {invitedByUserName}.";
            var message = new ConversationMessage(conversationId, content, userIds.First(), workspaceId, false, true);

            await PostMessageToConversationCommonAsync(message);
        }

        public async Task PostJoinChannelSystemMessageAsync(int channelId, List<int> userIds, int workspaceId) {
            var userNames = await _userQueryService.GetUserNamesAsync(userIds.Skip(1).ToList(), workspaceId);
            var channel = await _channelQueryService.GetChannelByIdAsync(channelId, workspaceId);
            var content = $"joined {channel.Name}" + (string.IsNullOrEmpty(userNames) ? "." : $" along with {userNames}.");
            var message = new ChannelMessage(channelId, content, userIds.First(), workspaceId, false, true);

            await PostMessageToChannelCommonAsync(message);
        }

        private async Task<IEnumerable<File>> UploadAndSaveFilesForMessageAsync(IList<IFormFile> files, int userId, int workspaceId) {
            var savedFiles = new List<File>();
            foreach (var file in files) {
                var savedFileName = await _fileHelper.UploadFileAsync(file, workspaceId);
                var newFile = new File(savedFileName, file.FileName, file.ContentType, userId, workspaceId);
                _context.Files.Add(newFile);

                await _context.SaveChangesAsync();

                savedFiles.Add(newFile);
            }

            return savedFiles;
        }

        public async Task PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId) {
            if (!files.Any()) {
                throw new ArgumentException($"File list is empty.");
            }

            var savedFiles = await UploadAndSaveFilesForMessageAsync(files, userId, workspaceId);

            var message = new ConversationMessage(conversationId, string.Empty, userId, workspaceId, true);
            foreach (var file in savedFiles) {
                message.AddMessageFileAttachment(file);
            }

            await PostMessageToConversationCommonAsync(message);
        }

        public async Task PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId) {
            if (!files.Any()) {
                throw new ArgumentException($"File list is empty.");
            }

            var savedFiles = await UploadAndSaveFilesForMessageAsync(files, userId, workspaceId);

            var message = new ChannelMessage(channelId, string.Empty, userId, workspaceId, true);
            foreach (var file in savedFiles) {
                message.AddMessageFileAttachment(file);
            }

            await PostMessageToChannelCommonAsync(message);
        }

        public async Task ShareFileToConversationAsync(int conversationId, int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");

            }

            var message = new ConversationMessage(conversationId, string.Empty, userId, workspaceId, true);
            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            message.AddMessageFileAttachment(file);

            await PostMessageToConversationCommonAsync(message);
        }

        public async Task ShareFileToChannelAsync(int channelId, int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");

            }

            var message = new ChannelMessage(channelId, string.Empty, userId, workspaceId, true);
            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            message.AddMessageFileAttachment(file);

            await PostMessageToChannelCommonAsync(message);
        }

        public async Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");
            }

            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            return (await _fileHelper.DownloadFileAsync(file.SavedName, workspaceId), file.ContentType);
        }

        public string GetStringifiedMessageHtml(string messageHtml) {
            return _messageParsingHelper.Stringify(messageHtml);
        }

        private async Task<bool> UpdateMessageContentAsync(Message messageInDb, string message) {
            var content = _messageParsingHelper.Parse(message);
            if (content == messageInDb.Content) {
                return false;
            }

            messageInDb.UpdateContent(content);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateMessageInConversationAsync(string messageContent, int conversationId, int messageId, int currentUserId) {
            var messageInDb = _context.ConversationMessages.Single(cm => cm.SenderId == currentUserId && cm.ConversationId == conversationId &&
                                                                         cm.Id == messageId);
            if (!await UpdateMessageContentAsync(messageInDb, messageContent)) {
                return;
            }

            await SendConversationMessageItemChangedNotificationAsync(conversationId, messageId, MessageChangeType.Edited);
        }

        public async Task UpdateMessageInChannelAsync(string messageContent, int channelId, int messageId, int currentUserId,
            int workspaceId, List<int> mentionUserIds) {
            var messageInDb = _context.ChannelMessages.Single(cm => cm.SenderId == currentUserId && cm.ChannelId == channelId &&
                                                                    cm.Id == messageId);
            if (!await UpdateMessageContentAsync(messageInDb, messageContent)) {
                return;
            }

            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            await _cacheService.UpdateUnreadChannelMessageMentionForUsersAsync(channelId, messageId, userIds, workspaceId, mentionUserIds);
            await SendChannelMessageItemChangedNotificationAsync(channelId, messageId, MessageChangeType.Edited);
        }

        private async Task DeleteMessageCommonAsync(int messageId, int userId, int workspaceId) {
            var message = await _context.Messages
                .Include(m => m.MessageFileAttachments)
                .SingleAsync(m => m.Id == messageId && m.SenderId == userId);

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();


            if (message.HasFileAttachments) {
                var fileIds = message.MessageFileAttachments.Select(mfa => mfa.FileId);
                await RemoveFilesForMessageAsync(fileIds, workspaceId);
            }
        }

        private async Task RemoveFilesForMessageAsync(IEnumerable<int> fileIds, int workspaceId) {
            var files = await _context.Files
                .Include(f => f.MessageFileAttachments)
                .Where(f => fileIds.Contains(f.Id)).ToListAsync();

            foreach (var file in files) {
                if (file.MessageFileAttachments.Any()) {
                    continue;
                }

                _context.Files.Remove(file);
                await _context.SaveChangesAsync();

                await _fileHelper.DeleteFileAsync(file.SavedName, workspaceId);
            }
        }

        private async Task ClearUnreadConversationMessageIdForUsersAsync(int conversationId, int messageId, int workspaceId) {
            var userIds = (await _conversationQueryService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            foreach (var id in userIds) {
                await _cacheService.ClearUnreadConversationMessageIdForUserAsync(conversationId, messageId, id, workspaceId);
            }

            await _notificationService.SendUnreadConversationMessageClearedNotificationAsync(userIds, conversationId);
        }

        public async Task DeleteMessageInConversationAsync(int conversationId, int messageId, int userId,
            int workspaceId) {
            await DeleteMessageCommonAsync(messageId, userId, workspaceId);

            await ClearUnreadConversationMessageIdForUsersAsync(conversationId, messageId, workspaceId);
            await SendConversationMessageItemChangedNotificationAsync(conversationId, messageId, MessageChangeType.Deleted);
        }

        private async Task ClearUnreadChannelMessageForUserAsync(int channelId, int messageId, int workspaceId) {
            var userIds = (await _channelQueryService.GetAllChannelUserIdsAsync(channelId)).ToList();
            foreach (var id in userIds) {
                await _cacheService.ClearUnreadChannelMessageForUserAsync(channelId, messageId, id, workspaceId);
            }

            await _notificationService.SendUnreadChannelMessageClearedNotificationAsync(userIds, channelId);
        }

        public async Task DeleteMessageInChannelAsync(int channelId, int messageId, int userId,
            int workspaceId) {
            await DeleteMessageCommonAsync(messageId, userId, workspaceId);

            await ClearUnreadChannelMessageForUserAsync(channelId, messageId, workspaceId);
            await SendChannelMessageItemChangedNotificationAsync(channelId, messageId, MessageChangeType.Deleted);
        }

        private async Task<bool> EligibleForTheFileAsync(int fileId, int userId, int workspaceId) {
            if (!await _context.Files.AnyAsync(f => f.Id == fileId && f.WorkspaceId == workspaceId)) {
                return false;
            }

            var messageIds = await _context.MessageFileAttachments
                .Where(mfa => mfa.FileId == fileId)
                .Select(mfa => mfa.MessageId)
                .ToListAsync();

            if (await _context.ChannelMessages
                .AnyAsync(cm => messageIds.Contains(cm.Id) &&
                                _context.ChannelSubscriptions.Any(cs => cs.ChannelId == cm.ChannelId && cs.UserId == userId))) {
                return true;
            }

            if (await _context.ConversationMessages
                .AnyAsync(cm => messageIds.Contains(cm.Id) &&
                                _context.ConversationUsers.Any(cu => cu.ConversationId == cm.ConversationId && cu.UserId == userId))) {
                return true;
            }

            return false;
        }

        private async Task AddReactionToMessageCommonAsync(int messageId, string emojiColons, int userId) {
            var messageReaction = await _context.MessageReactions.SingleOrDefaultAsync(mr => mr.MessageId == messageId && mr.EmojiColons == emojiColons);
            if (messageReaction == null) {
                messageReaction = new MessageReaction(messageId, emojiColons);
                _context.MessageReactions.Add(messageReaction);
                await _context.SaveChangesAsync();
            }

            var messageReactionUser = new MessageReactionUser(messageReaction.Id, userId);
            _context.MessageReactionUsers.Add(messageReactionUser);
            await _context.SaveChangesAsync();

        }

        public async Task AddReactionToMessageInConversationAsync(int conversationId, int messageId, string emojiColons,
            int userId) {
            await AddReactionToMessageCommonAsync(messageId, emojiColons, userId);

            await SendConversationMessageItemChangedNotificationAsync(conversationId, messageId, MessageChangeType.Edited);
        }

        public async Task AddReactionToMessageInChannelAsync(int channelId, int messageId, string emojiColons,
            int userId) {
            await AddReactionToMessageCommonAsync(messageId, emojiColons, userId);

            await SendChannelMessageItemChangedNotificationAsync(channelId, messageId, MessageChangeType.Edited);
        }

        private async Task RemoveReactionToMessageCommonAsync(int messageId, string emojiColons, int userId) {
            var messageReaction = await _context.MessageReactions
                .Include(mr => mr.MessageReactionUsers)
                .SingleAsync(mr => mr.MessageId == messageId && mr.EmojiColons == emojiColons);
            var messageReactionUser = messageReaction.MessageReactionUsers.Single(mru => mru.UserId == userId);
            messageReaction.MessageReactionUsers.Remove(messageReactionUser);

            if (!messageReaction.MessageReactionUsers.Any()) {
                _context.MessageReactions.Remove(messageReaction);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveReactionToMessageInConversationAsync(int conversationId, int messageId,
            string emojiColons, int userId) {
            await RemoveReactionToMessageCommonAsync(messageId, emojiColons, userId);

            await SendConversationMessageItemChangedNotificationAsync(conversationId, messageId, MessageChangeType.Edited);
        }

        public async Task RemoveReactionToMessageInChannelAsync(int channelId, int messageId, string emojiColons,
            int userId) {
            await RemoveReactionToMessageCommonAsync(messageId, emojiColons, userId);

            await SendChannelMessageItemChangedNotificationAsync(channelId, messageId, MessageChangeType.Edited);
        }

    }
}