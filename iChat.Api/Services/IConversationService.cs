using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IConversationService
    {
        Task<int> StartConversationAsync(List<int> userIds, int workspaceId);
        Task<Conversation> GetConversationByIdAsync(int id, int workspaceId);
        Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId);
    }
}