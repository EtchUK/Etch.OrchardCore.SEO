using Microsoft.Extensions.Localization;
using Etch.OrchardCore.SEO.RobotsTxt.Drivers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.RobotsTxt
{
    [Feature("Etch.OrchardCore.SEO.RobotsTxt")]
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
                        .AddClass("robots").Id("robots")
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = RobotsTxtSettingsDisplayDriver.GroupId })
                            .Permission(Permissions.ManageRobotsTxt)
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
