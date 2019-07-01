using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Dtos;

namespace iChat.Api.Services
{
    public interface IConversationQueryService
    {
        bool IsUserInConversation(int id, int userId);
        Task<ConversationDto> GetConversationByIdAsync(int id, int userId, int workspaceId);
        Task<IEnumerable<ConversationDto>> GetRecentConversationsForUserAsync(int userId, int workspaceId);
        Task<IEnumerable<int>> GetAllConversationUserIdsAsync(int conversationId);
        Task<IEnumerable<UserDto>> GetAllConversationUsersAsync(int conversationId);
        Task<bool> IsSelfConversationAsync(int conversationId, int userId);
    }
}