using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.MetaTags
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer T;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["SEO"], settings => settings
                        .Add(T["Default Meta Tags"], T["Default Meta Tags"], layers => layers
                        .AddClass("metaTags").Id("metaTags")
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = "DefaultMetaTags" })
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
