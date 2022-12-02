using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Controllers
{
    public class ProviderSearchController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderSearchController> _logger;

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
            _logger.LogInformation("Provider description gathering for {ukprn}", model.UKPRN);

            if (!ModelState.IsValid)
            {
                return View("~/Views/ProviderSearch/Index.cshtml", model);
            }

            var result = await _mediator.Send(new GetProviderQuery(model.UKPRN));

            return View("~/Views/ProviderSearch/Index.cshtml", model);
        }
    }
}