using iChat.Api.Constants;
using iChat.Api.Dtos;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly iChatContext _context;
        private readonly IChannelService _channelService;
        private readonly IMessageParsingService _messageParsingService;

        public MessageService(iChatContext context, IChannelService channelService, IMessageParsingService messageParsingService)
        {
            _context = context;
            _channelService = channelService;
            _messageParsingService = messageParsingService;
        }

        public async Task<IEnumerable<MessageGroupDto>> GetMessagesForChannelAsync(int channelId, int workspaceId)
        {
            if (channelId == iChatConstants.DefaultChannelIdInRequest)
            {
                channelId = await _channelService.GetDefaultChannelGeneralIdAsync(workspaceId);
            }

            var messageGroups = await _context.ChannelMessages
                .Include(m => m.Sender)
                .Where(m => m.ChannelId == channelId && m.WorkspaceId == workspaceId)
                .GroupBy(cm => cm.CreatedDate.Date)
                .OrderBy(group => group.Key)
                .Select(group =>
                    new MessageGroupDto
                    {
                        DateString = group.Key.ToString("dddd, MMM d"),
                        Messages = group.Select(m => m.MapToMessageDto())
                    })
                .ToListAsync();

            return messageGroups;
        }

        public async Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId, int currentUserId, int workspaceId)
        {
            var messages = await _context.DirectMessages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId &&
                            m.SenderId == currentUserId &&
                            m.WorkspaceId == workspaceId ||
                            m.ReceiverId == currentUserId &&
                            m.SenderId == userId &&
                            m.WorkspaceId == workspaceId)
                .OrderBy(m => m.CreatedDate)
                .Cast<Message>()
                .ToListAsync();
            return messages;
        }

        public async Task PostMessageToUserAsync(string newMessage, int userId, int currentUserId, int workspaceId)
        {
            if (userId < 1)
            {
                throw new ArgumentException("invalid user id");
            }

            var message = new DirectMessage
            {
                ReceiverId = userId,
                Content = _messageParsingService.Parse(newMessage),
                CreatedDate = DateTime.Now,
                SenderId = currentUserId,
                WorkspaceId = workspaceId
            };

            _context.DirectMessages.Add(message);

            await _context.SaveChangesAsync();
        }

        public async Task PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId)
        {
            if (channelId < 1)
            {
                throw new ArgumentException("invalid channel id");
            }

            var message = new ChannelMessage
            {
                ChannelId = channelId,
                Content = _messageParsingService.Parse(newMessage),
                CreatedDate = DateTime.Now,
                SenderId = currentUserId,
                WorkspaceId = workspaceId
            };

            _context.ChannelMessages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}