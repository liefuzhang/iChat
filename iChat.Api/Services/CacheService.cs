using iChat.Api.Constants;
using iChat.Api.Contract;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetActiveSidebarItemAsync(bool isChannel, int itemId, int userId, int workspaceId)
        {
            var item = new ActiveSidebarItem
            {
                IsChannel = isChannel,
                ItemId = itemId
            };
            var key = GetRedisKeyForActiveSidebarItem(userId, workspaceId);

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(item));
        }

        public async Task<ActiveSidebarItem> GetActiveSidebarItemAsync(int userId, int workspaceId)
        {
            var key = GetRedisKeyForActiveSidebarItem(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ActiveSidebarItem>(value);
        }

        private string GetRedisKeyForActiveSidebarItem(int userId, int workspaceId)
        {
            return $"{iChatConstants.RedisKeyActiveSidebarItemPrefix}-{workspaceId}/{userId}";
        }

        public async Task AddNewUnreadMessageForUsersAsync(int conversationId, IEnumerable<int> userIds, int workspaceId)
        {
            foreach (var userId in userIds.Distinct())
            {
                await AddRecentConversationForUserAsync(conversationId, userId, workspaceId, incrementUnreadMessage: true);
            }
        }

        public async Task AddRecentConversationForUserAsync(int conversationId, int userId,
            int workspaceId, bool incrementUnreadMessage = false)
        {
            ConversationUnreadItem item = null;
            var items = await GetRecentConversationItemsForUserAsync(userId, workspaceId);
            if (items.Any())
            {
                item = items.SingleOrDefault(i => i.ConversationId == conversationId);
                if (item != null)
                {
                    // move to end of list
                    items.Remove(item);
                }
                else if (items.Count >= iChatConstants.RedisRecentConversationMaxNumber)
                {
                    items.RemoveAt(0);
                }
            }
            if (item == null)
            {
                item = new ConversationUnreadItem(conversationId);
            }
            if (incrementUnreadMessage)
            {
                item.IncrementUnreadMessage();
            }
            items.Add(item);

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task ClearUnreadMessageForUserAsync(int conversationId, int userId, int workspaceId)
        {
            var items = await GetRecentConversationItemsForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ConversationId == conversationId);
            if (item == null)
            {
                return;
            }

            item.ClearUnreadMessage();

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task<List<ConversationUnreadItem>> GetRecentConversationItemsForUserAsync(int userId, int workspaceId)
        {
            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return new List<ConversationUnreadItem>();
            }

            return JsonConvert.DeserializeObject<List<ConversationUnreadItem>>(value);
        }

        private static string GetRedisKeyForRecentConversation(int userId, int workspaceId)
        {
            return $"{iChatConstants.RedisKeyRecentConversationPrefix}-{workspaceId}/{userId}";
        }

        public async Task AddUnreadChannelForUsersAsync(int channelId, IEnumerable<int> userIds, int workspaceId,
            List<int> mentionUserIds)
        {
            foreach (var userId in userIds.Distinct())
            {
                var items = await GetUnreadChannelForUserAsync(userId, workspaceId);
                var item = items.SingleOrDefault(i => i.ChannelId == channelId);
                if (item == null)
                {
                    item = new ChannelUnreadItem(channelId);
                    items.Add(item);
                }

                if (mentionUserIds.Contains(userId))
                {
                    item.IncrementUnreadMention();
                }

                var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
            }
        }

        public async Task RemoveUnreadChannelForUserAsync(int channelId, int userId, int workspaceId)
        {
            var items = await GetUnreadChannelForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ChannelId == channelId);
            if (item == null)
            {
                return;
            }

            items.Remove(item);
            var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task<List<ChannelUnreadItem>> GetUnreadChannelForUserAsync(int userId, int workspaceId)
        {
            var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return new List<ChannelUnreadItem>();
            }

            return JsonConvert.DeserializeObject<List<ChannelUnreadItem>>(value);
        }

        private string GetRedisKeyForUnreadChannel(int userId, int workspaceId)
        {
            return $"{iChatConstants.RedisKeyRecentUnreadChannelPrefix}-{workspaceId}/{userId}";
        }
    }
}