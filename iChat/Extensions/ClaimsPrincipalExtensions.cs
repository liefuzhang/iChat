using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iChat.Extensions
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
