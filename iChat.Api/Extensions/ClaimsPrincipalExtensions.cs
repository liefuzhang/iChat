using System;
using System.Security.Claims;

namespace iChat.Api.Extensions
{
    public static class ClaimsPrincipalExtensions {
        public static int GetUserId(this ClaimsPrincipal principal) {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(id);
        }
    }
}
