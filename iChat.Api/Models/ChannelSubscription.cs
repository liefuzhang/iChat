using System;

namespace iChat.Api.Models
{
    public class ChannelSubscription
    {
        protected ChannelSubscription() {
        }

        public ChannelSubscription(int channelId, int userId) {
            if (channelId < 1 || userId < 1) {
                throw new ArgumentException("Invalid input");
            }

            ChannelId = channelId;
            UserId = userId;
        }

        public int ChannelId { get; private set; }
        public Channel Channel { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
    }
}
