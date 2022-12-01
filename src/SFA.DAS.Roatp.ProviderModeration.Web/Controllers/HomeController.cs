using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationConfiguration _applicationConfiguration;
    
    public HomeController(IOptions<ApplicationConfiguration> applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "ProviderSearch");
    }

    [Route("/Dashboard")]
    public IActionResult Dashboard()
    {
        return Redirect(_applicationConfiguration.EsfaAdminServicesBaseUrl + "/dashboard");
    }
}
