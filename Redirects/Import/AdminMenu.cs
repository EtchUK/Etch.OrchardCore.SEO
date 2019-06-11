using Microsoft.Extensions.Localization;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Import
{
    [Feature("Etch.OrchardCore.SEO.Redirects.Import")]
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["SEO"], settings => settings
                        .Add(T["Import Redirects"], T["Import Redirects"], layers => layers
                            .Action("Index", "AdminImportRedirects", new { area = "Etch.OrchardCore.SEO" })
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
