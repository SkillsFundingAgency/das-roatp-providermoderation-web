using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    [Authorize(Roles = Roles.RoatpTribalTeam)]
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
        [Route("providers/provider-description", Name = RouteNames.GetProviderDescription)]
        public IActionResult Index()
        {
            return View("~/Views/ProviderSearch/Index.cshtml");
        }

        [HttpPost]
        [Route("providers/provider-description", Name = RouteNames.PostProviderDescription)]
        public async Task<IActionResult> GetProviderDescription(ProviderSearchSubmitModel submitModel)
        {
            _logger.LogInformation("Provider description gathering for {ukprn}", submitModel.Ukprn);

            if (!ModelState.IsValid)
            {
                return View("~/Views/ProviderSearch/Index.cshtml", submitModel);
            }
            try
            {
                var providerSearchResult = await _mediator.Send(new GetProviderQuery(submitModel.Ukprn.GetValueOrDefault()));

                if (providerSearchResult != null && providerSearchResult.Provider.ProviderType != ProviderType.Main)
                {
                    ModelState.AddModelError("ProviderNotMainProvider", ProviderNotMainProvider);
                    return View("~/Views/ProviderSearch/Index.cshtml", submitModel);
                }
                var resultModel = (ProviderSearchResultViewModel)providerSearchResult;
                if(resultModel != null)
                {
                    resultModel.AddProviderDescriptionLink = Url.RouteUrl(RouteNames.GetAddProviderDescription, new { ukprn = submitModel.Ukprn });
                    resultModel.ChangeProviderDescriptionLink = Url.RouteUrl(RouteNames.GetAddProviderDescription, new { ukprn = submitModel.Ukprn });
                }
                return View("~/Views/ProviderSearch/SearchResults.cshtml", resultModel);

            }
            catch (InvalidOperationException)
            {
                _logger.LogError("Provider not found for ukprn {model.Ukprn}", submitModel.Ukprn);
                ModelState.AddModelError("ProviderSearch", ProviderNotAvailable);
                return View("~/Views/ProviderSearch/Index.cshtml", submitModel);
            }
        }
    }
}