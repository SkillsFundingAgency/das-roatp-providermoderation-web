using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class ProviderClaims
    {
        public static readonly string UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        public static readonly string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public static readonly string Givenname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
    }
}
