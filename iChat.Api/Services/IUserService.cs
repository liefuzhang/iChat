﻿using System.Threading.Tasks;
using iChat.Api.Models;

namespace iChat.Api.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id, int workspaceId);
        Task<User> GetUserByEmailAsync(string email, int workspaceId);
    }
}