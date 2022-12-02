using SFA.DAS.Roatp.CourseManagement.Domain.Configuration;
using SFA.DAS.Roatp.ProviderModeration.Domain.Interfaces;
using SFA.DAS.Roatp.ProviderModeration.Infrastructure.ApiClients;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class RegisterApplicationServicesExtension
{
    public static void AddServiceRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureHttpClient(services, configuration);
        services.AddHttpContextAccessor();
    }
    private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        var handlerLifeTime = TimeSpan.FromMinutes(5);
        services.AddHttpClient<IApiClient, ApiClient>(config =>
        {
            var outerApiConfiguration = configuration
                .GetSection(nameof(RoatpProviderModerationOuterApiConfiguration))
                .Get<RoatpProviderModerationOuterApiConfiguration>();

            config.BaseAddress = new Uri(outerApiConfiguration.BaseUrl);
            config.DefaultRequestHeaders.Add("Accept", "application/json");
            config.DefaultRequestHeaders.Add("X-Version", "1");
            config.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", outerApiConfiguration.SubscriptionKey);
        })
       .SetHandlerLifetime(handlerLifeTime);
    }
}
