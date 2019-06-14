using System;

namespace iChat.Api.Models
{
    public class FileDto
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int UploadedByUserId { get; private set; }
        public DateTime UploadedDate { get; private set; }
    }
}
