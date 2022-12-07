using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class AuthenticationServicesExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfig = configuration.GetSection(nameof(StaffAuthenticationConfiguration)).Get<StaffAuthenticationConfiguration>();

        services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignOutScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.Wtrealm = authConfig.WtRealm;
                options.MetadataAddress = authConfig.MetadataAddress;
                options.CallbackPath = "/home";
                options.TokenValidationParameters.RoleClaimType = Roles.RoleClaimType;
            })
            .AddCookie();

        return services;
    }
}

public static class Roles
{
    public const string RoleClaimType = "http://service/service";

    public const string RoatpTribalTeam = "TAD";

    public static bool HasValidRole(this ClaimsPrincipal user)
    {
        return user.IsInRole(RoatpTribalTeam);
    }
}
