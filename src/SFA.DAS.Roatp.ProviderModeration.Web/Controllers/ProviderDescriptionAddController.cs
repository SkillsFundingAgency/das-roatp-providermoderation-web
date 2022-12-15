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
    //[Authorize(Roles = Roles.RoatpTribalTeam)]
    public class ProviderDescriptionAddController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderDescriptionAddController> _logger;
        public const string ProviderNotAvailable = "no provider has been found";
        public const string ProviderNotMainProvider = "this provider is not valid";
        public ProviderDescriptionAddController(IMediator mediator, ILogger<ProviderDescriptionAddController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("providers/add-provider-description", Name = RouteNames.GetAddProviderDescription)]
        public IActionResult Index()
        {
            var providerDescriptionAddViewModel = new ProviderDescriptionAddViewModel();
            providerDescriptionAddViewModel.LegalName = "Test";
            return View("~/Views/ProviderSearch/ProviderDescriptionAdd.cshtml", providerDescriptionAddViewModel);
        }

        [HttpPost]
        [Route("providers/add-provider-description", Name = RouteNames.PostAddProviderDescription)]
        public IActionResult AddProviderDescription(ProviderDescriptionAddSubmitModel model)
        {
            _logger.LogInformation("Provider description gathering for {ukprn}", model.Ukprn);

            if (!ModelState.IsValid)
            {
                return View("~/Views/ProviderSearch/Index.cshtml", model);
            }
            return View();
        }
    }
}