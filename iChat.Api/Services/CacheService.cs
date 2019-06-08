using iChat.Api.Models;
using iChat.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using iChat.Api.Constants;
using iChat.Api.Contract;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iChat.Api.Services {
    public class CacheService : ICacheService {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache) {
            _cache = cache;
        }

        public async Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int userId, int workspaceId) {
            var item = new ActiveSidebarItem {
                IsChannel = isChannel,
                ItemId = itemId
            };
            var key = GetRedisKeyForActiveSidebarItem(userId, workspaceId);

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(item));
        }

        public async Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int userId, int workspaceId) {
            var key = GetRedisKeyForActiveSidebarItem(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return null;

            return JsonConvert.DeserializeObject<ActiveSidebarItem>(value);
        }

        private string GetRedisKeyForActiveSidebarItem(int userId, int workspaceId) {
            return $"{iChatConstants.RedisKeyActiveSidebarItemPrefix}-{workspaceId}/{userId}";
        }

        public async Task AddRecentConversationIdForUsersAsync(int conversationId, IEnumerable<int> userIds, int workspaceId) {
            foreach (var userId in userIds) {
                await AddRecentConversationIdForUserAsync(conversationId, userId, workspaceId);
            }
        }

        public async Task AddRecentConversationIdForUserAsync(int conversationId, int userId, int workspaceId) {
            var items = await GetRecentConversationItemsForUserAsync(userId, workspaceId);
            if (items == null) {
                items = new List<ConversationItem>(new[] { new ConversationItem(conversationId) });
            } else {
                var item = items.SingleOrDefault(i => i.ConversationId == conversationId);
                if (item != null) {
                    // move to end of list
                    items.Remove(item);
                } else if (items.Count >= iChatConstants.RedisRecentConversationMaxNumber) {
                    items.RemoveAt(0);
                }
                items.Add(new ConversationItem(conversationId));
            }

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task<List<ConversationItem>> GetRecentConversationItemsForUserAsync(int userId, int workspaceId) {
            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return new List<ConversationItem>();

            return JsonConvert.DeserializeObject<List<ConversationItem>>(value);
        }

        private string GetRedisKeyForRecentConversation(int userId, int workspaceId) {
            return $"{iChatConstants.RedisKeyRecentConversationPrefix}-{workspaceId}/{userId}";
        }
    }
}