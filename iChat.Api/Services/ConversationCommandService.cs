using iChat.Api.Data;
using iChat.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class ConversationCommandService : IConversationCommandService
    {
        private readonly iChatContext _context;
        private readonly IUserQueryService _userQueryService;
        private readonly IConversationQueryService _conversationQueryService;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;

        public ConversationCommandService(iChatContext context, IUserQueryService userQueryService,
            IConversationQueryService conversationQueryService,
            ICacheService cacheService, INotificationService notificationService)
        {
            _context = context;
            _userQueryService = userQueryService;
            _conversationQueryService = conversationQueryService;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<int> StartConversationWithOthersAsync(List<int> withUserIds, int userId, int workspaceId)
        {
            if (withUserIds == null || withUserIds.Count < 1)
            {
                throw new ArgumentException("Invalid users");
            }

            var userIds = withUserIds;
            userIds.Add(userId);

            var conversationId = await StartConversationForUsersAsync(userIds, userId, workspaceId);

            await _cacheService.AddRecentConversationForUserAsync(conversationId, userId, workspaceId);

            return conversationId;
        }

        public async Task InviteOtherMembersToConversationAsync(int conversationId, List<int> userIds, int userId)
        {
            if (userIds == null || userIds.Count < 1)
            {
                throw new ArgumentException("Invalid users");
            }

            if (!_conversationQueryService.IsUserInConversation(conversationId, userId))
            {
                throw new ArgumentException($"User is not in conversation.");
            }

            AddUsersToConversation(userIds, conversationId);
            await _context.SaveChangesAsync();
        }

        // Self conversation will always be on the top, and not cached 
        public async Task<int> StartSelfConversationAsync(int userId, int workspaceId)
        {
            var userIds = new List<int>();
            userIds.Add(userId);
            var conversationId = await StartConversationForUsersAsync(userIds, userId, workspaceId);

            return conversationId;
        }

        private int? GetConversationIdIfExists(List<int> userIds, int workspaceId)
        {
            var conversationUsers = _context.Conversations
                .Where(c => c.WorkspaceId == workspaceId)
                .SelectMany(c => c.ConversationUsers);
            var group = conversationUsers
                .GroupBy(cu => cu.ConversationId)
                .Where(g => g.Select(u => u.UserId).ToHashSet().SetEquals(userIds));

            return !group.Any() ? null : (int?)group.Single().Key;
        }

        private async Task<int> StartConversationForUsersAsync(List<int> userIds, int userId, int workspaceId)
        {
            int conversationId;
            var existingConversationId = GetConversationIdIfExists(userIds, workspaceId);
            if (existingConversationId != null)
            {
                conversationId = existingConversationId.Value;
            }
            else
            {
                var newConversation = new Conversation(userId, workspaceId);
                _context.Conversations.Add(newConversation);
                await _context.SaveChangesAsync();

                AddUsersToConversation(userIds, newConversation.Id);
                await _context.SaveChangesAsync();
                conversationId = newConversation.Id;
            }

            return conversationId;
        }

        private void AddUsersToConversation(List<int> userIds, int conversationId)
        {
            userIds.ForEach(id =>
            {
                var conversationUser = new ConversationUser(conversationId, id);
                _context.ConversationUsers.Add(conversationUser);
            });
        }

        public async Task NotifyTypingAsync(int conversationId, int currentUserId, int workspaceId)
        {
            var currentUser = await _userQueryService.GetUserByIdAsync(currentUserId, workspaceId);
            if (currentUser == null)
            {
                return;
            }

            var userIds = (await _conversationQueryService.GetAllConversationUserIdsAsync(conversationId)).ToList();
            userIds.Remove(currentUserId);

            _notificationService.SendUserTypingNotificationAsync(userIds, currentUser.DisplayName, false, conversationId);
        }
    }
}