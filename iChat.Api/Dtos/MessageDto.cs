using iChat.Api.Models;
using System.Collections.Generic;

namespace iChat.Api.Dtos {
    public class MessageDto {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public UserDto Sender { get; set; }
        public string Content { get; set; }
        public int WorkspaceId { get; set; }
        public string TimeString { get; set; }
        public bool IsConsecutiveMessage { get; set; }
        public bool HasFileAttachments { get; set; }
        public bool ContentEdited { get; set; }
        public List<FileDto> FileAttachments { get; set; }
        public List<MessageReactionDto> MessageReactionDtos { get; set; }
    }
}