using AutoMapper;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using iChat.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase {
        private IIdentityService _identityService;
        private readonly AppSettings _appSettings;
        private IMapper _mapper;

        public IdentityController(IIdentityService identityService, IOptions<AppSettings> appSettings, IMapper mapper) {
            _identityService = identityService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto) {
            try {
                var user = _identityService.Authenticate(userDto.Email, userDto.Password);

                if (user == null) {
                    return BadRequest("Email or password is incorrect");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info (without password) and token to store on client side
                return Ok(new {
                    id = user.Id,
                    email = user.Email,
                    displayName = user.DisplayName,
                    token = tokenString
                });
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto) {
            try {
                _identityService.Register(userDto.Email, userDto.Password, userDto.WorkspaceId);
                return Ok();
            } catch (Exception ex) {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }
    }
}
