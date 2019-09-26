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
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "ImportRedirects",
                areaName: "Etch.OrchardCore.SEO",
                pattern: "Admin/SEO/Redirects/Import",
                defaults: new { controller = "AdminImportRedirects", action = "Index" }
            );

            routes.MapAreaControllerRoute(
                name: "ImportRedirectsImport",
                areaName: "Etch.OrchardCore.SEO",
                pattern: "Admin/SEO/Redirects/Import/Submit",
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
