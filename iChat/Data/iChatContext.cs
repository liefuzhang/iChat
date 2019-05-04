using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iChat.Data
{
    public class iChatContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public iChatContext(DbContextOptions<iChatContext> options) : base(options) {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Channel>().ToTable("Channel");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<Message>().ToTable("Message");

            //// config many-to-many for channel and user
            modelBuilder.Entity<Subscription>()
                .HasKey(s => new { s.ChannelId, s.UserId });

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.ChannelSubscriptions)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Channel)
                .WithMany(c => c.ChannelSubscriptions)
                .HasForeignKey(s => s.ChannelId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
