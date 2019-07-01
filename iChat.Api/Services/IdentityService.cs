using AutoMapper;
using iChat.Api.Data;
using iChat.Api.Dtos;
using iChat.Api.Helpers;
using iChat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly iChatContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUserQueryService _userQueryService;
        private readonly IWorkspaceQueryService _workspaceQueryService;
        private readonly IChannelQueryService _channelQueryService;

        public IdentityService(iChatContext context, IOptions<AppSettings> appSettings, IMapper mapper,
            IUserQueryService userQueryService, IWorkspaceQueryService workspaceService, IChannelQueryService channelQueryService)
        {
            _context = context;
            _mapper = mapper;
            _userQueryService = userQueryService;
            _workspaceQueryService = workspaceService;
            _appSettings = appSettings.Value;
            _channelQueryService = channelQueryService;
        }

        public async Task<UserProfileDto> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Invalid input.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            // check if email exists
            if (user == null || !user.VerifyPassword(password))
            {
                throw new Exception("Email or password is incorrect.");
            }

            // authentication successful
            return await GetProfileAsync(user);
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId, int workspaceId)
        {
            var user = await _userQueryService.GetUserByIdAsync(userId, workspaceId);

            if (user == null)
            {
                throw new Exception("Cannot find user profile.");
            }

            return await GetProfileAsync(user);
        }

        private string GenerateAccessToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<UserProfileDto> GetProfileAsync(User user)
        {
            var dto = _mapper.Map<UserProfileDto>(user);
            dto.WorkspaceName = (await _workspaceQueryService.GetWorkspaceByIdAsync(user.WorkspaceId))?.Name;
            dto.DefaultChannelId = await _channelQueryService.GetDefaultChannelGeneralIdAsync(user.WorkspaceId);
            dto.Token = GenerateAccessToken(user.Id);

            return dto;
        }
    }
}