using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_RedirectToAction()
        {
            var mockOptions = new Mock<IOptions<ApplicationConfiguration>>();
            var config = new ApplicationConfiguration() { EsfaAdminServicesBaseUrl = @"https://test.com" };
            mockOptions.Setup(o => o.Value).Returns(config);
            var sut = new HomeController(mockOptions.Object);

            var result = sut.Index() as RedirectToActionResult;
            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("ProviderSearch");
        }

        [Test]
        public void Dashboard_RedirectToAdminServicesBaseUrl()
        {
            var mockOptions = new Mock<IOptions<ApplicationConfiguration>>();
            var config = new ApplicationConfiguration() { EsfaAdminServicesBaseUrl = @"https://test.com" };
            mockOptions.Setup(o => o.Value).Returns(config);
            var sut = new HomeController(mockOptions.Object);

            var result = sut.Dashboard() as RedirectResult;
            result.Should().NotBeNull();
            result.Url.Contains("/dashboard");
        }
    }
}
