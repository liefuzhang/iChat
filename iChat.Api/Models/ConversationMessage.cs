using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace iChat.Api.Models
{
    public class ConversationMessage : Message
    {
        protected ConversationMessage() { }
        public ConversationMessage(int conversationId, string content, int senderId, int workspaceId)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Content cannot be empty");
            }
            if (conversationId < 1)
            {
                throw new Exception("Invalid conversation");
            }
            if (senderId < 1)
            {
                throw new Exception("Invalid sender");
            }
            if (workspaceId < 1)
            {
                throw new Exception("Invalid workspace");
            }

            ConversationId = conversationId;
            Content = content;
            SenderId = senderId;
            WorkspaceId = workspaceId;
            CreatedDate = DateTime.Now;
        }
        public int ConversationId { get; private set; }
        public Conversation Conversation { get; private set; }
    }
}