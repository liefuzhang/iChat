using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Workspace
    {
        protected Workspace() {        }

        public Workspace(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new Exception("Workspace name cannot be empty");
            }

            Name = name;
            CreatedDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int OwnerId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public ICollection<Channel> Channels { get; private set; }
        public ICollection<User> Users { get; private set; }

        public void SetOwner(int ownerId) {
            if (ownerId < 1) {
                throw new ArgumentException("Invalid ownerId");
            }
        }
    }
}
