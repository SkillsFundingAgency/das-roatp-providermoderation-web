using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;

namespace SFA.DAS.Roatp.Application.UnitTests.Locations.Commands.CreateLocation
{
    [TestFixture]
    public class GetProviderQueryTests
    {
        [Test, AutoData]
        public void Constructor_SetsEntityProperty(int ukprn)
        {
            var sut = new GetProviderQuery(ukprn);

            sut.Should().NotBeNull();
            sut.Ukprn .Should().Be(ukprn);
        }
    }
}
