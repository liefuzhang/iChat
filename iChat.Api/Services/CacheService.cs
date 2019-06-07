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

        public async Task AddRecentConversationIdForUserAsync(int conversationId, int userId, int workspaceId) {
            var ids = await GetRecentConversationIdsForUserAsync(userId, workspaceId);
            if (ids == null) {
                ids = new List<int>(new[] { conversationId });
            } else {
                if (ids.Contains(conversationId)) {
                    // move to end of list
                    ids.Remove(conversationId);
                } else if (ids.Count >= iChatConstants.RedisRecentConversationMaxNumber) {
                    ids.RemoveAt(0);
                }
                ids.Add(conversationId);
            }

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(ids));
        }

        public async Task<List<int>> GetRecentConversationIdsForUserAsync(int userId, int workspaceId) {
            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return new List<int>();

            return JsonConvert.DeserializeObject<List<int>>(value);
        }

        private string GetRedisKeyForRecentConversation(int userId, int workspaceId) {
            return $"{iChatConstants.RedisKeyRecentConversationPrefix}-{workspaceId}/{userId}";
        }
    }
}