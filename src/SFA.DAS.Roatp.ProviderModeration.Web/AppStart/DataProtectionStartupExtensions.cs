using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

public static class DataProtectionStartupExtensions
{
    public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        var configuration = config.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>();
        if (!environment.IsDevelopment() && !string.IsNullOrEmpty(configuration.SessionRedisConnectionString))
        {
            var redisConnectionString = configuration.SessionRedisConnectionString;
            var dataProtectionKeysDatabase = configuration.DataProtectionKeysDatabase;

            var redis = ConnectionMultiplexer.Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

            services.AddDataProtection()
                .SetApplicationName("das-admin-service-web")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }
        return services;
    }
}