using Microsoft.Extensions.DependencyInjection;
using Moov2.OrchardCore.SEO.Redirects;
using Moov2.OrchardCore.SEO.Redirects.Drivers;
using Moov2.OrchardCore.SEO.Redirects.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Moov2.OrchardCore.SEO.Redirects
{
    [Feature("Moov2.OrchardCore.SEO.Redirects")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IContentPartDisplayDriver, RedirectPartDisplay>();

            services.AddSingleton<ContentPart, RedirectPart>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}