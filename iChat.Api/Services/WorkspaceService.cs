using iChat.Api.Models;
using iChat.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly iChatContext _context;

        public WorkspaceService(iChatContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Workspace name cannot be empty");
            }

            if (_context.Workspaces.Any(w => w.Name == name))
            {
                throw new Exception($"Workspace \"{name}\" is already taken");
            }

            var workspace = new Workspace
            {
                Name = name,
                CreatedDate = DateTime.Now
            };

            _context.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();

            return workspace.Id;
        }

        public async Task UpdateOwnerIdAsync(int workspaceId, int userId)
        {
            if (workspaceId < 1 || userId < 1)
            {
                throw new ArgumentException("Invalid workspaceId or userId");
            }

            var workspace = _context.Workspaces.Single(w => w.Id == workspaceId);
            workspace.OwnerId = userId;
            await _context.SaveChangesAsync();
        }

        public async Task<Workspace> GetWorkspaceByIdAsync(int workspaceId)
        {
            return await _context.Workspaces.SingleAsync(w => w.Id == workspaceId);
        }
    }
}