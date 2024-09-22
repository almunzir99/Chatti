using System.Security.Claims;
using System.Security.Principal;

namespace Chatti.Api.Extensions
{

    public static class IdentityExtension
    {
        public static string GetClaimValue(this IPrincipal user, string claim)
        {
            var claimIdentity = user.Identity as ClaimsIdentity;
            var value = claimIdentity?.Claims
                .SingleOrDefault(c => c.Type.Equals(claim));
            if (value == null)
                return "0";
            return value.Value;
        }
    }
}
