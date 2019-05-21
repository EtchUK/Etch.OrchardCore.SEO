using OrchardCore.Security.Permissions;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.HostnameRedirects
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageHostnameRedirects = new Permission("ManageHostnameRedirects", "Manage hostname redirects");

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ManageHostnameRedirects };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageHostnameRedirects }
                }
            };
        }
    }
}