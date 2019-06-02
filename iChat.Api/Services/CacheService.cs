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

namespace iChat.Api.Services {
    public class CacheService : ICacheService {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache) {
            _cache = cache;
        }

        public async Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int workspaceId, int userId) {
            var item = new ActiveSidebarItem {
                IsChannel = isChannel,
                ItemId = itemId
            };
            var key = GetRedisKeyForActiveSidebarItem(workspaceId, userId);

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(item));
        }

        public async Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int workspaceId, int userId) {
            var key = GetRedisKeyForActiveSidebarItem(workspaceId, userId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return null;

            return JsonConvert.DeserializeObject<ActiveSidebarItem>(value);
        }

        private string GetRedisKeyForActiveSidebarItem(int workspaceId, int userId) {
            return $"{iChatConstants.RedisKeyActiveSidebarItemPrefix}-{workspaceId}/{userId}";
        }
    }
}