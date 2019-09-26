using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.HostnameRedirects
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageHostnameRedirects = new Permission("ManageHostnameRedirects", "Manage hostname redirects");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageHostnameRedirects }.AsEnumerable());
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