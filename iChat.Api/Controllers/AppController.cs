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

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        private ICacheService _cacheService;

        public AppController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        // GET api/app/activeSidebarItem
        [HttpGet("activeSidebarItem")]
        public async Task<ActionResult<ActiveSidebarItem>> GetActiveSidebarItemAsync()
        {
            var item = await _cacheService.GetActiveSidebarItemAsync(User.GetWorkplaceId(), User.GetUserId());

            return Ok(item);
        }

        // POST api/app/activeSidebarItem
        [HttpPost("activeSidebarItem")]
        public async Task<IActionResult> SetActiveSidebarItemAsync(ActiveSidebarItem item) {
            await _cacheService.SetActiveSidebarItemAsync(item.IsChannel, item.ItemId,
                User.GetWorkplaceId(), User.GetUserId());

            return Ok();
        }
    }
}
