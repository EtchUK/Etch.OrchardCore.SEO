using Etch.OrchardCore.SEO.Redirects.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement.Routing;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Routing
{
    public class RedirectRouteTransformer : DynamicRouteValueTransformer
    {
        private readonly IRedirectEntries _entries;
        private readonly AutorouteOptions _options;

        public RedirectRouteTransformer(IRedirectEntries entries, IOptions<AutorouteOptions> options)
        {
            _entries = entries;
            _options = options.Value;
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var contentItemId = GetContentItemId(httpContext);
            if (!string.IsNullOrEmpty(contentItemId))
            {
                return new ValueTask<RouteValueDictionary>(new RouteValueDictionary {
                    {"Area", "Etch.OrchardCore.SEO"},
                    {"Controller", "Redirect"},
                    {"Action", "Index"},
                    {"ContentItemId", contentItemId}
                });
            }

            return new ValueTask<RouteValueDictionary>((RouteValueDictionary)null);
        }

        private string GetContentItemId(HttpContext httpContext)
        {
            var url = httpContext.Request.Path.ToString().TrimEnd('/');
            if (string.IsNullOrEmpty(url) && _entries.TryGetContentItemId("/", out var contentItemId))
            {
                return contentItemId;
            }

            if (_entries.TryGetContentItemId(url, out var contentItem))
            {
                return contentItem;
            }

            return null;
        }
    }
}