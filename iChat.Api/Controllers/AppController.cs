using iChat.Api.Contract;
using iChat.Api.Extensions;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IUserService _userService;

        public AppController(ICacheService cacheService, IUserService userService)
        {
            _cacheService = cacheService;
            _userService = userService;
        }

        // GET api/app/userSesstionData
        [HttpGet("userSessionData")]
        public async Task<ActionResult<UserSessionData>> GetUserSessionDataAsync()
        {
            var sessionData = new UserSessionData
            {
                ActiveSidebarItem = await _cacheService.GetActiveSidebarItemAsync(User.GetUserId(), User.GetWorkspaceId()),
                UserStatus = await _userService.GetUserStatus(User.GetUserId(), User.GetWorkspaceId())
            };

            return Ok(sessionData);
        }

        // POST api/app/activeSidebarItem
        [HttpPost("activeSidebarItem")]
        public async Task<IActionResult> SetActiveSidebarItemAsync(ActiveSidebarItem item)
        {
            await _cacheService.SetActiveSidebarItemAsync(item.IsChannel, item.ItemId,
                User.GetUserId(), User.GetWorkspaceId());

            return Ok();
        }
    }
}
