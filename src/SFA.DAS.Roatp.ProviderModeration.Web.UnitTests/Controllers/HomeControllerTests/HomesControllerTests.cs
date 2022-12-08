using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _sut;
        private Mock<IOptions<ApplicationConfiguration>> _mockOptions;
        private ApplicationConfiguration _config;

        [SetUp]
        public void Before_each_test()
        {
            _mockOptions = new Mock<IOptions<ApplicationConfiguration>>();
            _config = new ApplicationConfiguration() { EsfaAdminServicesBaseUrl = @"https://test.com" };
            _mockOptions.Setup(o => o.Value).Returns(_config);
            _sut = new HomeController(_mockOptions.Object);
        }

        [Test]
        public void Index_RedirectToGetProviderDescriptionRoute()
        {
            var result = _sut.Index() as RedirectToRouteResult;

            result.Should().NotBeNull();
            result?.RouteName.Should().Be(RouteNames.GetProviderDescription);
        }

        [Test]
        public void Dashboard_RedirectToAdminServicesBaseUrl()
        {
            var result = _sut.Dashboard() as RedirectResult;

            result.Should().NotBeNull();
            result?.Url.Contains("/dashboard");
        }
    }
}
