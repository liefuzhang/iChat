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
        private readonly IWorkspaceService _workspaceService;
        private ICacheService _cacheService;

        public UsersController(iChatContext context, IUserService userService,
            IWorkspaceService workspaceService, IChannelService channelService,
            ICacheService cacheService) {
            _context = context;
            _userService = userService;
            _workspaceService = workspaceService;
            _channelService = channelService;
            _cacheService = cacheService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync() {
            var users = await _userService
                .GetAllUsersAsync(User.GetWorkplaceId());
            return users.ToList();
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAsync(int id) {
            var user = await _userService.GetUserByIdAsync(id, User.GetWorkplaceId());
            if (user == null) {
                return NotFound();
            }

            return user;
        }

        // POST api/users/invite
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUsers(List<string> emails) {
            var user = await _userService.GetUserByIdAsync(User.GetUserId());
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(User.GetWorkplaceId());
            await _userService.InviteUsersAsync(user, workspace, emails);

            return Ok();
        }

        // POST api/users/acceptInvitation
        [HttpPost("acceptInvitation")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvitation(UserInvitationDto userInvitationDto) {
            var userId = await _userService.AcceptInvitationAsync(userInvitationDto);
            var user = await _userService.GetUserByIdAsync(userId);
            await _channelService.AddUserToDefaultChannelsAsync(userId, user.WorkspaceId);

            return Ok();
        }

        // POST api/users/status
        [HttpPost("status")]
        public async Task<IActionResult> SetStatus([FromBody]string status) {
            var statusEnum = (UserStatus)Enum.Parse(typeof(UserStatus), status);
            await _userService.SetUserStatusAsync(User.GetUserId(), User.GetWorkplaceId(), statusEnum);

            return Ok();
        }

        // POST api/users/clearStatus
        [HttpPost("clearStatus")]
        public async Task<IActionResult> ClearStatus() {
            await _userService.ClearUserStatusAsync(User.GetUserId(), User.GetWorkplaceId());

            return Ok();
        }


        [AllowAnonymous]
        // TODO remove after development
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto userDto) {
            await _userService.RegisterAsync(userDto.Email, userDto.Password, 1);
            return Ok();
        }
    }
}
