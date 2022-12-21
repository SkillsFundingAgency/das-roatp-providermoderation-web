using MediatR;

namespace SFA.DAS.Roatp.ProviderModeration.Application.Providers.Commands.UpdateProviderDescription
{
    public class UpdateProviderDescriptionCommand : IRequest
    {
        public int Ukprn { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string ProviderDescription { get; set; }
    }
}
