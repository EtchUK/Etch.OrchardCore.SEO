using OrchardCore.Autoroute.Services;
using OrchardCore.ContentManagement.Routing;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.Redirects.Services
{
    public class RedirectEntries : AutorouteEntries, IRedirectEntries
    {
        void IRedirectEntries.AddEntries(IEnumerable<AutorouteEntry> entries)
        {
            base.AddEntries(entries);
        }

        void IRedirectEntries.RemoveEntries(IEnumerable<AutorouteEntry> entries)
        {
            base.RemoveEntries(entries);
        }
    }
}
