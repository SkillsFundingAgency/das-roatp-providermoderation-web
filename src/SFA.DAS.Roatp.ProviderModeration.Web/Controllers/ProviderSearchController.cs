using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    public class ProviderSearchController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}