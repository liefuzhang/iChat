﻿using AutoMapper;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MessageDto = iChat.Api.Dtos.MessageDto;

namespace iChat.Api.Services {
    public class MessageQueryService : IMessageQueryService {
        private readonly iChatContext _context;
        private readonly IChannelQueryService _channelQueryService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IMapper _mapper;

        public MessageQueryService(iChatContext context, IChannelQueryService channelQueryService,
            IConversationQueryService conversationQueryService,
            IUserQueryService userQueryService, IMapper mapper) {
            _context = context;
            _channelQueryService = channelQueryService;
            _conversationQueryService = conversationQueryService;
            _userQueryService = userQueryService;
            _mapper = mapper;
        }

        private void AllowConsecutiveMessages(List<MessageGroupDto> groups) {
            var maxDiffInMin = 3;
            groups.ForEach(g => {
                var messages = g.Messages.ToList();
                for (var i = 0; i < messages.Count(); i++) {
                    if (i == 0 || messages[i - 1].SenderId != messages[i].SenderId ||
                    messages[i - 1].IsSystemMessage) {
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
            var channel = await _channelQueryService.GetChannelByIdAsync(channelId, workspaceId);
            var createdByUser = await _userQueryService.GetUserByIdAsync(channel.CreatedByUserId);
            var users = await _channelQueryService.GetAllChannelUsersAsync(channelId);
            messageLoad.MessageChannelDescriptionDto = new MessageChannelDescriptionDto {
                CreatedByUser = _mapper.Map<UserDto>(createdByUser),
                CreatedDateString = channel.CreatedDateString,
                MessageChannelName = channel.Name,
                UserList = users
            };
        }

        public async Task<MessageLoadDto> GetMessagesForChannelAsync(int channelId, int userId, int workspaceId, int currentPage) {
            if (!_channelQueryService.IsUserSubscribedToChannel(channelId, userId)) {
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
            var conversation = await _conversationQueryService.GetConversationByIdAsync(conversationId, userId, workspaceId);
            var createdByUser = await _userQueryService.GetUserByIdAsync(conversation.CreatedByUserId);
            var users = await _conversationQueryService.GetAllConversationUsersAsync(conversationId);
            messageLoad.MessageChannelDescriptionDto = new MessageChannelDescriptionDto {
                CreatedByUser = _mapper.Map<UserDto>(createdByUser),
                CreatedDateString = conversation.CreatedDateString,
                MessageChannelName = conversation.Name,
                UserList = users
            };
        }

        public async Task<MessageLoadDto> GetMessagesForConversationAsync(int conversationId, int userId, int workspaceId, int currentPage) {
            if (!_conversationQueryService.IsUserInConversation(conversationId, userId)) {
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
            if (!_channelQueryService.IsUserSubscribedToChannel(channelId, userId)) {
                throw new ArgumentException($"User is not subsribed to channel.");
            }

            var message = await _context.ChannelMessages
                .Include(m => m.Sender)
                .Include(m => m.MessageReactions)
                .SingleAsync(m => m.Id == messageId && m.ChannelId == channelId);

            return await GetMessageGroupForSingleMessageAsync(message);
        }

        public async Task<MessageGroupDto> GetSingleMessagesForConversationAsync(int conversationId, int messageId, int userId) {
            if (!_conversationQueryService.IsUserInConversation(conversationId, userId)) {
                throw new ArgumentException($"User is not in conversation.");
            }

            var message = await _context.ConversationMessages
                .Include(m => m.Sender)
                .Include(m => m.MessageReactions)
                .SingleAsync(m => m.Id == messageId && m.ConversationId == conversationId);

            return await GetMessageGroupForSingleMessageAsync(message);
        }
    }
}