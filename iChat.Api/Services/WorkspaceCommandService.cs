using iChat.Api.Models;
using iChat.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class WorkspaceCommandService : IWorkspaceCommandService
    {
        private readonly iChatContext _context;

        public WorkspaceCommandService(iChatContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterAsync(string name)
        {
            if (_context.Workspaces.Any(w => w.Name == name))
            {
                throw new Exception($"Workspace \"{name}\" is already taken");
            }

            var workspace = new Workspace(name);

            _context.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();

            return workspace.Id;
        }

        public async Task UpdateOwnerIdAsync(int workspaceId, int userId)
        {
            var workspace = await _context.Workspaces.SingleAsync(w => w.Id == workspaceId);
            workspace.SetOwner(userId);
            await _context.SaveChangesAsync();
        }
    }
}