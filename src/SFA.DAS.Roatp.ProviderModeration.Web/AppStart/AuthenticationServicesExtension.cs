using System.Diagnostics.CodeAnalysis;
using SFA.DAS.DfESignIn.Auth.AppStart;
using SFA.DAS.DfESignIn.Auth.Enums;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class AuthenticationServicesExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAndConfigureDfESignInAuthentication(configuration,
            "SFA.DAS.AdminService.Web.Auth",
            typeof(CustomServiceRole),
            ClientName.RoatpServiceAdmin,
            "/SignOut",
            "");
        return services;
    }
}

