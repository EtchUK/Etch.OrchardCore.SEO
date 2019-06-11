using Etch.OrchardCore.SEO.Redirects.Import.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;

namespace Etch.OrchardCore.SEO.Redirects.Import
{
    [Feature("Etch.OrchardCore.SEO.Redirects.Import")]
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "ImportRedirects",
                areaName: "Etch.OrchardCore.SEO",
                template: "Admin/SEO/Redirects/Import",
                defaults: new { controller = "AdminImportRedirects", action = "Index" }
            );

            routes.MapAreaRoute(
                name: "ImportRedirectsImport",
                areaName: "Etch.OrchardCore.SEO",
                template: "Admin/SEO/Redirects/Import/Submit",
                defaults: new { controller = "AdminImportRedirects", action = "Import" }
            );
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IImportRedirectsService, ImportRedirectsService>();
        }
    }
}
