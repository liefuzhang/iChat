using System;
using iChat.Api.Dtos;

namespace iChat.Api.Models
{
    public abstract class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }
        public int WorkspaceId { get; set; }
        public DateTime CreatedDate { get; set; }

        public MessageDto MapToMessageDto()
        {
            return new MessageDto
            {
                Id = Id,
                WorkspaceId = WorkspaceId,
                Content = Content,
                SenderId = SenderId,
                Sender = Sender,
                TimeString = CreatedDate.ToShortTimeString()
            };
        }
    }
}
