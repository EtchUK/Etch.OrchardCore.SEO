using OrchardCore.Security.Permissions;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.RobotsTxt
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageRobotsTxt = new Permission("ManageRobotsTxt", "Manage robots.txt");

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ManageRobotsTxt };
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