using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Data {
    public class iChatContext : DbContext {
        public iChatContext(DbContextOptions<iChatContext> options) : base(options) {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChannelSubscription> ChannelSubscriptions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }

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

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Workspace)
                .WithMany(w => w.Users)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
