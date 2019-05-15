using iChat.Api.Dtos;
using iChat.Api.Services;
using iChat.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
                _identityService.ValidateUserEmailAndPassword(workspaceDto.Email, workspaceDto.Password);
                var workspaceId = _workspaceService.Register(workspaceDto.WorkspaceName);
                var userId = _identityService.Register(workspaceDto.Email, workspaceDto.Password, workspaceId);
                _workspaceService.UpdateOwnerId(workspaceId, userId);

                await _channelService.AddDefaultChannelsToNewWorkplaceAsync(workspaceId);
                await _channelService.AddUserToDefaultChannelsAsync(userId, workspaceId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
