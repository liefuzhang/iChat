using iChat.Api.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IMessageCommandService
    {
        Task<int> PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId, int workspaceId, bool hasFileAttachments = false);
        Task<int> PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId, bool hasFileAttachments = false);
        Task UpdateMessageInConversationAsync(string message, int conversationId, int messageId, int currentUserId);
        Task UpdateMessageInChannelAsync(string message, int channelId, int messageId, int currentUserId);
        Task<int> PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId);
        Task<int> PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId);
        Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId);
        Task<int> ShareFileToConversationAsync(int conversationId, int fileId, int userId, int workspaceId);
        Task<int> ShareFileToChannelAsync(int channelId, int fileId, int userId, int workspaceId);
        Task DeleteMessageAsync(int messageId, int userId);
        Task AddReactionToMessageAsync(int messageId, string emojiColons, int userId);
        Task RemoveReactionToMessageAsync(int messageId, string emojiColons, int userId);
    }
}