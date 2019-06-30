using AutoMapper;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MessageDto = iChat.Api.Dtos.MessageDto;

namespace iChat.Api.Services {
    public class MessageService : IMessageService {
        private readonly iChatContext _context;
        private readonly IChannelService _channelService;
        private readonly IConversationService _conversationService;
        private readonly IUserService _userService;
        private readonly IMessageParsingHelper _messageParsingHelper;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        public MessageService(iChatContext context, IChannelService channelService, IConversationService conversationService,
            IUserService userService, IMessageParsingHelper messageParsingHelper, IMapper mapper, IFileHelper fileHelper) {
            _context = context;
            _channelService = channelService;
            _conversationService = conversationService;
            _userService = userService;
            _messageParsingHelper = messageParsingHelper;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }

        private void AllowConsecutiveMessages(List<MessageGroupDto> groups) {
            var maxDiffInMin = 3;
            groups.ForEach(g => {
                var messages = g.Messages.ToList();
                for (var i = 0; i < messages.Count(); i++) {
                    if (i == 0 || messages[i - 1].SenderId != messages[i].SenderId) {
                        continue;
                    }
                    var time = DateTime.Parse(messages[i].TimeString);
                    var prevTime = DateTime.Parse(messages[i - 1].TimeString);
                    if ((time - prevTime).Minutes <= maxDiffInMin) {
                        messages[i].IsConsecutiveMessage = true;
                    }
                }
                g.Messages = messages;
            });
        }

        private async Task AddFilesToMessagesAsync(IEnumerable<MessageDto> messages) {
            var fileMessages = messages.Where(m => m.HasFileAttachments).ToList();
            var fileMessageIds = fileMessages.Select(m => m.Id);
            var files = await _context.Files.Include(f => f.MessageFileAttachments)
                    .Where(f => f.MessageFileAttachments
                                .Any(mfa => fileMessageIds.Contains(mfa.MessageId))).ToListAsync();
            foreach (var fileMessage in fileMessages) {
                var fileAttachments = files.Where(f => f.MessageFileAttachments.Any(mfa => mfa.MessageId == fileMessage.Id)).ToList();
                fileMessage.FileAttachments = _mapper.Map<List<FileDto>>(fileAttachments);
            }
        }

        private void SortMessageReactionsByCreatedDate(IEnumerable<MessageDto> messages) {
            foreach (var m in messages) {
                m.MessageReactions = m.MessageReactions.OrderBy(mr => mr.CreatedDate).ToList();
            }

        }

        private async Task AddReactionUsersToMessagesAsync(IEnumerable<MessageDto> messages) {
            var messageReactions = messages.SelectMany(m => m.MessageReactions).ToList();
            var messageReationIds = messageReactions.Select(mr => mr.Id);
            var messageReactionUsers = await _context.MessageReactionUsers
                    .Include(mru => mru.User)
                    .Where(mru => messageReationIds.Any(id => id == mru.MessageReactionId))
                    .ToListAsync();
            foreach (var messageReaction in messageReactions) {
                var users = messageReactionUsers.Where(mru => mru.MessageReactionId == messageReaction.Id)
                    .Select(mru => mru.User);
                messageReaction.Users = _mapper.Map<List<UserDto>>(users);
            }
        }

        private async Task AddAssociatedDataToMessagesAsync(List<MessageDto> messageDtos) {
            await AddFilesToMessagesAsync(messageDtos);
            SortMessageReactionsByCreatedDate(messageDtos);
            await AddReactionUsersToMessagesAsync(messageDtos);
        }


        private async Task<List<MessageGroupDto>> GetMessageGroupsAsync(IQueryable<Message> baseQuery, int currentPage) {
            var messages = await baseQuery
                .Include(m => m.MessageReactions)
                .OrderByDescending(m => m.CreatedDate)
                .Skip((currentPage - 1) * iChatConstants.DefaultMessagePageSize)
                .Take(iChatConstants.DefaultMessagePageSize)
                .ToListAsync();

            var groups = messages
                .OrderBy(m => m.CreatedDate)
                .GroupBy(m => m.CreatedDate.Date)
                .OrderBy(group => group.Key)
                .Select(group =>
                    new MessageGroupDto {
                        DateString = group.First().DateString,
                        Messages = group.Select(m => _mapper.Map<MessageDto>(m))
                    })
                .ToList();

            AllowConsecutiveMessages(groups);

            var messageDtos = groups.SelectMany(g => g.Messages).ToList();
            await AddAssociatedDataToMessagesAsync(messageDtos);

            return groups;
        }

        private async Task<MessageLoadDto> GetMessageLoadAsync(IQueryable<Message> baseQuery, int currentPage) {
            var messageGroups = await GetMessageGroupsAsync(baseQuery, currentPage);
            var totalPage = (baseQuery.Count() - 1) / iChatConstants.DefaultMessagePageSize + 1;

            return new MessageLoadDto {
                TotalPage = totalPage,
                MessageGroupDtos = messageGroups
            };
        }

        private async Task AddMessageChannelDescriptionForChannel(MessageLoadDto messageLoad, int channelId, int workspaceId) {
            var channel = await _channelService.GetChannelByIdAsync(channelId, workspaceId);
            var createdByUser = await _userService.GetUserByIdAsync(channel.CreatedByUserId);
            messageLoad.MessageChannelDescriptionDto = new MessageChannelDescriptionDto {
                CreatedByUser = _mapper.Map<UserDto>(createdByUser),
                CreatedDataString = channel.CreatedDateString
            };
        }
        
        public async Task<MessageLoadDto> GetMessagesForChannelAsync(int channelId, int userId, int workspaceId, int currentPage) {
            if (!_channelService.IsUserSubscribedToChannel(channelId, userId)) {
                throw new ArgumentException($"User is not subsribed to channel.");
            }

            var messageGroupsBaseQuery = _context.ChannelMessages
                .Include(m => m.Sender)
                .Where(m => m.ChannelId == channelId && m.WorkspaceId == workspaceId);
            var messageLoad = await GetMessageLoadAsync(messageGroupsBaseQuery, currentPage);
            await AddMessageChannelDescriptionForChannel(messageLoad, channelId, workspaceId);

            return messageLoad;
        }

        private async Task AddMessageChannelDescriptionForConversation(MessageLoadDto messageLoad, int conversationId, int userId, int workspaceId) {
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId, userId, workspaceId);
            var createdByUser = await _userService.GetUserByIdAsync(conversation.CreatedByUserId);
            messageLoad.MessageChannelDescriptionDto = new MessageChannelDescriptionDto {
                CreatedByUser = _mapper.Map<UserDto>(createdByUser),
                CreatedDataString = conversation.CreatedDateString
            };
        }

        public async Task<MessageLoadDto> GetMessagesForConversationAsync(int conversationId, int userId, int workspaceId, int currentPage) {
            if (!_conversationService.IsUserInConversation(conversationId, userId)) {
                throw new ArgumentException($"User is not in conversation.");
            }

            var messageGroupsBaseQuery = _context.ConversationMessages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId && m.WorkspaceId == workspaceId);
            var messageLoad = await GetMessageLoadAsync(messageGroupsBaseQuery, currentPage);
            await AddMessageChannelDescriptionForConversation(messageLoad, conversationId, userId, workspaceId);

            return messageLoad;
        }

        private async Task<MessageGroupDto> GetMessageGroupForSingleMessageAsync(Message message) {
            var messageDto = _mapper.Map<MessageDto>(message);

            var messageDtos = new List<MessageDto>(new[] { messageDto });
            await AddAssociatedDataToMessagesAsync(messageDtos);

            return new MessageGroupDto() {
                DateString = message.DateString,
                Messages = messageDtos
            };
        }

        public async Task<MessageGroupDto> GetSingleMessagesForChannelAsync(int channelId, int messageId, int userId) {
            if (!_channelService.IsUserSubscribedToChannel(channelId, userId)) {
                throw new ArgumentException($"User is not subsribed to channel.");
            }

            var message = await _context.ChannelMessages
                .Include(m => m.Sender)
                .Include(m => m.MessageReactions)
                .SingleAsync(m => m.Id == messageId && m.ChannelId == channelId);

            return await GetMessageGroupForSingleMessageAsync(message);
        }

        public async Task<MessageGroupDto> GetSingleMessagesForConversationAsync(int conversationId, int messageId, int userId) {
            if (!_conversationService.IsUserInConversation(conversationId, userId)) {
                throw new ArgumentException($"User is not in conversation.");
            }

            var message = await _context.ConversationMessages
                .Include(m => m.Sender)
                .Include(m => m.MessageReactions)
                .SingleAsync(m => m.Id == messageId && m.ConversationId == conversationId);

            return await GetMessageGroupForSingleMessageAsync(message);
        }

        public async Task<int> PostMessageToConversationAsync(string newMessageContent, int conversationId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false) {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ConversationMessage(conversationId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ConversationMessages.Add(message);
            await _context.SaveChangesAsync();

            return message.Id;
        }

        public async Task<int> PostMessageToChannelAsync(string newMessageContent, int channelId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false) {
            var content = _messageParsingHelper.Parse(newMessageContent);
            var message = new ChannelMessage(channelId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ChannelMessages.Add(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        private async Task UpdateMessageContent(Message messageInDb, string message) {
            var content = _messageParsingHelper.Parse(message);
            if (content == messageInDb.Content) {
                return;
            }

            messageInDb.UpdateContent(content);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageInConversationAsync(string messageContent, int conversationId, int messageId, int currentUserId) {
            var messageInDb = _context.ConversationMessages.Single(cm => cm.SenderId == currentUserId && cm.ConversationId == conversationId &&
                                                                    cm.Id == messageId);
            await UpdateMessageContent(messageInDb, messageContent);
        }

        public async Task UpdateMessageInChannelAsync(string messageContent, int channelId, int messageId, int currentUserId) {
            var messageInDb = _context.ChannelMessages.Single(cm => cm.SenderId == currentUserId && cm.ChannelId == channelId &&
                                                                    cm.Id == messageId);
            await UpdateMessageContent(messageInDb, messageContent);
        }

        private async Task AddNewFileForMessageAsync(string savedFileName, string fileName, string contentType, int messageId, int userId,
            int workspaceId) {
            var newFile = new Models.File(savedFileName, fileName, contentType, userId, workspaceId);
            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();

            await AttachFileToMessageAsync(messageId, newFile.Id);
        }

        private async Task AttachFileToMessageAsync(int messageId, int fileId) {
            _context.MessageFileAttachments.Add(new MessageFileAttachment(messageId, fileId));
            await _context.SaveChangesAsync();
        }

        private async Task UploadAndSaveFilesForMessageAsync(IList<IFormFile> files, int messageId, int userId, int workspaceId) {
            foreach (var file in files) {
                var savedFileName = await _fileHelper.UploadFileAsync(file, workspaceId);
                await AddNewFileForMessageAsync(savedFileName, file.FileName, file.ContentType, messageId, userId, workspaceId);
            }
        }

        public async Task<int> PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId) {
            if (!files.Any()) {
                throw new ArgumentException($"File list is empty.");
            }

            var messageId = await PostMessageToConversationAsync(string.Empty, conversationId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);

            return messageId;
        }

        public async Task<int> PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId) {
            if (!files.Any()) {
                throw new ArgumentException($"File list is empty.");
            }

            var messageId = await PostMessageToChannelAsync(string.Empty, channelId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);

            return messageId;
        }

        public async Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");
            }

            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            return (await _fileHelper.DownloadFileAsync(file.SavedName, workspaceId), file.ContentType);
        }

        public async Task<int> ShareFileToConversationAsync(int conversationId, int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");

            }

            var messageId = await PostMessageToConversationAsync(string.Empty, conversationId, userId, workspaceId, true);
            await AttachFileToMessageAsync(messageId, fileId);

            return messageId;
        }

        public async Task<int> ShareFileToChannelAsync(int channelId, int fileId, int userId, int workspaceId) {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId))) {
                throw new ArgumentException($"File access required.");

            }

            var messageId = await PostMessageToChannelAsync(string.Empty, channelId, userId, workspaceId, true);
            await AttachFileToMessageAsync(messageId, fileId);

            return messageId;
        }

        public async Task DeleteMessageAsync(int messageId, int userId) {
            var message = await _context.Messages.SingleAsync(m => m.Id == messageId && m.SenderId == userId);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
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

        public async Task AddReactionToMessageAsync(int messageId, string emojiColons, int userId) {
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

        public async Task RemoveReactionToMessageAsync(int messageId, string emojiColons, int userId) {
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
    }
}