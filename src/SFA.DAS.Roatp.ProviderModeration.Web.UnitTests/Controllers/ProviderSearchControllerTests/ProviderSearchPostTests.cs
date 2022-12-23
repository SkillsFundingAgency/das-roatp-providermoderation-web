using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.ProviderSearchControllerTests
{
    [TestFixture]
    public class ProviderSearchPostTests
    {
        private ProviderSearchController _controller;
        private Mock<ILogger<ProviderSearchController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IUrlHelper> _urlHelperMock;

        public const int Ukprn = 12345678;
        public const string MarketingInfo = "Marketing info";
        string verifyAddProviderDescriptionUrl = "http://test-AddProviderDescriptionUrl";

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<ProviderSearchController>>();
            var provider = new GetProviderResponse
            {
                MarketingInfo = MarketingInfo,
                ProviderType = ProviderType.Main
            };
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new GetProviderQueryResult
                {
                   Provider = provider
                });

            _urlHelperMock = new Mock<IUrlHelper>();

            _urlHelperMock
               .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName.Equals(RouteNames.GetAddProviderDescription))))
               .Returns(verifyAddProviderDescriptionUrl);

            _controller = new ProviderSearchController(_mediator.Object, _logger.Object);
            _controller.Url = _urlHelperMock.Object;
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_ReturnsValidResponse()
        {
            var submitModel = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };


           var result = await _controller.GetProviderDescription(submitModel);
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.ViewName.Should().Contain("ProviderSearch/SearchResults.cshtml");
            viewResult?.Model.Should().NotBeNull();
            var model = viewResult.Model as ProviderSearchResultViewModel;
            model.Should().NotBeNull();
            model.AddProviderDescriptionLink.Should().Be(verifyAddProviderDescriptionUrl);
            model.ChangeProviderDescriptionLink.Should().Be(verifyAddProviderDescriptionUrl);
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_ReturnsNonValidResponse()
        {
            var provider = new GetProviderResponse
            {
                MarketingInfo = MarketingInfo,
                ProviderType = ProviderType.Supporting
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new GetProviderQueryResult
                {
                    Provider = provider
                });

            _controller = new ProviderSearchController(_mediator.Object, _logger.Object);

            var model = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };

            var result = await _controller.GetProviderDescription(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.ViewName.Should().Contain("ProviderSearch/Index.cshtml");
            viewResult?.Model.Should().NotBeNull();
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_ModelStateErrorReturnSameView()
        {
            _controller.ModelState.AddModelError("ProviderNotMainProvider", "ErrorMessage");

            var model = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };
            var result = await _controller.GetProviderDescription(model);

            var viewResult = result as ViewResult;
            viewResult?.Should().NotBeNull();
            viewResult?.ViewName.Should().Contain("ProviderSearch/Index.cshtml");
            viewResult?.Model.Should().NotBeNull();

            _mediator.Verify(m => m.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_InvalidOperationExceptionWhenNoProvider()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            _controller = new ProviderSearchController(_mediator.Object, _logger.Object);

            var model = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };
            var result = await _controller.GetProviderDescription(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.ViewName.Should().Contain("ProviderSearch/Index.cshtml");
            viewResult?.Model.Should().NotBeNull();

            _mediator.Verify(m => m.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
