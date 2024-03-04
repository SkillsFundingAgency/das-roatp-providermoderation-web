using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Controllers;
using SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure;
using SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.TestHelpers;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Controllers.ErrorControllerTests;
[TestFixture]
public class ErrorInServiceTests
{
    private const string upn = "test";
    private readonly Mock<HttpContext> httpContextMock = new();
    private readonly InMemoryFakeLogger<ErrorController> loggerFake = new();
    private readonly Exception exception = new("Something went wrong");
    private const string path = "/providers/10012002";
    private readonly static string GetProviderDescriptionUrl = Guid.NewGuid().ToString();
    private ErrorController sut;

    [SetUp]
    public void Before_Each_Test()
    {
        var featuresMock = new Mock<IFeatureCollection>();

        featuresMock.Setup(f => f.Get<IExceptionHandlerPathFeature>())
            .Returns(new ExceptionHandlerFeature
            {
                Path = path,
                Error = exception
            });
        httpContextMock.Setup(p => p.Features).Returns(featuresMock.Object);

        sut = new ErrorController(loggerFake)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object,
            }
        };
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.GetProviderDescription, GetProviderDescriptionUrl);
    }

    [Test]
    public void ErrorInService_UserIsAuthenticatedLogErrorAndReturnsErrorInServiceView()
    {
        var authorisedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Upn, upn)
        }, "mock"));
        httpContextMock.Setup(c => c.User).Returns(authorisedUser);

        var result = (ViewResult)sut.ErrorInService();

        result.Should().NotBeNull();
        loggerFake.Message.Contains(upn).Should().BeTrue();
        loggerFake.Message.Contains(path).Should().BeTrue();
    }

    [Test]
    public void ErrorInService_UserIsNotAuthenticated_LogErrorAndReturnsErrorInServiceView()
    {
        var unauthorisedUser = new ClaimsPrincipal(new ClaimsIdentity());
        httpContextMock.Setup(c => c.User).Returns(unauthorisedUser);

        var sut = new ErrorController(loggerFake)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object,
            }
        };
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.GetProviderDescription, GetProviderDescriptionUrl);

        var result = (ViewResult)sut.ErrorInService();

        result.Should().NotBeNull();
        loggerFake.Message.Contains(upn).Should().BeFalse();
        loggerFake.Message.Contains(path).Should().BeTrue();
    }

    [TearDown]
    public void TearDown()
    {
        sut?.Dispose();
    }

    public class InMemoryFakeLogger<T> : ILogger<T>
    {
        public LogLevel Level { get; private set; }
        public Exception Ex { get; private set; } = null!;
        public string Message { get; private set; } = null!;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

#nullable enable
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Level = logLevel;
            Message = state!.ToString()!;
            Ex = exception!;
        }

        public class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new NullScope();

            private NullScope()
            {
            }

            void IDisposable.Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
