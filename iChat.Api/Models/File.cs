using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class File
    {
        public File(string savedName, string name, int userId, int workspaceId) : this()
        {
            if (string.IsNullOrWhiteSpace(savedName) || userId < 1 || workspaceId < 1)
            {
                throw new ArgumentException("Invalid argument");
            }

            SavedName = savedName;
            Name = name;
            UploadedByUserId = userId;
            WorkspaceId = workspaceId;
            UploadedDate = DateTime.Now;
        }

        protected File()
        {
            MessageFileAttachments = new HashSet<MessageFileAttachment>();
        }

        public int Id { get; private set; }
        public string SavedName { get; private set; }
        public string Name { get; private set; }
        public int UploadedByUserId { get; private set; }
        public User UploadedByUser { get; private set; }
        public DateTime UploadedDate { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<MessageFileAttachment> MessageFileAttachments { get; private set; }
    }
}
