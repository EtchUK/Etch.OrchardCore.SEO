using Microsoft.Extensions.Localization;
using Moov2.OrchardCore.SEO.RobotsTxt.Drivers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.RobotsTxt
{
    [Feature("Moov2.OrchardCore.SEO.RobotsTxt")]
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
                        .Add(T["Robots.txt"], T["Robots.txt"], layers => layers
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = RobotsTxtSettingsDisplayDriver.GroupId })
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
