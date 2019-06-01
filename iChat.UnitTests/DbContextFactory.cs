using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace iChat.UnitTests
{
    public class DbContextFactory
    {
        public iChatContext CreateNewContext() {
            var options = new DbContextOptionsBuilder<iChatContext>()
                .UseInMemoryDatabase(databaseName: $"test_database")
                .Options;

            var context = new iChatContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context; // there is no need to dispose context, 
                            // it's being taken care of by .NET core
        }

    }
}
