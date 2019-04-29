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
    public class iChatContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public iChatContext(DbContextOptions<iChatContext> options) : base(options) {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Channel>().ToTable("Channel");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<Message>().ToTable("Message");

            modelBuilder.Entity<Subscription>()
                .HasKey(s => new { s.ChannelID, s.UserID });
        }
    }
}
