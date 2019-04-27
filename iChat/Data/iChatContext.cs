using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Models;
using Microsoft.EntityFrameworkCore;

namespace iChat.Data
{
    public class iChatContext : DbContext {
        public iChatContext(DbContextOptions<iChatContext> options) : base(options) {
        }

        public DbSet<Channel> Channels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Channel>().ToTable("Channel");
        }
    }
}
