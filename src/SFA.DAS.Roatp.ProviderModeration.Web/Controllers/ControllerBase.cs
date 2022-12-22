using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public abstract class ControllerBase : Controller
    {
        protected string UserId => User.FindFirstValue(ProviderClaims.UserId);
        protected string UserDisplayName => User.FindFirstValue(ProviderClaims.Givenname + " "+ ProviderClaims.Surname);
    }
}
