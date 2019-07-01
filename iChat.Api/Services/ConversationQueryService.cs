﻿using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class ConversationQueryService : IConversationQueryService {
        private readonly iChatContext _context;
        private readonly IUserQueryService _userQueryService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public ConversationQueryService(iChatContext context, IUserQueryService userQueryService,
            ICacheService cacheService, IMapper mapper) {
            _context = context;
            _userQueryService = userQueryService;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        private async Task<string> GetConversationNameAsync(int conversationId, int currentUserId, int workspaceId) {
            if (await IsSelfConversationAsync(conversationId, currentUserId)) {
                var selfConversationSuffix = " (you)";
                var userDisplayName = await _userQueryService.GetUserNamesAsync(new List<int> { currentUserId }, workspaceId);
                return userDisplayName + selfConversationSuffix;
            }

            var userIds = (await GetAllConversationUserIdsAsync(conversationId)).ToList();
            userIds.Remove(currentUserId);
            var conversationName = await _userQueryService.GetUserNamesAsync(userIds, workspaceId);
            return conversationName;
        }

        public async Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId) {
            return await _context.ConversationUsers
                .Where(cu => cu.ConversationId == conversationId)
                .Select(cu => cu.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllConversationUsersAsync(int conversationId) {
            var userIds = await GetAllConversationUserIdsAsync(conversationId);
            var users = await _context.Users.Where(u => userIds.Contains(u.Id))
                .Select(u => _mapper.Map<UserDto>(u)).ToListAsync();
            return users;
        }

        public async Task<bool> IsSelfConversationAsync(int conversationId, int userId) {
            var userIds = await GetAllConversationUserIdsAsync(conversationId);
            return userIds.Count() == 1 && userIds.Single() == userId;
        }

        public bool IsUserInConversation(int id, int userId) {
            return _context.ConversationUsers.Any(cu => cu.UserId == userId &&
                       cu.ConversationId == id);
        }

        public async Task<ConversationDto> GetConversationByIdAsync(int id, int userId, int workspaceId) {
            var conversation = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                            c.Id == id)
                .SingleOrDefaultAsync();

            var conversationDto = _mapper.Map<ConversationDto>(conversation);
            conversationDto.Name = await GetConversationNameAsync(id, userId, workspaceId);
            return conversationDto;
        }

        public async Task<IEnumerable<ConversationDto>> GetRecentConversationsForUserAsync(int userId, int workspaceId) {
            var recentConversationItems = await _cacheService.GetRecentConversationItemsForUserAsync(userId, workspaceId);
            var recentConversationIds = recentConversationItems.Select(i => i.ConversationId);
            var conversations = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    recentConversationIds.Contains(c.Id))
                .ToListAsync();

            await AddSelfConversationToBeginningAsync(userId, workspaceId, conversations);

            var conversationDtos = conversations.Select(async c => {
                var dto = _mapper.Map<ConversationDto>(c);
                dto.Name = await GetConversationNameAsync(c.Id, userId, workspaceId);
                dto.UnreadMessageCount = recentConversationItems.SingleOrDefault(i => i.ConversationId == c.Id)?.UnreadMessageCount ?? 0;
                return dto;
            });

            return await Task.WhenAll(conversationDtos);
        }

        private async Task AddSelfConversationToBeginningAsync(int userId, int workspaceId, List<Conversation> conversations) {
            var conversation = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ConversationUsers.Count() == 1 &&
                    c.ConversationUsers.Any(cu => cu.UserId == userId))
                .SingleOrDefaultAsync();

            if (conversation == null) {
                return;
            }

            conversations.Insert(0, conversation);
        }
    }
}