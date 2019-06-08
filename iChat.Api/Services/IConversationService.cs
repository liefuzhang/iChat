using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services {
    public interface IConversationService
    {
        Task<int> StartConversationWithOthersAsync(List<int> withUserIds, int userId, int workspaceId);
        Task<int> StartSelfConversationAsync(int userId, int workspaceId);
        Task<ConversationDto> GetConversationByIdAsync(int id, int userId, int workspaceId);
        Task<IEnumerable<ConversationDto>> GetConversationsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId);
        Task<bool> IsSelfConversationAsync(int conversationId, int userId);
    }
}