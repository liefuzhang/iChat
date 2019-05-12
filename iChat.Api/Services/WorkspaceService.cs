using System;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Models;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iChat.Api.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly iChatContext _context;

        public WorkspaceService(iChatContext context)
        {
            _context = context;
        }

        public int Register(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("Workspace name cannot be empty");
            }

            if (_context.Workspaces.Any(w => w.Name == name)) {
                throw new Exception($"Workspace \"{name}\" is already taken");
            }

            var workspace = new Workspace {
                Name = name,
                CreatedDate = DateTime.Now
            };

            _context.Workspaces.Add(workspace);
            _context.SaveChanges();

            return workspace.Id;
        }

        public void UpdateOwnerId(int workspaceId, int userId) {
            if (workspaceId < 1 || userId < 1) {
                throw new ArgumentException("Invalid workspaceId or userId");
            }

            var workspace = _context.Workspaces.Single(w => w.Id == workspaceId);
            workspace.OwnerId = userId;
            _context.SaveChanges();
        }
    }
}