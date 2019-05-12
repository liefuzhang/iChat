using System;
using System.Linq;
using iChat.Api.Models;
using iChat.Data;

namespace iChat.Api.Services {
    public class IdentityService : IIdentityService {
        private readonly iChatContext _context;

        public IdentityService(iChatContext context) {
            _context = context;
        }

        public User Authenticate(string email, string password) {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            // check if email exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public int Register(string email, string password, int workspaceId) {
            ValidateUserEmailAndPassword(email, password);
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            var user = new User {
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = DateTime.Now,
                WorkspaceId = workspaceId,
                DisplayName = email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public void ValidateUserEmailAndPassword(string email, string password) {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required");

            if (password.Length < 6)
                throw new Exception("Password needs to have at least 6 characters");

            if (_context.Users.Any(u => u.Email == email))
                throw new Exception($"Email \"{email}\" is already taken");
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt) {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}