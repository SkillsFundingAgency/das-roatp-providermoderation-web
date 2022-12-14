using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Providers.Commands.UpdateProviderDescription;
using SFA.DAS.Roatp.ProviderModeration.Application.Providers.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    [Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderDescriptionReviewController : ControllerBase
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
        [Route("providers/{ukprn}/review-provider-description/{mode}", Name = RouteNames.GetReviewProviderDescription)]
        public async Task<IActionResult> Index([FromRoute] int ukprn, [FromRoute] ProviderDescriptionMode mode)
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
                EditEntry = Url.RouteUrl(RouteNames.GetReviewProviderDescriptionEdit, new { ukprn, mode }),
            };
            return View(ViewPath, providerDescriptionReviewViewModel);
        }

        [HttpGet]
        [Route("providers/{ukprn}/review-provider-description-edit/{mode}", Name = RouteNames.GetReviewProviderDescriptionEdit)]
        public IActionResult EditEntry([FromRoute] int ukprn, [FromRoute] ProviderDescriptionMode mode)
        {
            TempData.Keep("ProviderDescription");
            return mode == ProviderDescriptionMode.Add
                ? RedirectToRoute(RouteNames.GetAddProviderDescription, new { ukprn })
                : (IActionResult)RedirectToRoute(RouteNames.GetUpdateProviderDescription, new { ukprn });
        }

        [HttpPost]
        [Route("providers/{ukprn}/review-provider-description/{mode}", Name = RouteNames.PostReviewProviderDescription)]
        public async Task<IActionResult> ReviewProviderDescription(ProviderDescriptionReviewViewModel submitModel, [FromRoute] ProviderDescriptionMode mode)
        {
            _logger.LogInformation("Provider description updating for {ukprn}", submitModel.Ukprn);
            TempData.Remove("ProviderDescription");
            
            var command = new UpdateProviderDescriptionCommand
            {
                Ukprn = submitModel.Ukprn,
                UserId = UserId,
                UserDisplayName = UserDisplayName,
                ProviderDescription = submitModel.ProviderDescription
            };

            await _mediator.Send(command);

            return RedirectToRoute(RouteNames.GetProviderDetails, new { submitModel.Ukprn });
        }
    }
}