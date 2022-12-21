using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Application.Providers.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
using SFA.DAS.Roatp.ProviderModeration.Domain.Interfaces;

namespace SFA.DAS.Roatp.ProviderModeration.Application.UnitTests.Providers.Queries
{
    [TestFixture]
    public class GetProviderHandlerTests
    {
        private GetProviderQueryHandler _handler;
        private Mock<IApiClient> _apiClient;
        private GetProviderQuery _query;
        private readonly GetProviderResponse _provider = new();
        private const int Ukprn = 12345678;
        private const string MarketingInfo = "Marketing info";

        [SetUp]
        public void Setup()
        {
            _query = new GetProviderQuery(Ukprn);
            _apiClient = new Mock<IApiClient>();
            _handler = new GetProviderQueryHandler(_apiClient.Object, Mock.Of<ILogger<GetProviderQueryHandler>>());
        }

        [Test]
        public async Task Handle_ValidApiRequest_ReturnsValidResponse()
        {
            _provider.MarketingInfo = MarketingInfo;
            _provider.ProviderType = ProviderType.Main;
            _apiClient.Setup(x => x.Get<GetProviderResponse>($"providers/{_query.Ukprn}")).ReturnsAsync(_provider);
            var result = await _handler.Handle(_query, CancellationToken.None);
            result.Should().NotBeNull();
            result.Provider.Should().BeEquivalentTo(_provider);
        }

        [Test]
        public void Handle_InvalidApiResponse_ThrowsException()
        {
            _apiClient.Setup(x => x.Get<GetProviderResponse>($"providers/{_query.Ukprn}")).ReturnsAsync((GetProviderResponse)null);

            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_query, CancellationToken.None));
        }
    }
}
