using System;
using System.Collections.Generic;

namespace iChat.Api.Models
{
    public class Channel
    {
        public Channel(string name, int workspaceId, string topic) : this()
        {
            if (string.IsNullOrWhiteSpace(name) || workspaceId < 1)
            {
                throw new ArgumentException("Invalid argument");
            }

            Name = name;
            WorkspaceId = workspaceId;
            Topic = topic;
        }

        protected Channel()
        {
            ChannelSubscriptions = new HashSet<ChannelSubscription>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Topic { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ChannelMessage> ChannelMessages { get; private set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; private set; }
    }
}
