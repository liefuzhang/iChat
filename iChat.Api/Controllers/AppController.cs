using iChat.Api.Dtos;
using iChat.Api.Services;
using iChat.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using iChat.Api.Extensions;
using iChat.Api.Models;
using Microsoft.AspNetCore.Authorization;
using iChat.Api.Contract;
using System.Collections.Generic;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase {
        private ICacheService _cacheService;
        private IUserService _userService;

        public AppController(ICacheService cacheService, IUserService userService) {
            _cacheService = cacheService;
            _userService = userService;
        }

        // GET api/app/userSesstionData
        [HttpGet("userSessionData")]
        public async Task<ActionResult<UserSessionData>> GetUserSessionDataAsync() {
            var sessionData = new UserSessionData {
                ActiveSidebarItem = await _cacheService.GetActiveSidebarItemAsync(User.GetUserId(), User.GetWorkplaceId()),
                UserStatus = await _userService.GetUserStatus(User.GetUserId(), User.GetWorkplaceId())
            };

            return Ok(sessionData);
        }

        // POST api/app/activeSidebarItem
        [HttpPost("activeSidebarItem")]
        public async Task<IActionResult> SetActiveSidebarItemAsync(ActiveSidebarItem item) {
            await _cacheService.SetActiveSidebarItemAsync(item.IsChannel, item.ItemId,
                User.GetUserId(), User.GetWorkplaceId());

            return Ok();
        }
    }
}
