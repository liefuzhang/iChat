using System;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services {
    public class MessageService
    {
        private readonly iChatContext _context;

        public MessageService(iChatContext context) {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, int workspaceId) {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email, int workspaceId) {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}