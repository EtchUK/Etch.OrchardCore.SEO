using Fluid;
using Microsoft.Extensions.DependencyInjection;
using Etch.OrchardCore.SEO.MetaTags.Drivers;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Etch.OrchardCore.SEO.MetaTags.Services;
using Etch.OrchardCore.SEO.MetaTags.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Etch.OrchardCore.SEO.MetaTags
{
    [Feature("Etch.OrchardCore.SEO.MetaTags")]
    public class Startup : StartupBase
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<MetaTagsPartViewModel>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentPartDisplayDriver, MetaTagsPartDisplay>();
            services.AddSingleton<ContentPart, MetaTagsPart>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddScoped<IMetaTagsService, MetaTagsService>();
        }
    }
}
