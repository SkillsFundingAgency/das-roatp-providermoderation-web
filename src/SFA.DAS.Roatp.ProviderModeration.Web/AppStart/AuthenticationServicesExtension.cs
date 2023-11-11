using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using SFA.DAS.DfESignIn.Auth.AppStart;
using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
using SFA.DAS.Roatp.ProviderModeration.Web.Configuration;

namespace SFA.DAS.Roatp.ProviderModeration.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class AuthenticationServicesExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration["ApplicationConfiguration:UseDfeSignIn"],out var useDfeSignIn);
        if (useDfeSignIn)
        {
            services.AddAndConfigureDfESignInAuthentication(configuration,
                $"{typeof(AuthenticationServicesExtension).Assembly.GetName().Name}.Auth",
                typeof(CustomServiceRole),
                ClientName.RoatpServiceAdmin,
                "/SignOut",
                "");
            return services;
        }
        
        var authConfig = configuration.GetSection(nameof(StaffAuthenticationConfiguration)).Get<StaffAuthenticationConfiguration>();

        var cookieOptions = new Action<CookieAuthenticationOptions>(options =>
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

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
                options.TokenValidationParameters.RoleClaimType = Roles.RoleClaimType;
                options.Events.OnSecurityTokenValidated = async (ctx) =>
                {
                    await PopulateProviderClaims(ctx.HttpContext, ctx.Principal);
                };
            })
            .AddCookie(cookieOptions);

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

