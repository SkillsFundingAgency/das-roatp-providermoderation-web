using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public abstract class ControllerBase : Controller
    {
        protected string UserId => User.FindFirstValue(ProviderClaims.UserId);
        protected string UserDisplayName => string.Concat(User.FindFirstValue(ProviderClaims.Givenname), " ", User.FindFirstValue(ProviderClaims.Surname));
    }
}
