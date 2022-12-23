namespace SFA.DAS.Roatp.ProviderModeration.Web.TagHelpers
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SFA.DAS.Roatp.ProviderModeration.Domain.ApiModels;
    using SFA.DAS.Roatp.ProviderModeration.Web.Models;

    public static class ProviderSearchResultHtmlHelper
    {
        public static IHtmlContent ToProviderStatus(this IHtmlHelper htmlHelper, ProviderSearchResultViewModel provider)
        {
            switch (provider.ProviderStatusType)
            {
                case ProviderStatusType.ActiveButNotTakingOnApprentices:
                    {
                        var html = "<span class=\"govuk-tag govuk-tag--onboarding govuk-!-margin-bottom-1\">Active - but not taking on apprentices</span>" +
                                   "<p class=\"govuk-body\">Updated: " + provider.ProviderStatusUpdatedDate?.ToString("dd MMM yyyy") + "</p>";
                        return new HtmlString(html);
                    }
                case ProviderStatusType.Onboarding:
                    {
                        var html = "<span class=\"govuk-tag govuk-tag--onboarding govuk-!-margin-bottom-1\">On-boarding</span>" +
                                   "<p class=\"govuk-body\">Updated: " + provider.ProviderStatusUpdatedDate?.ToString("dd MMM yyyy") + "</p>";
                        return new HtmlString(html);
                    }
                default:
                    {
                        var html = "<span class=\"govuk-tag govuk-!-margin-bottom-1\">Active</span>" +
                                  "<p class=\"govuk-body\">Updated: " + provider.ProviderStatusUpdatedDate?.ToString("dd MMM yyyy") + "</p>";
                        return new HtmlString(html);
                    }
            }
        }

        public static IHtmlContent ToProviderType(this IHtmlHelper htmlHelper, ProviderSearchResultViewModel provider)
        {
            switch (provider.ProviderType)
            {
                case ProviderType.Employer:
                    {
                        return new HtmlString("Employer");
                    }
                case ProviderType.Supporting:
                    {
                        return new HtmlString("Supporting");
                    }
                default:
                    {
                        return new HtmlString("Main");
                    }
            }
        }

        public static IHtmlContent ToBooleanYesNo(this IHtmlHelper htmlHelper, bool booleanValue)
        {
            string formattedValue = "No";
            if (booleanValue)
            {
                formattedValue = "Yes";
            }

            return new HtmlString(formattedValue);
        }
    }
}