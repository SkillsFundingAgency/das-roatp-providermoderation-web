using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationConfiguration _applicationConfiguration;

    public HomeController(ApplicationConfiguration applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    public IActionResult Dashboard()
    {
        return Redirect(_applicationConfiguration.EsfaAdminServicesBaseUrl + "/dashboard");
    }
}
