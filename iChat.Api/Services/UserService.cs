﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services {
    public class UserService : IUserService {
        private readonly iChatContext _context;

        public UserService(iChatContext context) {
            _context = context;
        }

        // when workspace is not available (e.g. onTokenValidated)
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

        public async Task<IEnumerable<User>> GetAllUsersAsync(int workspaceId)
        {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .ToListAsync();

            return users;
        }
    }
}