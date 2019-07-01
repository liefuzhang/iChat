using iChat.Api.Data;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class WorkspaceQueryService : IWorkspaceQueryService
    {
        private readonly iChatContext _context;

        public WorkspaceQueryService(iChatContext context)
        {
            _context = context;
        }

        public async Task<Workspace> GetWorkspaceByIdAsync(int workspaceId)
        {
            return await _context.Workspaces.SingleAsync(w => w.Id == workspaceId);
        }
    }
}