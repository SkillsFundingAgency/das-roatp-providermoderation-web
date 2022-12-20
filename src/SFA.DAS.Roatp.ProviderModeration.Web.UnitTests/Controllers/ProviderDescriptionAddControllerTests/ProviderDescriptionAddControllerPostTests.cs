using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.ProviderDescriptionAddControllerTests
{
    [TestFixture]
    public class ProviderDescriptionAddControllerPostTests
    {
        private Mock<IMediator> _mediatorMock;
        private ProviderDescriptionAddController _sut;
        private Mock<IUrlHelper> _urlHelperMock;
        string verifyUrl = "http://test";
        public const int Ukprn = 12345678;
        public const string LegalName = "TestLegalName";
        public const string ProviderDescription = "TestProviderDescription";

        [SetUp]
        public void Before_Each_Test()
        {
            _mediatorMock = new Mock<IMediator>();

            _urlHelperMock = new Mock<IUrlHelper>();

            _urlHelperMock
               .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName.Equals(RouteNames.GetProviderDescription))))
               .Returns(verifyUrl);

            _urlHelperMock
               .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName.Equals(RouteNames.GetProviderDetails))))
               .Returns(verifyUrl);

            _sut = new ProviderDescriptionAddController(_mediatorMock.Object, Mock.Of<ILogger<ProviderDescriptionAddController>>());
            _sut.Url = _urlHelperMock.Object;
        }

        [Test]
        public void ProviderDescriptionAddController_AddProviderDescription_ValidResponseRedirectToRoute()
        {
            var submitModel = new ProviderDescriptionAddSubmitModel
            {
                Ukprn = Ukprn,
                LegalName = LegalName,
                ProviderDescription = ProviderDescription
            };

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ProviderDescription"] = submitModel.ProviderDescription;
            _sut.TempData = tempData;

            var result = _sut.AddProviderDescription(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult.RouteName.Should().Be(RouteNames.GetReviewProviderDescription);
        }

        [Test]
        public void ProviderDescriptionAddController_AddProviderDescription_ModelStateErrorReturnSameView()
        {
            _sut.ModelState.AddModelError("ProviderDescription", "ErrorMessageEmptyString");

            var submitModel = new ProviderDescriptionAddSubmitModel
            {
                Ukprn = Ukprn,
                LegalName = LegalName,
                ProviderDescription = string.Empty
            };
            var result = _sut.AddProviderDescription(submitModel);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain(ProviderDescriptionAddController.ViewPath);
            var model = viewResult.Model as ProviderDescriptionAddViewModel;
            model.Should().NotBeNull();
            model.Ukprn.Should().Be(submitModel.Ukprn);
            model.LegalName.Should().Be(submitModel.LegalName);
            model.ProviderDescription.Should().Be(submitModel.ProviderDescription);
            model.CancelLink.Should().Be(verifyUrl);
        }
    }
}
