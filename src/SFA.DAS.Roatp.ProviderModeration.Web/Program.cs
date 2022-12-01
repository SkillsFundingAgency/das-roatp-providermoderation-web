using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using NLog.Web;
using SFA.DAS.Roatp.ProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.Roatp.ProviderModeration.Web.AppStart;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseNLog();

        builder.AddConfigFromAzureTableStorage();

        builder.Services.AddControllersWithViews();

        var applicationAssembly = typeof(GetProviderQuery).Assembly;

        builder.Services
            .AddApplicationInsightsTelemetry()
            .AddMediatR(applicationAssembly)
            .AddFluentValidation(new[] { applicationAssembly })
            .AddAuthentication(builder.Configuration)
            .AddApplicationServices();

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
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run();
    }
}
