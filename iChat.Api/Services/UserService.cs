using System;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services
{
    public class UserService : IUserService
    {
        private readonly iChatContext _context;

        public UserService(iChatContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id) {
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email) {
            var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}