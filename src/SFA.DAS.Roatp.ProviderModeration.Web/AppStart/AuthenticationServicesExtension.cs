using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
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
                options.Events.OnSecurityTokenValidated = async (ctx) =>
                {
                    await PopulateProviderClaims(ctx.HttpContext, ctx.Principal);
                };
            })
            .AddCookie();

        return services;
    }

    private static Task PopulateProviderClaims(HttpContext httpContext, ClaimsPrincipal principal)
    {
        var userId = principal.Claims.First(c => c.Type.Equals(ProviderClaims.UserId)).Value;
        httpContext.Items.Add(ProviderClaims.UserId, userId);

        var givenname = principal.Claims.First(c => c.Type.Equals(ProviderClaims.Givenname)).Value;
        httpContext.Items.Add(ProviderClaims.Givenname, givenname);

        var surname = principal.Claims.First(c => c.Type.Equals(ProviderClaims.Surname)).Value;
        httpContext.Items.Add(ProviderClaims.Surname, surname);

        principal.Identities.First().AddClaim(new Claim(ProviderClaims.Givenname, givenname));
        principal.Identities.First().AddClaim(new Claim(ProviderClaims.Surname, surname));
        principal.Identities.First().AddClaim(new Claim(ProviderClaims.UserId, userId));
        return Task.CompletedTask;
    }
}

