using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    //[Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderSearchController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderSearchController> _logger;
        public const string ProviderNotAvailable = "no provider has been found";
        public const string ProviderNotMainProvider = "this provider is not valid";
        public ProviderSearchController(IMediator mediator, ILogger<ProviderSearchController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[Route("providers/{ukprn}/provider-description", Name = RouteNames.GetProviderDescription)]
        public async Task<IActionResult> GetProviderDescription(ProviderSearchSubmitModel model)
        {
            _logger.LogInformation("Provider description gathering for {ukprn}", model.Ukprn);

            if (!ModelState.IsValid)
            {
                return View("~/Views/ProviderSearch/Index.cshtml", model);
            }
            try
            {
                var providerResult = await _mediator.Send(new GetProviderQuery(model.Ukprn.GetValueOrDefault()));

                if (providerResult != null && providerResult.Provider.ProviderType != ProviderType.MainProvider)
                {
                    ModelState.AddModelError("ProviderNotMainProvider", ProviderNotMainProvider);
                    return View("~/Views/ProviderSearch/Index.cshtml", model);
                }
            }
            catch(InvalidOperationException ex) 
            {
                _logger.LogError("Provider not found for ukprn {model.Ukprn}", model.Ukprn);
                ModelState.AddModelError("ProviderSearch", ProviderNotAvailable);
                return View("~/Views/ProviderSearch/Index.cshtml", model);
            }
            return View("~/Views/ProviderSearch/Index.cshtml", model);
        }
    }
}