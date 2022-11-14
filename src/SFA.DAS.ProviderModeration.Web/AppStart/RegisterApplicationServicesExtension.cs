using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class RegisterApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
