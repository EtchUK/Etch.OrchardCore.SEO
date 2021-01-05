using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Etch.OrchardCore.SEO.RobotsTxt.Drivers;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using System;
using Microsoft.AspNetCore.Mvc;
using Etch.OrchardCore.SEO.RobotsTxt.Filters;

namespace Etch.OrchardCore.SEO.RobotsTxt
{
    [Feature("Etch.OrchardCore.SEO.RobotsTxt")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, RobotsTxtSettingsDisplayDriver>();
            services.AddScoped<IPermissionProvider, Permissions>();

            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(typeof(NoIndexFilter));
            });
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Robots.txt",
                areaName: "Etch.OrchardCore.SEO",
                pattern: "robots.txt",
                defaults: new { controller = "RobotsTxt", action = "Index" }
            );
        }
    }
}
