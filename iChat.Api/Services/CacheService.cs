using iChat.Api.Constants;
using iChat.Api.Contract;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
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

        public async Task AddUnreadConversationMessageForUsersAsync(int conversationId, int messageId,
            IEnumerable<int> userIds,
            int workspaceId)
        {
            foreach (var userId in userIds.Distinct())
            {
                await AddRecentConversationForUserAsync(conversationId, userId, workspaceId, addUnreadMessage: true, messageId: messageId);
            }
        }

        public async Task AddRecentConversationForUserAsync(int conversationId, int userId,
            int workspaceId, bool addUnreadMessage = false, int messageId = 0)
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
            if (addUnreadMessage)
            {
                item.AddUnreadMessageId(messageId);
            }
            items.Add(item);

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task ClearAllUnreadConversationMessageIdsForUserAsync(int conversationId, int userId, int workspaceId)
        {
            var items = await GetRecentConversationItemsForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ConversationId == conversationId);
            if (item == null)
            {
                return;
            }

            item.ClearAllUnreadMessageIds();

            var key = GetRedisKeyForRecentConversation(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task ClearUnreadConversationMessageIdForUserAsync(int conversationId, int messageId, int userId, int workspaceId)
        {
            var items = await GetRecentConversationItemsForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ConversationId == conversationId);
            if (item == null)
            {
                return;
            }

            item.ClearUnreadMessageId(messageId);

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

        public async Task AddUnreadChannelMessageForUsersAsync(int channelId, int messageId,
            IEnumerable<int> userIds, int workspaceId, List<int> mentionUserIds = null)
        {
            foreach (var userId in userIds.Distinct())
            {
                var items = await GetUnreadChannelsForUserAsync(userId, workspaceId);
                var item = items.SingleOrDefault(i => i.ChannelId == channelId);
                if (item == null)
                {
                    item = new ChannelUnreadItem(channelId);
                    items.Add(item);
                }

                if (mentionUserIds != null && mentionUserIds.Contains(userId))
                {
                    item.AddUnreadMessage(messageId, true);
                }
                else
                {
                    item.AddUnreadMessage(messageId, false);
                }

                var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
            }
        }

        public async Task UpdateUnreadChannelMessageMentionForUsersAsync(int channelId, int messageId,
            IEnumerable<int> userIds, int workspaceId, List<int> mentionUserIds)
        {
            foreach (var userId in userIds.Distinct())
            {
                var items = await GetUnreadChannelsForUserAsync(userId, workspaceId);
                var item = items.SingleOrDefault(i => i.ChannelId == channelId);
                if (item == null)
                {
                    return;
                }

                if (mentionUserIds != null && mentionUserIds.Contains(userId))
                {
                    item.UpdateUnreadMessageMention(messageId, true);
                }
                else
                {
                    item.UpdateUnreadMessageMention(messageId, false);
                }

                var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
            }
        }

        public async Task ClearUnreadChannelForUserAsync(int channelId, int userId, int workspaceId)
        {
            var items = await GetUnreadChannelsForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ChannelId == channelId);
            if (item == null)
            {
                return;
            }

            items.Remove(item);
            var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task ClearUnreadChannelMessageForUserAsync(int channelId, int messageId, int userId, int workspaceId)
        {
            var items = await GetUnreadChannelsForUserAsync(userId, workspaceId);
            var item = items.SingleOrDefault(i => i.ChannelId == channelId);
            if (item == null)
            {
                return;
            }

            item.ClearUnreadMessageId(messageId);
            if (item.UnreadMessageCount == 0)
            {
                items.Remove(item);
            }

            var key = GetRedisKeyForUnreadChannel(userId, workspaceId);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(items));
        }

        public async Task<List<ChannelUnreadItem>> GetUnreadChannelsForUserAsync(int userId, int workspaceId)
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

        public async Task SetUserOnlineAsync(int userId, int workspaceId)
        {
            const int expirySeconds = 30 * 60;
            var key = GetRedisKeyForUserOnlineItem(userId, workspaceId);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expirySeconds));

            await _cache.SetAsync(key, new byte[1], options);
        }

        public async Task<bool> GetUserOnlineAsync(int userId, int workspaceId)
        {
            var key = GetRedisKeyForUserOnlineItem(userId, workspaceId);
            var value = await _cache.GetAsync(key);

            return value != null;
        }

        private string GetRedisKeyForUserOnlineItem(int userId, int workspaceId)
        {
            return $"{iChatConstants.RedisKeyUserOnlinePrefix}-{workspaceId}/{userId}";
        }
    }
}