using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Dtos;

namespace iChat.Api.Services {
    public class ConversationService : IConversationService {
        private readonly iChatContext _context;
        private readonly IUserService _userService;
        private ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ConversationService(iChatContext context, IUserService userService,
            ICacheService cacheService, IMapper mapper, INotificationService notificationService) {
            _context = context;
            _userService = userService;
            _cacheService = cacheService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<int> StartConversationWithOthersAsync(List<int> withUserIds, int userId, int workspaceId) {
            if (withUserIds == null || withUserIds.Count < 1) {
                throw new ArgumentException("Invalid users");
            }

            var userIds = withUserIds;
            userIds.Add(userId);

            var conversationId = await StartConversationForUsersAsync(userIds, userId, workspaceId);

            await _cacheService.AddRecentConversationForUserAsync(conversationId, userId, workspaceId);

            return conversationId;
        }

        public async Task InviteOtherMembersToConversationAsync(int conversationId, List<int> userIds, int userId) {
            if (userIds == null || userIds.Count < 1) {
                throw new ArgumentException("Invalid users");
            }

            if (!IsUserInConversation(conversationId, userId)) {
                throw new ArgumentException($"User is not in conversation.");
            }

            AddUsersToConversation(userIds, conversationId);
            await _context.SaveChangesAsync();
        }

        // Self conversation will always be on the top, and not cached 
        public async Task<int> StartSelfConversationAsync(int userId, int workspaceId) {
            var userIds = new List<int>();
            userIds.Add(userId);
            var conversationId = await StartConversationForUsersAsync(userIds, userId, workspaceId);

            return conversationId;
        }

        private async Task<int> StartConversationForUsersAsync(List<int> userIds, int userId, int workspaceId) {
            int conversationId;
            var existingConversationId = GetConversationIdIfExists(userIds, workspaceId);
            if (existingConversationId != null) {
                conversationId = existingConversationId.Value;
            } else {
                var newConversation = new Conversation(userId, workspaceId);
                _context.Conversations.Add(newConversation);
                await _context.SaveChangesAsync();

                AddUsersToConversation(userIds, newConversation.Id);
                await _context.SaveChangesAsync();
                conversationId = newConversation.Id;
            }

            return conversationId;
        }

        private int? GetConversationIdIfExists(List<int> userIds, int workspaceId) {
            var conversationUsers = _context.Conversations
                .Where(c => c.WorkspaceId == workspaceId)
                .SelectMany(c => c.ConversationUsers);
            var group = conversationUsers
                .GroupBy(cu => cu.ConversationId)
                .Where(g => g.Select(u => u.UserId).ToHashSet().SetEquals(userIds));

            return !group.Any() ? null : (int?)group.Single().Key;
        }

        private void AddUsersToConversation(List<int> userIds, int conversationId) {
            userIds.ForEach(id => {
                var conversationUser = new ConversationUser(conversationId, id);
                _context.ConversationUsers.Add(conversationUser);
            });
        }

        private async Task<string> GetConversationNameAsync(int conversationId, int currentUserId, int workspaceId) {
            if (await IsSelfConversationAsync(conversationId, currentUserId)) {
                var selfConversationSuffix = " (you)";
                var userDisplayName = (await _userService.GetUserByIdAsync(currentUserId, workspaceId))?.DisplayName;
                return userDisplayName + selfConversationSuffix;
            }

            var userIds = (await GetAllConversationUserIdsAsync(conversationId)).ToList();
            userIds.Remove(currentUserId);
            var userDisplayNames = await _context.Users.Where(u => userIds.Contains(u.Id))
                    .OrderBy(u => u.Id).Select(u => u.DisplayName).ToListAsync();
            var conversationName = string.Join(", ", userDisplayNames);
            return conversationName;
        }

        public async Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId)
        {
            return await _context.ConversationUsers
                .Where(cu => cu.ConversationId == conversationId)
                .Select(cu => cu.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllConversationUsersAsync(int conversationId)
        {
            var userIds = await GetAllConversationUserIdsAsync(conversationId);
            var users = await _context.Users.Where(u => userIds.Contains(u.Id))
                .Select(u => _mapper.Map<UserDto>(u)).ToListAsync();
            return users;
        }

        public async Task<bool> IsSelfConversationAsync(int conversationId, int userId) {
            var userIds = await GetAllConversationUserIdsAsync(conversationId);
            return userIds.Count() == 1 && userIds.Single() == userId;
        }

        public async Task NotifyTypingAsync(int conversationId, int currentUserId, int workspaceId) {
            var currentUser = await _userService.GetUserByIdAsync(currentUserId, workspaceId);
            if (currentUser == null) {
                return;
            }

            var userIds = (await GetAllConversationUserIdsAsync(conversationId)).ToList();
            userIds.Remove(currentUserId);

            _notificationService.SendUserTypingNotificationAsync(userIds, currentUser.DisplayName, false, conversationId);
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