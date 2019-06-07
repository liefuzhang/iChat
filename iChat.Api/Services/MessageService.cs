﻿using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public MessageService(iChatContext context, IChannelService channelService,
            IMessageParsingHelper messageParsingHelper, IMapper mapper)
        {
            _context = context;
            _channelService = channelService;
            _messageParsingHelper = messageParsingHelper;
            _mapper = mapper;
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
                    if (i == 0)
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

        public async Task PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId, int workspaceId)
        {
            var content = _messageParsingHelper.Parse(newMessage);
            var message = new ConversationMessage(conversationId, content, currentUserId, workspaceId);

            _context.ConversationMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId)
        {
            var content = _messageParsingHelper.Parse(newMessage);
            var message = new ChannelMessage(channelId, content, currentUserId, workspaceId);

            _context.ChannelMessages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}