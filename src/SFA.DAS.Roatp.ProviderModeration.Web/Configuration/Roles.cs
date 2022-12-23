using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class Roles
    {
        public const string RoleClaimType = "http://service/service";

        public const string RoatpTribalTeam = "TAD";

        public static bool HasValidRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(RoatpTribalTeam);
        }
    }
}
