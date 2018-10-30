using OrchardCore.Autoroute.Model;

namespace Moov2.OrchardCore.SEO.Redirects.Services
{
    public static class RedirectEntriesExtensions
    {
        public static void AddEntry(this IRedirectEntries entries, string contentItemId, string path)
        {
            entries.AddEntries(new[] { new AutorouteEntry { ContentItemId = contentItemId, Path = path } });
        }

        public static void RemoveEntry(this IRedirectEntries entries, string contentItemId, string path)
        {
            entries.RemoveEntries(new[] { new AutorouteEntry { ContentItemId = contentItemId, Path = path } });
        }
    }
}
