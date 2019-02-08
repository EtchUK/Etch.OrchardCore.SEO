using Fluid;
using Microsoft.Extensions.DependencyInjection;
using Moov2.OrchardCore.SEO.MetaTags.Drivers;
using Moov2.OrchardCore.SEO.MetaTags.Models;
using Moov2.OrchardCore.SEO.MetaTags.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Moov2.OrchardCore.SEO.MetaTags
{
    [Feature("Moov2.OrchardCore.SEO.MetaTags")]
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
        }
    }
}
