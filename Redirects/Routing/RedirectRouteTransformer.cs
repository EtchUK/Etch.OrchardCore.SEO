using Etch.OrchardCore.SEO.Redirects.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Routing
{
    public class RedirectRouteTransformer : DynamicRouteValueTransformer
    {
        private readonly IRedirectEntries _entries;

        public RedirectRouteTransformer(IRedirectEntries entries)
        {
            _entries = entries;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var contentItemId = await GetContentItemIdAsync(httpContext);

            if (string.IsNullOrEmpty(contentItemId))
                return null;

            return new RouteValueDictionary
            {
                { "Area", "Etch.OrchardCore.SEO" },
                { "Controller", "Redirect" },
                { "Action", "Index" },
                { "ContentItemId", contentItemId }
            };
        }

        private async Task<string> GetContentItemIdAsync(HttpContext httpContext)
        {
            var url = httpContext.Request.Path.ToString().TrimEnd('/');

            var (found, entry) = await _entries.TryGetEntryByPathAsync("/");

            if (string.IsNullOrEmpty(url) && found)
            {
                return entry.ContentItemId;
            }

            (found, entry) = await _entries.TryGetEntryByPathAsync(url);

            if (found)
            {
                return entry.ContentItemId;
            }

            return null;
        }
    }
}