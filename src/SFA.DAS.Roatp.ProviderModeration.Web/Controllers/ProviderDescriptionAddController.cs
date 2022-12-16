using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    //[Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderDescriptionAddController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderDescriptionAddController> _logger;
        public const string ViewPath = "~/Views/ProviderSearch/ProviderDescriptionAdd.cshtml";
        public ProviderDescriptionAddController(IMediator mediator, ILogger<ProviderDescriptionAddController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("providers/{ukprn}/add-provider-description", Name = RouteNames.GetAddProviderDescription)]
        public async Task<IActionResult> Index([FromRoute] int ukprn)
        {
            var providerSearchResult = await _mediator.Send(new GetProviderQuery(ukprn));
            var providerDescriptionAddViewModel = new ProviderDescriptionAddViewModel
            {
                Ukprn = ukprn,
                LegalName = providerSearchResult.Provider.LegalName,
                CancelLink = Url.RouteUrl(RouteNames.GetProviderDescription)
            };
            return View(ViewPath, providerDescriptionAddViewModel);
        }

        [HttpPost]
        [Route("providers/{ukprn}/add-provider-description", Name = RouteNames.PostAddProviderDescription)]
        public IActionResult AddProviderDescription(ProviderDescriptionAddSubmitModel submitModel)
        {
            _logger.LogInformation("Provider description gathering for {ukprn}", submitModel.Ukprn);

            if (!ModelState.IsValid)
            {
                var model = new ProviderDescriptionAddViewModel()
                {
                    Ukprn = submitModel.Ukprn,
                    LegalName = submitModel.LegalName,
                    ProviderDescription = submitModel.ProviderDescription,
                    CancelLink = Url.RouteUrl(RouteNames.GetProviderDescription)
                };
                return View(ViewPath, model);
            }
            return View(ViewPath, new ProviderDescriptionAddViewModel { LegalName = submitModel.LegalName , ProviderDescription  = submitModel.ProviderDescription });
        }
    }
}