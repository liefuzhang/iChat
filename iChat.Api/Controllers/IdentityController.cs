using iChat.Api.Dtos;
using iChat.Api.Extensions;
using iChat.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        // POST api/identity/authenticate
        [HttpPost("authenticate")]
        public async Task<ActionResult<UserProfileDto>> AuthenticateAsync(UserLoginDto loginDto)
        {
            var userProfileDto = await _identityService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            // return basic user info (without password) and token to store on client side
            return userProfileDto;
        }

        // GET api/identity/userProfile
        [HttpGet("userProfile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
        {
            var userProfileDto = await _identityService.GetUserProfileAsync(User.GetUserId());

            return userProfileDto;
        }
    }
}
