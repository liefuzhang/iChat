﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface IMessageCommandService
    {
        Task PostMessageToConversationAsync(string newMessage, int conversationId, int currentUserId, int workspaceId);
        Task PostMessageToChannelAsync(string newMessage, int channelId, int currentUserId, int workspaceId);
        Task UpdateMessageInConversationAsync(string message, int conversationId, int messageId, int currentUserId);
        Task UpdateMessageInChannelAsync(string message, int channelId, int messageId, int currentUserId);
        Task PostFileMessageToConversationAsync(IList<IFormFile> files, int conversationId, int userId, int workspaceId);
        Task PostFileMessageToChannelAsync(IList<IFormFile> files, int channelId, int userId, int workspaceId);
        Task<(Stream stream, string contentType)> DownloadFileAsync(int fileId, int userId, int workspaceId);
        Task ShareFileToConversationAsync(int conversationId, int fileId, int userId, int workspaceId);
        Task ShareFileToChannelAsync(int channelId, int fileId, int userId, int workspaceId);
        Task DeleteMessageAsync(int messageId, int userId);
        Task AddReactionToMessageAsync(int messageId, string emojiColons, int userId);
        Task RemoveReactionToMessageAsync(int messageId, string emojiColons, int userId);
    }
}