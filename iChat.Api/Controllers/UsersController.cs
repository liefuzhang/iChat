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

        public UsersController(iChatContext context, IUserService userService,
            IWorkspaceService workspaceService, IChannelService channelService) {
            _context = context;
            _userService = userService;
            _workspaceService = workspaceService;
            _channelService = channelService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAsync() {
            try {
                var users = await _userService
                    .GetAllUsersAsync(User.GetWorkplaceId());
                return users.ToList();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAsync(int id) {
            try {
                var user = await _userService.GetUserByIdAsync(id, User.GetWorkplaceId());
                if (user == null) {
                    return NotFound();
                }

                return user;
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // POST api/users/invite
        [HttpPost("invite")]
        public async Task<IActionResult> InviteUsers(List<string> emails) {
            try {
                var user = await _userService.GetUserByIdAsync(User.GetUserId());
                var workspace = await _workspaceService.GetWorkspaceByIdAsync(User.GetWorkplaceId());
                await _userService.InviteUsersAsync(user, workspace, emails);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/users/acceptInvitation
        [HttpPost("acceptInvitation")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptInvitation(UserInvitationDto userInvitationDto) {
            try {
                var userId = await _userService.AcceptInvitationAsync(userInvitationDto);
                var user = await _userService.GetUserByIdAsync(userId);
                await _channelService.AddUserToDefaultChannelsAsync(userId, user.WorkspaceId);

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
