using Etch.OrchardCore.SEO.Redirects.Drivers;
using Etch.OrchardCore.SEO.Redirects.Handlers;
using Etch.OrchardCore.SEO.Redirects.Indexes;
using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Routing;
using Etch.OrchardCore.SEO.Redirects.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Routing;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using System;
using System.Linq;
using YesSql;
using YesSql.Indexes;

namespace Etch.OrchardCore.SEO.Redirects
{
    [Feature("Etch.OrchardCore.SEO.Redirects")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddContentPart<RedirectPart>()
                .UseDisplayDriver<RedirectPartDisplay>()
                .AddHandler<RedirectPartHandler>();

            services.AddSingleton<IIndexProvider, RedirectPartIndexProvider>();
            services.AddSingleton<IRedirectEntries, RedirectEntries>();
            services.AddSingleton<ITenantService, TenantService>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddSingleton<RedirectRouteTransformer>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // The 1st segment prevents the transformer to be executed for the home.
            routes.MapDynamicControllerRoute<RedirectRouteTransformer>("/{**slug}");
        }
    }
}