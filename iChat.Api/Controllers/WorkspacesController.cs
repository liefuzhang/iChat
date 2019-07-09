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
        private readonly IWorkspaceCommandService _workspaceCommandService;
        private readonly IChannelCommandService _channelCommandService;
        private readonly IConversationCommandService _conversationCommandService;
        private readonly IUserQueryService _userQueryService;
        private readonly IUserCommandService _userCommandService;

        public WorkspacesController(iChatContext context, IUserQueryService userQueryService,
            IUserCommandService userCommandService, IChannelCommandService channelCommandService, 
            IConversationCommandService conversationCommandService, IWorkspaceCommandService workspaceCommandService)
        {
            _userQueryService = userQueryService;
            _userCommandService = userCommandService;
            _channelCommandService = channelCommandService;
            _conversationCommandService = conversationCommandService;
            _workspaceCommandService = workspaceCommandService;
        }

        // POST api/workspaces
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] WorkspaceDto workspaceDto)
        {
            if (await _userQueryService.IsEmailRegisteredAsync(workspaceDto.Email))
            {
                throw new Exception($"Email \"{workspaceDto.Email}\" is already taken");
            }

            var workspaceId = await _workspaceCommandService.RegisterAsync(workspaceDto.WorkspaceName);
            var userId = await _userCommandService.RegisterAsync(workspaceDto.Email, workspaceDto.Password, workspaceDto.DisplayName, workspaceId);
            await _workspaceCommandService.UpdateOwnerIdAsync(workspaceId, userId);

            await _channelCommandService.AddDefaultChannelsToNewWorkplaceAsync(userId, workspaceId);
            await _channelCommandService.AddUserToDefaultChannelsAsync(userId, workspaceId);

            await _conversationCommandService.StartSelfConversationAsync(userId, workspaceId);

            return Ok();
        }
    }
}
