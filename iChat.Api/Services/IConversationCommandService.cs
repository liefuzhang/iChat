using iChat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Dtos;

namespace iChat.Api.Services
{
    public interface IConversationCommandService
    {
        Task<int> StartConversationWithOthersAsync(List<int> withUserIds, int userId, int workspaceId);
        Task InviteOtherMembersToConversationAsync(int conversationId, List<int> userIds, int invitedByUserId,
            int workspaceId);
        Task<int> StartSelfConversationAsync(int userId, int workspaceId);
        Task NotifyTypingAsync(int conversationId, int currentUserId, int workspaceId);
    }
}