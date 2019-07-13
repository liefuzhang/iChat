using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class UserQueryService : IUserQueryService {
        private readonly iChatContext _context;
        private readonly IMapper _mapper;

        public UserQueryService(iChatContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // when workspace is not available (e.g. onTokenValidated)
        public async Task<UserDto> GetUserByIdAsync(int id) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int id, int workspaceId) {
            var user = await _context.Users
                .Where(u => u.WorkspaceId == workspaceId)
                .SingleOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserDto>(user);
        }
        
        public async Task<string> GetUserNamesAsync(List<int> userIds, int workspaceId) {
            var userDisplayNames = await _context.Users.Where(u => userIds.Contains(u.Id) &&
                                                                u.WorkspaceId == workspaceId)
                                                        .OrderBy(u => u.Id)
                                                        .Select(u => u.DisplayName)
                                                        .ToListAsync();
            return string.Join(", ", userDisplayNames);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int workspaceId) {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.WorkspaceId == workspaceId)
                .Select(u => _mapper.Map<UserDto>(u))
                .ToListAsync();

            return users;
        }

        public async Task<string> GetUserStatus(int userId, int workspaceId) {
            var user = await GetUserByIdAsync(userId, workspaceId);
            if (user == null) {
                throw new Exception("User cannot be found.");
            }

            return user.Status.ToString();
        }

        public async Task<bool> IsEmailRegisteredAsync(string email) {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}