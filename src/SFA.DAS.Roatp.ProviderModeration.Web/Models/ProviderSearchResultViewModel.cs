using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Models
{
    public class ProviderSearchResultViewModel
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string ProviderDescription { get; set; }
        
        public ProviderType ProviderType { get; set; }
        public ProviderStatusType ProviderStatusType { get; set; }
        public DateTime? ProviderStatusUpdatedDate { get; set; }
        public bool IsProviderHasStandard { get; set; } = false;
        public string ShowAddLink { get; set; }
        public string ShowChangeLink { get; set; }
        public string ProviderDescriptionStatus { get; set; }

        public string AddProviderDescriptionLink { get; set; }
        public string ChangeProviderDescriptionLink { get; set; }

        public static implicit operator ProviderSearchResultViewModel(GetProviderQueryResult source)
        {
            return new()
            {
                Ukprn = source.Provider.Ukprn,
                LegalName = source.Provider.LegalName,
                ProviderDescription = source.Provider.MarketingInfo,
                ProviderType = source.Provider.ProviderType,
                ProviderStatusType = source.Provider.ProviderStatusType,
                ProviderStatusUpdatedDate = source.Provider.ProviderStatusUpdatedDate,
                IsProviderHasStandard = source.Provider.IsProviderHasStandard,
                ShowAddLink = source.Provider.ProviderStatusType == ProviderStatusType.Onboarding && string.IsNullOrEmpty(source.Provider.MarketingInfo) ? "display: inline;" : "display: none;",
                ShowChangeLink = string.IsNullOrEmpty(source.Provider.MarketingInfo) ?  "display: none;" : "display: inline;",
                ProviderDescriptionStatus = string.IsNullOrEmpty(source.Provider.MarketingInfo) ? "Not submitted" : "Submitted",
        };
    }
    }
}
