using Microsoft.Extensions.DependencyInjection;
using Etch.OrchardCore.SEO.MetaTags.Drivers;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Etch.OrchardCore.SEO.MetaTags.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;

namespace Etch.OrchardCore.SEO.MetaTags
{
    [Feature("Etch.OrchardCore.SEO.MetaTags")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddContentPart<MetaTagsPart>()
                .UseDisplayDriver<MetaTagsPartDisplay>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddScoped<IMetaTagsService, MetaTagsService>();
            services.AddScoped<IMigrateMetaTagsPartService, MigrateMetaTagsPartService>();
        }
    }
}
