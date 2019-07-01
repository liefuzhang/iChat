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
        private readonly IUserQueryService _userQueryService;

        public AppController(ICacheService cacheService, IUserQueryService userQueryService)
        {
            _cacheService = cacheService;
            _userQueryService = userQueryService;
        }

        // GET api/app/userSesstionData
        [HttpGet("userSessionData")]
        public async Task<ActionResult<UserSessionData>> GetUserSessionDataAsync()
        {
            var sessionData = new UserSessionData
            {
                ActiveSidebarItem = await _cacheService.GetActiveSidebarItemAsync(User.GetUserId(), User.GetWorkspaceId()),
                UserStatus = await _userQueryService.GetUserStatus(User.GetUserId(), User.GetWorkspaceId())
            };

            return Ok(sessionData);
        }
    }
}
