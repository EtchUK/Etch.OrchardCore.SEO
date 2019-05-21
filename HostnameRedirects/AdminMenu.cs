using Microsoft.Extensions.Localization;
using Etch.OrchardCore.SEO.HostnameRedirects.Drivers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.HostnameRedirects
{
    [Feature("Etch.OrchardCore.SEO.HostnameRedirects")]
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
                        .Add(T["Hostname Redirects"], T["Hostname Redirects"], layers => layers
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = HostnameRedirectsSettingsDisplayDriver.GroupId })
                            .Permission(Permissions.ManageHostnameRedirects)
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
