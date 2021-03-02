using OrchardCore.ContentManagement.Routing;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.Redirects.Services
{
    public interface IRedirectEntries : IAutorouteEntries
    {
        void AddEntries(IEnumerable<AutorouteEntry> entries);

        void RemoveEntries(IEnumerable<AutorouteEntry> entries);
    }
}
