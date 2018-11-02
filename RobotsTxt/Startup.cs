using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moov2.OrchardCore.SEO.RobotsTxt.Drivers;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using System;

namespace Moov2.OrchardCore.SEO.RobotsTxt
{
    [Feature("Moov2.OrchardCore.SEO.RobotsTxt")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddScoped<IDisplayDriver<ISite>, RobotsTxtSettingsDisplayDriver>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Robots.txt",
                areaName: "Moov2.OrchardCore.SEO",
                template: "robots.txt",
                defaults: new { controller = "RobotsTxt", action = "Index" }
            );
        }
    }
}
