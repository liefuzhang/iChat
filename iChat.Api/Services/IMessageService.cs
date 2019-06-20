using iChat.Api.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public interface IMessageService {
        Task<IEnumerable<MessageGroupDto>> GetMessagesForChannelAsync(int channelId, int userId, int workspaceId);
        Task<IEnumerable<MessageGroupDto>> GetMessagesForConversationAsync(int conversationId, int userId, int workspaceId);
        Task<int> PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId, int workspaceId, bool hasFileAttachments = false);
        Task<int> PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId, bool hasFileAttachments = false);
        Task PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId);
        Task PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId);
        Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId);
    }
}