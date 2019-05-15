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

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase {

        private readonly iChatContext _context;
        private readonly IUserService _userService;

        public UsersController(iChatContext context, IUserService userService) {
            _context = context;
            _userService = userService;
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {
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
