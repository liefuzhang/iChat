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
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IdentityService(iChatContext context, IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory, IHostingEnvironment hostingEnvironment) {
            _context = context;
            _appSettings = appSettings.Value;
            _clientFactory = clientFactory;
            _hostingEnvironment = hostingEnvironment;
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

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
                return null;
            }

            // authentication successful
            return user;
        }

        public async Task<int> RegisterAsync(string email, string password, int workspaceId, string name = null) {
            await ValidateUserEmailAndPasswordAsync(email, password);
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            var identiconGuid = await GenerateUserIdenticon(email);

            var user = new User {
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = DateTime.Now,
                WorkspaceId = workspaceId,
                DisplayName = name ?? email,
                IdenticonGuid = identiconGuid
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        private async Task<Guid> GenerateUserIdenticon(string email) {
            var identiconGuid = Guid.NewGuid();
            var identiconName = $"{identiconGuid}{iChatConstants.IdenticonExt}";
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, iChatConstants.IdenticonPath,
                identiconName);
            var svgContent = string.Empty;

            try {
                var request = new HttpRequestMessage(HttpMethod.Get,
                        $"https://avatars.dicebear.com/v2/identicon/{identiconName}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode) {
                    svgContent = await response.Content.ReadAsStringAsync();
                } else {
                    throw new Exception("Error getting user icon");
                }
            } catch (Exception) {
                var defaultSvgPath = Path.Combine(_hostingEnvironment.WebRootPath, iChatConstants.IdenticonPath,
                    iChatConstants.DefaultIdenticonName);
                svgContent = File.ReadAllText(defaultSvgPath);
            }

            File.WriteAllText(filePath, svgContent);

            return identiconGuid;
        }

        public async Task ValidateUserEmailAndPasswordAsync(string email, string password) {
            if (string.IsNullOrWhiteSpace(password)) {
                throw new Exception("Password is required");
            }

            if (string.IsNullOrWhiteSpace(email)) {
                throw new Exception("Email is required");
            }

            if (password.Length < 6) {
                throw new Exception("Password needs to have at least 6 characters");
            }

            if (await _context.Users.AnyAsync(u => u.Email == email)) {
                throw new Exception($"Email \"{email}\" is already taken");
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt) {
            if (password == null) {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64) {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128) {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i]) {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            if (password == null) {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string GenerateAccessToken(int userId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}