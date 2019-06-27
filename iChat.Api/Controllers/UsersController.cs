using iChat.Api.Services;
using iChat.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using iChat.Api.Extensions;
using iChat.Api.Constants;
using iChat.Api.Contract;
using iChat.Api.Dtos;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase {

        private readonly iChatContext _context;
        private readonly IUserService _userService;
        private readonly IChannelService _channelService;
        private readonly IConversationService _conversationService;
        private readonly IWorkspaceService _workspaceService;
        private ICacheService _cacheService;

        public UsersController(iChatContext context, IUserService userService,
            IWorkspaceService workspaceService, IChannelService channelService,
            ICacheService cacheService, IConversationService conversationService) {
            _context = context;
            _userService = userService;
            _workspaceService = workspaceService;
            _channelService = channelService;
            _cacheService = cacheService;
            _conversationService = conversationService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync() {
            var users = await _userService
                .GetAllUsersAsync(User.GetWorkspaceId());
            return users.ToList();
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAsync(int id) {
            var user = await _userService.GetUserByIdAsync(id, User.GetWorkspaceId());
            if (user == null) {
                return NotFound();
            }

            return user;
        }

        // POST api/users/invite
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUsersAsync(List<string> emails) {
            var user = await _userService.GetUserByIdAsync(User.GetUserId());
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(User.GetWorkspaceId());
            await _userService.InviteUsersAsync(user, workspace, emails);

            return Ok();
        }

        // POST api/users/acceptInvitation
        [HttpPost("acceptInvitation")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvitationAsync(UserInvitationDto userInvitationDto) {
            var userId = await _userService.AcceptInvitationAsync(userInvitationDto);
            var user = await _userService.GetUserByIdAsync(userId);
            await _channelService.AddUserToDefaultChannelsAsync(userId, user.WorkspaceId);
            await _conversationService.StartSelfConversationAsync(userId, user.WorkspaceId);

            return Ok();
        }

        // POST api/users/forgotPassword
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]string email) {
            await _userService.ForgotPasswordAsync(email);

            return Ok();
        }

        // POST api/users/resetPassword
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto) {
            await _userService.ResetPasswordAsync(resetPasswordDto);

            return Ok();
        }

        // POST api/users/status
        [HttpPost("status")]
        public async Task<IActionResult> SetStatus([FromBody]string status) {
            var statusEnum = (UserStatus)Enum.Parse(typeof(UserStatus), status);
            await _userService.SetUserStatusAsync(User.GetUserId(), User.GetWorkspaceId(), statusEnum);

            return Ok();
        }

        // POST api/users/clearStatus
        [HttpPost("clearStatus")]
        public async Task<IActionResult> ClearStatus() {
            await _userService.ClearUserStatusAsync(User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        // POST api/users/editProfile
        [HttpPost("editProfile")]
        public async Task<IActionResult> EditProfile(UserEditDto userDto) {
            await _userService.EditProfile(userDto, User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }

        [AllowAnonymous]
        // TODO remove after development
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserLoginDto loginDto) {
            var workspaceId = 1;
            var userId = await _userService.RegisterAsync(loginDto.Email, loginDto.Password, loginDto.Email, workspaceId);
            await _channelService.AddUserToDefaultChannelsAsync(userId, workspaceId);
            await _conversationService.StartSelfConversationAsync(userId, workspaceId);

            return Ok();
        }
    }
}
