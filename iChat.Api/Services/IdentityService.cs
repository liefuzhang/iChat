using iChat.Api.Constants;
using iChat.Api.Helpers;
using iChat.Api.Models;
using iChat.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace iChat.Api.Services {
    public class IdentityService : IIdentityService {
        private readonly iChatContext _context;
        private readonly AppSettings _appSettings;

        public IdentityService(iChatContext context, IOptions<AppSettings> appSettings) {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<User> AuthenticateAsync(string email, string password) {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                return null;
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            // check if email exists
            if (user == null) {
                return null;
            }

            if (!user.VerifyPassword(password)) {
                return null;
            }

            // authentication successful
            return user;
        }
        
        public string GenerateAccessToken(int userId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}