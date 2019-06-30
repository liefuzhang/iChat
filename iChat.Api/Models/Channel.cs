using System;
using System.Collections.Generic;
using System.Globalization;

namespace iChat.Api.Models {
    public class Channel {
        public Channel(string name, int userId, int workspaceId, string topic) : this() {
            if (string.IsNullOrWhiteSpace(name) || userId < 1 || workspaceId < 1) {
                throw new ArgumentException("Invalid argument");
            }

            Name = name;
            CreatedByUserId = userId;
            WorkspaceId = workspaceId;
            Topic = topic;
            CreatedDate = DateTime.Now;
        }

        protected Channel() {
            ChannelSubscriptions = new HashSet<ChannelSubscription>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Topic { get; private set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ChannelMessage> ChannelMessages { get; private set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; private set; }
        public string CreatedDateString => CreatedDate.ToString("dddd, MMM d", CultureInfo.InvariantCulture);
    }
}
