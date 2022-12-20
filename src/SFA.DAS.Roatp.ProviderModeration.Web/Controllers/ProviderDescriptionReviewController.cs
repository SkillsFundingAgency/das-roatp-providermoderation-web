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
        public const string ViewPath = "~/Views/ProviderSearch/ProviderDescriptionReview.cshtml";
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

            var providerDescriptionTempData = (string)TempData["ProviderDescription"];
            TempData.Keep("ProviderDescription");
            var providerDescriptionReviewViewModel = new ProviderDescriptionReviewViewModel
            {
                Ukprn = ukprn,
                LegalName = providerSearchResult.Provider.LegalName,
                ProviderDescription = providerDescriptionTempData,
                CancelLink = Url.RouteUrl(RouteNames.GetProviderDetails, new { ukprn }),
                EditEntry = Url.RouteUrl(RouteNames.GetReviewProviderDescriptionEdit, new { ukprn }),
                
            };
            return View(ViewPath, providerDescriptionReviewViewModel);
        }

        [HttpGet]
        [Route("providers/{ukprn}/review-provider-description-edit", Name = RouteNames.GetReviewProviderDescriptionEdit)]
        public IActionResult EditEntry([FromRoute] int ukprn)
        {
            TempData.Keep("ProviderDescription");
            return RedirectToRoute(RouteNames.GetAddProviderDescription, new { ukprn });
        }

        [HttpPost]
        [Route("providers/{ukprn}/review-provider-description", Name = RouteNames.PostReviewProviderDescription)]
        public IActionResult ReviewProviderDescription(ProviderDescriptionReviewViewModel submitModel)
        {
            _logger.LogInformation("Provider description updating for {ukprn}", submitModel.Ukprn);
            TempData.Keep("ProviderDescription");
            var resultModel = new ProviderDescriptionReviewViewModel()
            {
                Ukprn = submitModel.Ukprn,
                LegalName = submitModel.LegalName,
                ProviderDescription = submitModel.ProviderDescription,
                CancelLink = Url.RouteUrl(RouteNames.GetProviderDetails, new { submitModel.Ukprn }),
                EditEntry = Url.RouteUrl(RouteNames.GetReviewProviderDescriptionEdit, new
                {
                    ukprn = submitModel.Ukprn,
                })
            };
            return View(ViewPath, resultModel);
        }
    }
}