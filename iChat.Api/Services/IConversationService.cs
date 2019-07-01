using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Dtos;

namespace iChat.Api.Services
{
    public interface IConversationService
    {
        Task<int> StartConversationWithOthersAsync(List<int> withUserIds, int userId, int workspaceId);
        Task InviteOtherMembersToConversationAsync(int conversationId, List<int> userIds, int userId);
        Task<int> StartSelfConversationAsync(int userId, int workspaceId);
        bool IsUserInConversation(int id, int userId);
        Task<ConversationDto> GetConversationByIdAsync(int id, int userId, int workspaceId);
        Task<IEnumerable<ConversationDto>> GetRecentConversationsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId);
        Task<IEnumerable<UserDto>> GetAllConversationUsersAsync(int conversationId);
        Task<bool> IsSelfConversationAsync(int conversationId, int userId);
        Task NotifyTypingAsync(int conversationId, int currentUserId, int workspaceId);
    }
}