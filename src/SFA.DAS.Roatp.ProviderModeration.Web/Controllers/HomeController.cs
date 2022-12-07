using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers;

//[Authorize(Roles = Roles.RoatpTribalTeam)]
public class HomeController : Controller
{
    private readonly ApplicationConfiguration _applicationConfiguration;
    
    public HomeController(IOptions<ApplicationConfiguration> applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public IActionResult Index()
    {
        return RedirectToRoute(RouteNames.GetProviderDescription);
    }


    [Route("/Dashboard")]
    public IActionResult Dashboard()
    {
        return Redirect(_applicationConfiguration.EsfaAdminServicesBaseUrl + "/dashboard");
    }
}
