using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    //[Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderSearchResultsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderSearchResultsController> _logger;
        public const string ProviderNotAvailable = "no provider has been found";
        public const string ProviderNotMainProvider = "this provider is not valid";
        public ProviderSearchResultsController(IMediator mediator, ILogger<ProviderSearchResultsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("providers/{ukprn}", Name = RouteNames.GetProviderDetails)]
        public async Task<IActionResult> GetProvider([FromRoute] int ukprn)
        {
            _logger.LogInformation("Provider description gathering for {ukprn}", ukprn);
            var providerSearchResult = await _mediator.Send(new GetProviderQuery(ukprn));
            var resultModel = (ProviderSearchResultViewModel)providerSearchResult;
            if (resultModel != null)
            {
                resultModel.AddProviderDescriptionLink = Url.RouteUrl(RouteNames.GetAddProviderDescription, new { ukprn = ukprn });
                resultModel.ChangeProviderDescriptionLink = Url.RouteUrl(RouteNames.GetAddProviderDescription, new { ukprn = ukprn });
            }
            return View("~/Views/ProviderSearch/SearchResults.cshtml", resultModel);
        }
    }
}