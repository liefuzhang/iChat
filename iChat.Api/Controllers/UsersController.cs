using iChat.Api.Services;
using iChat.Data;
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

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase {

        private readonly iChatContext _context;
        private readonly IUserService _userService;
        private readonly IWorkspaceService _workspaceService;
        private readonly IEmailService _emailService;

        public UsersController(iChatContext context, IUserService userService, IEmailService emailService, IWorkspaceService workspaceService) {
            _context = context;
            _userService = userService;
            _workspaceService = workspaceService;
            _emailService = emailService;
        }
        
        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAsync() {
            try {
                var users = await _context.Users.AsNoTracking()
                    .Where(u => u.WorkspaceId == User.GetWorkplaceId())
                    .ToListAsync();
                return users;
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
        public async Task<IActionResult> Post(List<string> emails) {
            try {
                var user = await _userService.GetUserByIdAsync(User.GetUserId());
                var workspace = await _workspaceService.GetWorkspaceByIdAsync(User.GetWorkplaceId());
                await _emailService.SendUserInvitationEmailAsync(new UserInvitationData {
                    ReceiverAddresses = emails,
                    InviterName = user.DisplayName,
                    InviterEmail = user.Email,
                    WorkspaceName = workspace.Name
                });

                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
