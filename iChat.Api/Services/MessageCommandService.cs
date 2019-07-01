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

namespace iChat.Api.Services
{
    public class MessageCommandService : IMessageCommandService
    {
        private readonly iChatContext _context;
        private readonly IMessageParsingHelper _messageParsingHelper;
        private readonly IFileHelper _fileHelper;

        public MessageCommandService(iChatContext context, IMessageParsingHelper messageParsingHelper, IFileHelper fileHelper)
        {
            _context = context;
            _messageParsingHelper = messageParsingHelper;
            _fileHelper = fileHelper;
        }

        public async Task<int> PostMessageToConversationAsync(string newMessageContent, int conversationId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false)
        {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ConversationMessage(conversationId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ConversationMessages.Add(message);
            await _context.SaveChangesAsync();

            return message.Id;
        }

        public async Task<int> PostMessageToChannelAsync(string newMessageContent, int channelId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false)
        {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ChannelMessage(channelId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ChannelMessages.Add(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        private async Task UpdateMessageContent(Message messageInDb, string message)
        {
            var content = _messageParsingHelper.Parse(message);
            if (content == messageInDb.Content)
            {
                return;
            }

            messageInDb.UpdateContent(content);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageInConversationAsync(string messageContent, int conversationId, int messageId, int currentUserId)
        {
            var messageInDb = _context.ConversationMessages.Single(cm => cm.SenderId == currentUserId && cm.ConversationId == conversationId &&
                                                                    cm.Id == messageId);
            await UpdateMessageContent(messageInDb, messageContent);
        }

        public async Task UpdateMessageInChannelAsync(string messageContent, int channelId, int messageId, int currentUserId)
        {
            var messageInDb = _context.ChannelMessages.Single(cm => cm.SenderId == currentUserId && cm.ChannelId == channelId &&
                                                                    cm.Id == messageId);
            await UpdateMessageContent(messageInDb, messageContent);
        }

        private async Task AddNewFileForMessageAsync(string savedFileName, string fileName, string contentType, int messageId, int userId,
            int workspaceId)
        {
            var newFile = new Models.File(savedFileName, fileName, contentType, userId, workspaceId);
            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();

            await AttachFileToMessageAsync(messageId, newFile.Id);
        }

        private async Task AttachFileToMessageAsync(int messageId, int fileId)
        {
            _context.MessageFileAttachments.Add(new MessageFileAttachment(messageId, fileId));
            await _context.SaveChangesAsync();
        }

        private async Task UploadAndSaveFilesForMessageAsync(IList<IFormFile> files, int messageId, int userId, int workspaceId)
        {
            foreach (var file in files)
            {
                var savedFileName = await _fileHelper.UploadFileAsync(file, workspaceId);
                await AddNewFileForMessageAsync(savedFileName, file.FileName, file.ContentType, messageId, userId, workspaceId);
            }
        }

        public async Task<int> PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId)
        {
            if (!files.Any())
            {
                throw new ArgumentException($"File list is empty.");
            }

            var messageId = await PostMessageToConversationAsync(string.Empty, conversationId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);

            return messageId;
        }

        public async Task<int> PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId)
        {
            if (!files.Any())
            {
                throw new ArgumentException($"File list is empty.");
            }

            var messageId = await PostMessageToChannelAsync(string.Empty, channelId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);

            return messageId;
        }

        public async Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId)
        {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId)))
            {
                throw new ArgumentException($"File access required.");
            }

            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            return (await _fileHelper.DownloadFileAsync(file.SavedName, workspaceId), file.ContentType);
        }

        public async Task<int> ShareFileToConversationAsync(int conversationId, int fileId, int userId, int workspaceId)
        {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId)))
            {
                throw new ArgumentException($"File access required.");

            }

            var messageId = await PostMessageToConversationAsync(string.Empty, conversationId, userId, workspaceId, true);
            await AttachFileToMessageAsync(messageId, fileId);

            return messageId;
        }

        public async Task<int> ShareFileToChannelAsync(int channelId, int fileId, int userId, int workspaceId)
        {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId)))
            {
                throw new ArgumentException($"File access required.");

            }

            var messageId = await PostMessageToChannelAsync(string.Empty, channelId, userId, workspaceId, true);
            await AttachFileToMessageAsync(messageId, fileId);

            return messageId;
        }

        public async Task DeleteMessageAsync(int messageId, int userId)
        {
            var message = await _context.Messages.SingleAsync(m => m.Id == messageId && m.SenderId == userId);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> EligibleForTheFileAsync(int fileId, int userId, int workspaceId)
        {
            if (!await _context.Files.AnyAsync(f => f.Id == fileId && f.WorkspaceId == workspaceId))
            {
                return false;
            }

            var messageIds = await _context.MessageFileAttachments
                .Where(mfa => mfa.FileId == fileId)
                .Select(mfa => mfa.MessageId)
                .ToListAsync();

            if (await _context.ChannelMessages
                .AnyAsync(cm => messageIds.Contains(cm.Id) &&
                                _context.ChannelSubscriptions.Any(cs => cs.ChannelId == cm.ChannelId && cs.UserId == userId)))
            {
                return true;
            }

            if (await _context.ConversationMessages
                .AnyAsync(cm => messageIds.Contains(cm.Id) &&
                                _context.ConversationUsers.Any(cu => cu.ConversationId == cm.ConversationId && cu.UserId == userId)))
            {
                return true;
            }

            return false;
        }

        public async Task AddReactionToMessageAsync(int messageId, string emojiColons, int userId)
        {
            var messageReaction = await _context.MessageReactions.SingleOrDefaultAsync(mr => mr.MessageId == messageId && mr.EmojiColons == emojiColons);
            if (messageReaction == null)
            {
                messageReaction = new MessageReaction(messageId, emojiColons);
                _context.MessageReactions.Add(messageReaction);
                await _context.SaveChangesAsync();
            }

            var messageReactionUser = new MessageReactionUser(messageReaction.Id, userId);
            _context.MessageReactionUsers.Add(messageReactionUser);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReactionToMessageAsync(int messageId, string emojiColons, int userId)
        {
            var messageReaction = await _context.MessageReactions
                .Include(mr => mr.MessageReactionUsers)
                .SingleAsync(mr => mr.MessageId == messageId && mr.EmojiColons == emojiColons);
            var messageReactionUser = messageReaction.MessageReactionUsers.Single(mru => mru.UserId == userId);
            messageReaction.MessageReactionUsers.Remove(messageReactionUser);

            if (!messageReaction.MessageReactionUsers.Any())
            {
                _context.MessageReactions.Remove(messageReaction);
            }

            await _context.SaveChangesAsync();
        }
    }
}