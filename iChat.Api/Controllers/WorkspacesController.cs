using iChat.Api.Dtos;
using iChat.Api.Models;
using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspacesController : ControllerBase
    {

        private readonly iChatContext _context;
        private readonly IWorkspaceService _workspaceService;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public WorkspacesController(iChatContext context, IWorkspaceService workspaceService,
            IIdentityService identityService, IUserService userService)
        {
            _context = context;
            _workspaceService = workspaceService;
            _identityService = identityService;
            _userService = userService;
        }

        // POST api/workspaces
        [HttpPost("register")]
        public IActionResult Register([FromBody] WorkspaceDto workspaceDto)
        {
            try {
                _identityService.ValidateUserEmailAndPassword(workspaceDto.Email, workspaceDto.Password);
                var workspaceId = _workspaceService.Register(workspaceDto.WorkspaceName);
                var userId = _identityService.Register(workspaceDto.Email, workspaceDto.Password, workspaceId);
                _workspaceService.UpdateOwnerId(workspaceId, userId);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
