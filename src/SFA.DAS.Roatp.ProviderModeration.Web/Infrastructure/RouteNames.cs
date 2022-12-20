using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class RouteNames
    {
        public const string GetProviderDescription = "provider-description";
        public const string PostProviderDescription = "provider-description";
        public const string GetProviderDetails = "provider-details";
        public const string GetAddProviderDescription = "add-provider-description";
        public const string PostAddProviderDescription = "add-provider-description";
        public const string GetReviewProviderDescription = "review-provider-description";
        public const string GetReviewProviderDescriptionEdit = "review-provider-description-edit";
        public const string PostReviewProviderDescription = "review-provider-description";
    }
}
