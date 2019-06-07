using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class ConversationService : IConversationService {
        private readonly iChatContext _context;
        private readonly IUserService _userService;
        private ICacheService _cacheService;

        public ConversationService(iChatContext context, IUserService userService,
            IDistributedCache cache, ICacheService cacheService) {
            _context = context;
            _userService = userService;
            _cacheService = cacheService;
        }

        public async Task<int> StartConversationAsync(List<int> withUserIds, int userId, int workspaceId) {
            if (withUserIds == null || withUserIds.Count < 1) {
                throw new ArgumentException("Invalid users");
            }

            var userIds = withUserIds;
            userIds.Add(userId);

            int conversationId;
            var existingConversationId = GetConversationIdIfExists(userIds, workspaceId);
            if (existingConversationId != null) {
                conversationId = existingConversationId.Value;
            } else {
                var userDisplayNames = await _context.Users.Where(u => userIds.Contains(u.Id))
                    .OrderBy(u => u.Id).Select(u => u.DisplayName).ToListAsync();
                var conversationName = string.Join(", ", userDisplayNames);
                var newConversation = new Conversation(conversationName, workspaceId);

                _context.Conversations.Add(newConversation);
                await _context.SaveChangesAsync();

                AddUsersToConversation(userIds, newConversation.Id);
                await _context.SaveChangesAsync();
                conversationId = newConversation.Id;
            }

            await _cacheService.AddRecentConversationIdForUserAsync(conversationId, userId, workspaceId);

            return conversationId;
        }

        private int? GetConversationIdIfExists(List<int> userIds, int workspaceId) {
            var conversationUsers = _context.Conversations
                .Where(c => c.WorkspaceId == workspaceId)
                .SelectMany(c => c.ConversationUsers);
            var group = conversationUsers
                .GroupBy(cu => cu.ConversationId)
                .Where(g => g.Select(u => u.UserId).ToHashSet().SetEquals(userIds));

            return group.Count() == 0 ? null : (int?)group.Single().Key;
        }

        private void AddUsersToConversation(List<int> userIds, int conversationId) {
            userIds.ForEach(id => {
                var conversationUser = new ConversationUser(conversationId, id);
                _context.ConversationUsers.Add(conversationUser);
            });
        }

        public async Task<Conversation> GetConversationByIdAsync(int id, int workspaceId) {
            var conversation = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                            c.Id == id)
                .SingleOrDefaultAsync();

            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId, int workspaceId) {
            var recentConversationIds = await _cacheService.GetRecentConversationIdsForUserAsync(userId, workspaceId);
            var conversations = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    recentConversationIds.Contains(c.Id))
                .ToListAsync();

            return conversations;
        }

        public async Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId) {
            return await _context.ConversationUsers
                .Where(cu => cu.ConversationId == conversationId)
                .Select(cu => cu.UserId)
                .ToListAsync();
        }
    }
}