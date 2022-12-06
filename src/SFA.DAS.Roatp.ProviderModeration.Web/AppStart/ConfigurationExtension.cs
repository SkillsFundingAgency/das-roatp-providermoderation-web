using Microsoft.Extensions.Options;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtension
{
    public static WebApplicationBuilder AddConfigFromAzureTableStorage(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Configuration.AddAzureTableStorage(options =>
        {
            options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
            options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
            options.EnvironmentName = configuration["EnvironmentName"];
            options.PreFixConfigurationKeys = false;
        });

        return builder;
    }

    public static void RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationConfiguration>(configuration.GetSection(nameof(ApplicationConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApplicationConfiguration>>().Value);
    }
}
