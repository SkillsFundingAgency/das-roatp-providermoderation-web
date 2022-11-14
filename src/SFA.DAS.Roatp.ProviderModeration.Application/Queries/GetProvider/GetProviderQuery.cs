using MediatR;

namespace SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;

public class GetProviderQuery : IRequest<GetProviderQueryResponse>
{
    public int Ukprn { get; init; }
}
