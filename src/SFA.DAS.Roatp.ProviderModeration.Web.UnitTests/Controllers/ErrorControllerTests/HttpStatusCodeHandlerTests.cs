using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.TestHelpers;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.ErrorControllerTests;
[TestFixture]
public class HttpStatusCodeHandlerTests
{
    const string PageNotFoundViewName = "PageNotFound";
    const string ErrorInServiceViewName = "ErrorInService";
    private readonly static string GetProviderDescriptionUrl = Guid.NewGuid().ToString();

    [TestCase(403, PageNotFoundViewName)]
    [TestCase(404, PageNotFoundViewName)]
    [TestCase(500, ErrorInServiceViewName)]
    public void HttpStatusCodeHandler_ReturnsRespectiveView(int statusCode, string expectedViewName)
    {
        // Arrange
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.GetProviderDescription, GetProviderDescriptionUrl);

        // Act
        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(statusCode);

        // Assert
        result.ViewName.Should().Be(expectedViewName);
    }

    [Test]
    public void HttpStatusCodeHandler_InternalServerError_ReturnsRespectiveView()
    {
        // Arrange
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.GetProviderDescription, GetProviderDescriptionUrl);

        // Act
        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(500);
        var viewModel = result!.Model as ErrorViewModel;

        // Assert
        Assert.That(viewModel, Is.InstanceOf<ErrorViewModel>());
    }

    [Test]
    public void HttpStatusCodeHandler_InternalServerError_ShouldReturnExpectedValue()
    {
        // Arrange
        var sut = new ErrorController(Mock.Of<ILogger<ErrorController>>());
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.GetProviderDescription, GetProviderDescriptionUrl);

        // Act
        ViewResult result = (ViewResult)sut.HttpStatusCodeHandler(500);
        var viewModel = result!.Model as ErrorViewModel;

        // Assert
        Assert.That(viewModel!.HomePageUrl, Is.EqualTo(GetProviderDescriptionUrl));
    }
}
