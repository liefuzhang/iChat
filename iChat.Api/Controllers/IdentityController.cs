using iChat.Api.Constants;
using iChat.Api.Dtos;
using iChat.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IIdentityService _identityService;
        private IWorkspaceService _workspaceService;

        public IdentityController(IIdentityService identityService, IWorkspaceService workspaceService)
        {
            _identityService = identityService;
            _workspaceService = workspaceService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserDto userDto)
        {
            try
            {
                var user = await _identityService.AuthenticateAsync(userDto.Email, userDto.Password);

                if (user == null)
                {
                    return BadRequest("Email or password is incorrect");
                }

                var workspace = await _workspaceService.GetWorkspaceByIdAsync(user.WorkspaceId);

                var tokenString = _identityService.GenerateAccessToken(user.Id);

                // return basic user info (without password) and token to store on client side
                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    displayName = user.DisplayName,
                    workspaceName = workspace?.Name,
                    identiconPath = Path.Combine(iChatConstants.IdenticonPath, $"{user.IdenticonGuid}{iChatConstants.IdenticonExt}"),
                    token = tokenString
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto userDto)
        {
            try
            {
                await _identityService.RegisterAsync(userDto.Email, userDto.Password, userDto.WorkspaceId);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }
    }
}
