using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.RobotsTxt
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageRobotsTxt = new Permission("ManageRobotsTxt", "Manage robots.txt");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageRobotsTxt }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageRobotsTxt }
                }
            };
        }
    }
}