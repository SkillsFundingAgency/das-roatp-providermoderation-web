﻿using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.ProviderSearchControllerTests
{
    [TestFixture]
    public class ProviderSearchGetTests
    {
        private ProviderSearchController _controller;
        private Mock<ILogger<ProviderSearchController>> _logger;
        private Mock<IMediator> _mediator;

        public const int Ukprn = 12345678;
        public const string MarketingInfo = "Marketing info";
        

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<ProviderSearchController>>();
            var provider = new Provider
            {
                MarketingInfo = MarketingInfo,
                ProviderType = ProviderType.MainProvider
            };


            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new GetProviderQueryResult
                {
                   Provider = provider
                });

            _controller = new ProviderSearchController(_mediator.Object, _logger.Object);
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_ReturnsValidResponse()
        {
            var model = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };
            var result = await _controller.GetProviderDescription(model);
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ProviderSearch/Index.cshtml");
            viewResult.Model.Should().NotBeNull();
        }

        [Test]
        public async Task ProviderController_GetProviderDescription_ModelStateErrorReturnSameView()
        {
            _controller.ModelState.AddModelError("ProviderNotMainProvider", "ErrorMessage");

            _mediator.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetProviderQueryResult)null);

            var model = new ProviderSearchSubmitModel
            {
                Ukprn = Ukprn
            };
            var result = await _controller.GetProviderDescription(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ProviderSearch/Index.cshtml");
            viewResult.Model.Should().NotBeNull();
        }
    }
}
