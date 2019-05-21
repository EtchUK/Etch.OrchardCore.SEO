using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Etch.OrchardCore.SEO.Redirects.Drivers;
using Etch.OrchardCore.SEO.Redirects.Handlers;
using Etch.OrchardCore.SEO.Redirects.Indexes;
using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Services;
using OrchardCore.Autoroute.Model;
using OrchardCore.Autoroute.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
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

            services.AddScoped<IContentPartDisplayDriver, RedirectPartDisplay>();
            services.AddScoped<IContentPartHandler, RedirectPartHandler>();

            services.AddSingleton<ContentPart, RedirectPart>();
            services.AddSingleton<IIndexProvider, RedirectPartIndexProvider>();
            services.AddSingleton<IRedirectEntries, RedirectEntries>();

            services.AddScoped<IDataMigration, Migrations>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var entries = serviceProvider.GetRequiredService<IRedirectEntries>();
            var session = serviceProvider.GetRequiredService<ISession>();
            var redirects = session.QueryIndex<RedirectPartIndex>(o => o.Published).ListAsync().GetAwaiter().GetResult();

            entries.AddEntries(redirects.Select(x => new AutorouteEntry { ContentItemId = x.ContentItemId, Path = x.Url }));

            var redirectRoute = new AutorouteRoute(entries, routes.DefaultHandler);

            routes.Routes.Insert(0, redirectRoute);
        }
    }
}