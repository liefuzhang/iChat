using System;
using System.Collections.Generic;

namespace iChat.Api.Models {
    public class ResetPasswordRequset {
        protected ResetPasswordRequset() { }

        public ResetPasswordRequset(string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                throw new Exception("Email cannot be empty");
            }

            Email = email;
            ResetCode = Guid.NewGuid();
        }

        public int Id { get; private set; }
        public string Email { get; private set; }
        public bool Resetted { get; private set; }
        public bool Cancelled { get; private set; }
        public Guid ResetCode { get; private set; }

        public void Process() {
            Resetted = true;
        }

        public void Cancel() {
            Cancelled = true;
        }
    }
}
