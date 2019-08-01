using iChat.Api.Constants;
using iChat.Api.Dtos;
using iChat.Api.Extensions;
using iChat.Api.Models;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IUserQueryService _userQueryService;
        private readonly IUserCommandService _userCommandService;
        private readonly IChannelCommandService _channelCommandService;
        private readonly IConversationCommandService _conversationCommandService;
        private readonly IWorkspaceQueryService _workspaceQueryService;
        private readonly ICacheService _cacheService;

        public UsersController(IUserQueryService userQueryService,
            IUserCommandService userCommandService,
            IWorkspaceQueryService workspaceService, IChannelCommandService channelCommandService,
            IConversationCommandService conversationCommandService,
            ICacheService cacheService)
        {
            _userQueryService = userQueryService;
            _userCommandService = userCommandService;
            _workspaceQueryService = workspaceService;
            _channelCommandService = channelCommandService;
            _conversationCommandService = conversationCommandService;
            _cacheService = cacheService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync()
        {
            var users = await _userQueryService
                .GetAllUsersAsync(User.GetWorkspaceId());
            return users.ToList();
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetAsync(int id)
        {
            var user = await _userQueryService.GetUserByIdAsync(id, User.GetWorkspaceId());
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST api/users/invite
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUsersAsync(List<string> emails)
        {
            var user = await _userQueryService.GetUserByIdAsync(User.GetUserId());
            var workspace = await _workspaceQueryService.GetWorkspaceByIdAsync(User.GetWorkspaceId());
            await _userCommandService.InviteUsersAsync(user, workspace, emails);

            return Ok();
        }

        // POST api/users/acceptInvitation
        [HttpPost("acceptInvitation")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvitationAsync(UserInvitationDto userInvitationDto)
        {
            var userId = await _userCommandService.AcceptInvitationAsync(userInvitationDto);
            var user = await _userQueryService.GetUserByIdAsync(userId);
            await _channelCommandService.AddUserToDefaultChannelsAsync(userId, user.WorkspaceId);
            await _conversationCommandService.StartSelfConversationAsync(userId, user.WorkspaceId);

            return Ok();
        }

        // POST api/users/forgotPassword
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]string email)
        {
            await _userCommandService.ForgotPasswordAsync(email);

            return Ok();
        }

        // POST api/users/resetPassword
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            await _userCommandService.ResetPasswordAsync(resetPasswordDto);

            return Ok();
        }

        // POST api/users/status
        [HttpPost("status")]
        public async Task<IActionResult> SetStatus([FromBody]string status)
        {
            var statusEnum = (UserStatus)Enum.Parse(typeof(UserStatus), status);
            await _userCommandService.SetUserStatusAsync(User.GetUserId(), User.GetWorkspaceId(), statusEnum);

            return Ok();
        }

        // POST api/users/clearStatus
        [HttpPost("clearStatus")]
        public async Task<IActionResult> ClearStatus()
        {
            await _userCommandService.ClearUserStatusAsync(User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/users/editProfile
        [HttpPost("editProfile")]
        public async Task<IActionResult> EditProfile(UserEditDto userDto)
        {
            await _userCommandService.EditProfile(userDto, User.GetUserId());

            return Ok();
        }

        // GET api/users/1/onlineStatus
        [HttpGet("{userId}/onlineStatus")]
        public async Task<IActionResult> GetUserOnlineStatusAsync(int userId) {
            var isOnline = await _cacheService.GetUserOnlineAsync(userId, User.GetWorkspaceId());

            return Ok(isOnline);
        }
    }
}
