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
        private readonly IDistributedCache _cache;

        public ConversationService(iChatContext context, IUserService userService, IDistributedCache cache) {
            _context = context;
            _userService = userService;
            _cache = cache;
        }

        public async Task<int> StartConversationAsync(List<int> userIds, int workspaceId) {
            if (userIds == null || userIds.Count < 2) {
                throw new ArgumentException("Invalid users");
            }

            var conversationId = GetConversationIdIfExists(userIds, workspaceId);
            if (conversationId != null)
                return conversationId.Value;

            var userDisplayNames = await _context.Users.Where(u => userIds.Contains(u.Id))
                .OrderBy(u => u.Id).Select(u => u.DisplayName).ToListAsync();
            var conversationName = string.Join(", ", userDisplayNames);
            var newConversation = new Conversation(conversationName, workspaceId);

            _context.Conversations.Add(newConversation);
            await _context.SaveChangesAsync();

            AddUsersToConversation(userIds, newConversation.Id);
            await _context.SaveChangesAsync();

            return newConversation.Id;
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
            var conversations = await _context.Conversations.AsNoTracking()
                .Where(c => c.WorkspaceId == workspaceId &&
                    c.ConversationUsers.Any(cu => cu.UserId == userId &&
                        cu.ConversationId == c.Id))
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