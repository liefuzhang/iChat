using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Data {
    public class iChatContext : DbContext {
        public iChatContext(DbContextOptions<iChatContext> options) : base(options) {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChannelSubscription> ChannelSubscriptions { get; set; }
        public DbSet<ConversationUser> ConversationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<MessageFileAttachment> MessageFileAttachments { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }
        public DbSet<MessageReactionUser> MessageReactionUsers { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }
        public DbSet<ResetPasswordRequset> ResetPasswordRequsets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            //// config many-to-many for channel and user
            modelBuilder.Entity<ChannelSubscription>()
                .HasKey(s => new { s.ChannelId, s.UserId });

            modelBuilder.Entity<ChannelSubscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.ChannelSubscriptions)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<ChannelSubscription>()
                .HasOne(s => s.Channel)
                .WithMany(c => c.ChannelSubscriptions)
                .HasForeignKey(s => s.ChannelId);

            modelBuilder.Entity<ConversationUser>()
                .HasKey(s => new { s.ConversationId, s.UserId });

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Workspace)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Workspace)
                .WithMany(w => w.Users)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<File>()
                .HasOne(f => f.Workspace)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<File>()
                .HasOne(f => f.UploadedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConversationMessage>()
                .HasOne(cm => cm.Conversation)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageFileAttachment>()
                .HasKey(m => new { m.FileId, m.MessageId });

            modelBuilder.Entity<MessageReactionUser>()
                .HasKey(m => new { m.MessageReactionId, m.UserId });

            modelBuilder.Entity<Channel>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
