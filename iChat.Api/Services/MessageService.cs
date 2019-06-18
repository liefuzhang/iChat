using AutoMapper;
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

namespace iChat.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly iChatContext _context;
        private readonly IChannelService _channelService;
        private readonly IMessageParsingHelper _messageParsingHelper;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        public MessageService(iChatContext context, IChannelService channelService,
            IMessageParsingHelper messageParsingHelper, IMapper mapper, IFileHelper fileHelper)
        {
            _context = context;
            _channelService = channelService;
            _messageParsingHelper = messageParsingHelper;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }

        private async Task<List<MessageGroupDto>> GetMessageGroups(IQueryable<Message> baseQuery)
        {
            var groups = await baseQuery.GroupBy(cm => cm.CreatedDate.Date)
                .OrderBy(group => group.Key)
                .Select(group =>
                    new MessageGroupDto
                    {
                        DateString = group.Key.ToString("dddd, MMM d", CultureInfo.InvariantCulture),
                        Messages = group.Select(m => _mapper.Map<MessageDto>(m))
                    })
                .ToListAsync();

            AllowConsecutiveMessages(groups);
            await AddFilesToMessagesAsync(groups);

            return groups;
        }

        private void AllowConsecutiveMessages(List<MessageGroupDto> groups)
        {
            var maxDiffInMin = 3;
            groups.ForEach(g =>
            {
                var messages = g.Messages.ToList();
                for (var i = 0; i < messages.Count(); i++)
                {
                    if (i == 0 || messages[i - 1].SenderId != messages[i].SenderId)
                    {
                        continue;
                    }
                    var time = DateTime.Parse(messages[i].TimeString);
                    var prevTime = DateTime.Parse(messages[i - 1].TimeString);
                    if ((time - prevTime).Minutes <= maxDiffInMin)
                    {
                        messages[i].IsConsecutiveMessage = true;
                    }
                }
                g.Messages = messages;
            });
        }

        private async Task AddFilesToMessagesAsync(List<MessageGroupDto> groups)
        {
            var fileMessages = groups.Select(g => g.Messages).SelectMany(m => m).Where(m => m.HasFileAttachments).ToList();
            var fileMessageIds = fileMessages.Select(m => m.Id);
            var files = await _context.Files.Include(f => f.MessageFileAttachments)
                .Where(f => f.MessageFileAttachments
                .Any(mfa => fileMessageIds.Contains(mfa.MessageId))).ToListAsync();
            foreach (var fileMessage in fileMessages)
            {
                var fileAttachments = files.Where(f => f.MessageFileAttachments.Any(mfa => mfa.MessageId == fileMessage.Id)).ToList();
                fileMessage.FileAttachments = _mapper.Map<List<FileDto>>(fileAttachments);
            }
        }

        public async Task<IEnumerable<MessageGroupDto>> GetMessagesForChannelAsync(int channelId, int workspaceId)
        {
            var messageGroupsBaseQuery = _context.ChannelMessages
                .Include(m => m.Sender)
                .Where(m => m.ChannelId == channelId && m.WorkspaceId == workspaceId);
            var messageGroups = await GetMessageGroups(messageGroupsBaseQuery);

            return messageGroups;
        }

        public async Task<IEnumerable<MessageGroupDto>> GetMessagesForConversationAsync(int conversationId, int workspaceId)
        {
            var messageGroupsBaseQuery = _context.ConversationMessages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId && m.WorkspaceId == workspaceId);
            var messageGroups = await GetMessageGroups(messageGroupsBaseQuery);

            return messageGroups;
        }

        public async Task<int> PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false)
        {
            var content = _messageParsingHelper.Parse(newMessage);
            var message = new ConversationMessage(conversationId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ConversationMessages.Add(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        public async Task<int> PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId,
            int workspaceId, bool hasFileAttachments = false)
        {
            var content = _messageParsingHelper.Parse(newMessage);
            var message = new ChannelMessage(channelId, content, currentUserId, workspaceId, hasFileAttachments);

            _context.ChannelMessages.Add(message);
            await _context.SaveChangesAsync();
            return message.Id;
        }

        private async Task AddNewFileForMessageAsync(string savedFileName, string fileName, string contentType, int messageId, int userId,
            int workspaceId)
        {
            var newFile = new Models.File(savedFileName, fileName, contentType, userId, workspaceId);
            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();

            _context.MessageFileAttachments.Add(new MessageFileAttachment(messageId, newFile.Id));
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

        public async Task PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId)
        {
            if (!files.Any())
            {
                return;
            }

            var messageId = await PostMessageToConversationAsync(string.Empty, conversationId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);
        }


        public async Task PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId)
        {
            if (!files.Any())
            {
                return;
            }

            var messageId = await PostMessageToChannelAsync(string.Empty, channelId, userId, workspaceId, true);
            await UploadAndSaveFilesForMessageAsync(files, messageId, userId, workspaceId);
        }

        public async Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId)
        {
            if (!(await EligibleForTheFileAsync(fileId, userId, workspaceId)))
            {
                return (null, null);
            }

            var file = await _context.Files.SingleAsync(f => f.Id == fileId);
            return (await _fileHelper.DownloadFileAsync(file.SavedName, workspaceId), file.ContentType);
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
    }
}