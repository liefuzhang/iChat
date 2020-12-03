using iChat.Api.Constants;
using System;
using System.Collections.Generic;
using System.IO;

namespace iChat.Api.Models {
    public class User {
        protected User() { }

        public User(string email, string password, int workspaceId,
            string displayName, Guid identiconGuid) {
            if (string.IsNullOrWhiteSpace(email)) {
                throw new Exception("Email is required");
            }

            Email = email;
            Status = UserStatus.Active;
            DisplayName = (string.IsNullOrWhiteSpace(displayName) ? email : displayName);
            CreatedDate = DateTime.UtcNow;
            IdenticonGuid = identiconGuid;
            WorkspaceId = workspaceId;

            SetPassword(password);
        }

        public int Id { get; private set; }
        public string Email { get; private set; }
        public UserStatus Status { get; private set; }
        public string DisplayName { get; private set; }
        public string PhoneNumber { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public Guid IdenticonGuid { get; private set; }
        public int WorkspaceId { get; private set; }
        public Workspace Workspace { get; private set; }
        public ICollection<ChannelSubscription> ChannelSubscriptions { get; private set; }
        public ICollection<ConversationUser> ConversationUsers { get; private set; }
        public string IdenticonPath => @"https://ichat-apis.herokuapp.com/" +
            Path.Combine(iChatConstants.IdenticonPath, $"{IdenticonGuid}{iChatConstants.IdenticonExt}");

        public void SetPassword(string password) {
            if (string.IsNullOrWhiteSpace(password)) {
                throw new Exception("Password is required");
            }

            if (password.Length < 6) {
                throw new Exception("Password needs to have at least 6 characters");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password) {
            if (password == null) {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (PasswordHash.Length != 64) {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (PasswordSalt.Length != 128) {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != PasswordHash[i]) {
                        return false;
                    }
                }
            }

            return true;
        }

        public void SetStatus(UserStatus status) {
            if (!Enum.IsDefined(typeof(UserStatus), status)) {
                throw new Exception("Invalid status");
            }

            Status = status;
        }

        public void UpdateDetails(string displayName, string phoneNumber) {
            DisplayName = displayName;
            PhoneNumber = phoneNumber;
        }
    }
}
