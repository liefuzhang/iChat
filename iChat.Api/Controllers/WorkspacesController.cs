using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IConversationService _conversationService;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public WorkspacesController(iChatContext context, IWorkspaceService workspaceService,
            IIdentityService identityService, IUserService userService,
            IChannelService channelService, IConversationService conversationService)
        {
            _workspaceService = workspaceService;
            _identityService = identityService;
            _userService = userService;
            _channelService = channelService;
            _conversationService = conversationService;
        }

        // POST api/workspaces
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] WorkspaceDto workspaceDto)
        {
            if (await _userService.IsEmailRegisteredAsync(workspaceDto.Email))
            {
                throw new Exception($"Email \"{workspaceDto.Email}\" is already taken");
            }

            var workspaceId = await _workspaceService.RegisterAsync(workspaceDto.WorkspaceName);
            var userId = await _userService.RegisterAsync(workspaceDto.Email, workspaceDto.Password, workspaceDto.DisplayName, workspaceId);
            await _workspaceService.UpdateOwnerIdAsync(workspaceId, userId);

            await _channelService.AddDefaultChannelsToNewWorkplaceAsync(workspaceId);
            await _channelService.AddUserToDefaultChannelsAsync(userId, workspaceId);

            await _conversationService.StartSelfConversationAsync(userId, workspaceId);

            return Ok();
        }
    }
}
