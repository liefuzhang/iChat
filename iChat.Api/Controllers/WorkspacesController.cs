using iChat.Api.Dtos;
using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using iChat.Api.Extensions;
using iChat.Api.Models;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspacesController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IChannelService _channelService;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public WorkspacesController(iChatContext context, IWorkspaceService workspaceService,
            IIdentityService identityService, IUserService userService, IChannelService channelService)
        {
            _workspaceService = workspaceService;
            _identityService = identityService;
            _userService = userService;
            _channelService = channelService;
        }

        // POST api/workspaces
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] WorkspaceDto workspaceDto)
        {
            try
            {
                await _identityService.ValidateUserEmailAndPasswordAsync(workspaceDto.Email, workspaceDto.Password);
                var workspaceId = await _workspaceService.RegisterAsync(workspaceDto.WorkspaceName);
                var userId = await _identityService.RegisterAsync(workspaceDto.Email, workspaceDto.Password, workspaceId);
                await _workspaceService.UpdateOwnerIdAsync(workspaceId, userId);

                await _channelService.AddDefaultChannelsToNewWorkplaceAsync(workspaceId);
                await _channelService.AddUserToDefaultChannelsAsync(userId, workspaceId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/workspaces
        public async Task<ActionResult<Workspace>> GetWorkspaceAsync()
        {
            try
            {
                var workspace = await _workspaceService.GetWorkspaceByIdAsync(User.GetWorkplaceId());

                return Ok(workspace);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
