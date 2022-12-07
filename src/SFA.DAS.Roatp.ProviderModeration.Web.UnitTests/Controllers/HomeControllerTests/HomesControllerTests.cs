using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController sut;
        private Mock<IOptions<ApplicationConfiguration>> mockOptions;
        private ApplicationConfiguration config;

        [SetUp]
        public void Before_each_test()
        {
            mockOptions = new Mock<IOptions<ApplicationConfiguration>>();
            config = new ApplicationConfiguration() { EsfaAdminServicesBaseUrl = @"https://test.com" };
            mockOptions.Setup(o => o.Value).Returns(config);
            sut = new HomeController(mockOptions.Object);
        }

        [Test]
        public void Index_RedirectToAction()
        {
            var result = sut.Index() as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("ProviderSearch");
        }

        [Test]
        public void Dashboard_RedirectToAdminServicesBaseUrl()
        {
            var result = sut.Dashboard() as RedirectResult;

            result.Should().NotBeNull();
            result.Url.Contains("/dashboard");
        }
    }
}
