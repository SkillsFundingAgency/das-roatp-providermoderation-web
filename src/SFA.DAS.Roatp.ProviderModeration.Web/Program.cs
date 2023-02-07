using System.Diagnostics.CodeAnalysis;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using SFA.DAS.Roatp.ProviderModeration.Application.Providers.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
using SFA.DAS.Roatp.ProviderModeration.Web.Validators;

namespace SFA.DAS.Roatp.ProviderModeration.Web;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        NLogBuilder.ConfigureNLog("nlog.config");

        builder.Host.UseNLog();

        builder.AddConfigFromAzureTableStorage();

        builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

        builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

        builder.Services.RegisterConfigurations(builder.Configuration);

        var applicationAssembly = typeof(GetProviderQuery).Assembly;

        builder.Services
        .AddFluentValidation(fv =>
         {
             fv.RegisterValidatorsFromAssemblyContaining<ProviderSearchSubmitModelValidator>();
             fv.ImplicitlyValidateChildProperties = true;
         });

        builder.Services
            .AddApplicationInsightsTelemetry()
            .AddMediatR(applicationAssembly)
            .AddAuthentication(builder.Configuration)
            .AddServiceRegistrations(builder.Configuration);

        builder.Services.AddHealthChecks();

        //Add services above
        var app = builder.Build();
        //Add pipeline below

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHealthChecks();
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }
}
