using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    //[Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderDescriptionReviewController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderDescriptionReviewController> _logger;
        public const string ViewPath = "~/Views/ProviderSearch/ProviderDescriptionAdd.cshtml";
        public ProviderDescriptionReviewController(IMediator mediator, ILogger<ProviderDescriptionReviewController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("providers/{ukprn}/review-provider-description", Name = RouteNames.GetReviewProviderDescription)]
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

        [HttpGet]
        //[Route("providers/{ukprn}/review-provider-description-edit", Name = RouteNames.GetReviewProviderDescriptionEdit)]
        public IActionResult EditEntry()
        {
            var model = TempData["ProviderDescription"] as ProviderDescriptionReviewViewModel;
            //var providerSearchResult = await _mediator.Send(new GetProviderQuery(ukprn));
            var providerDescriptionAddViewModel = new ProviderDescriptionAddViewModel
            {
                Ukprn = model.Ukprn,
                LegalName = model.LegalName,
                ProviderDescription = model.ProviderDescription,
                CancelLink = Url.RouteUrl(RouteNames.GetProviderDescription)
            };
            return View("~/Views/ProviderSearch/ProviderDescriptionAdd.cshtml", providerDescriptionAddViewModel);
        }

        [HttpPost]
        [Route("providers/{ukprn}/review-provider-description", Name = RouteNames.PostReviewProviderDescription)]
        public IActionResult ReviewProviderDescription(ProviderDescriptionReviewViewModel submitModel)
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
                 var resultModel = new ProviderDescriptionReviewViewModel()
                 {
                     Ukprn = submitModel.Ukprn,
                     LegalName = submitModel.LegalName,
                     ProviderDescription = submitModel.ProviderDescription,
                     EditEntry = Url.RouteUrl(RouteNames.GetAddProviderDescription,new
                     {
                         ukprn = submitModel.Ukprn,
                     })
                 };
            return View("~/Views/ProviderSearch/ProviderDescriptionReview.cshtml", resultModel);
        }
    }
}