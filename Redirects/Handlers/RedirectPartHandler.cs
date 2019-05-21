using Microsoft.AspNetCore.Routing;
using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Handlers
{
    public class RedirectPartHandler : ContentPartHandler<RedirectPart>
    {
        #region Dependencies

        private IRedirectEntries _entries;

        #endregion

        #region Constructor

        public RedirectPartHandler(IRedirectEntries entries)
        {
            _entries = entries;
        }

        #endregion

        #region Overrides

        public override Task GetContentItemAspectAsync(ContentItemAspectContext context, RedirectPart part)
        {
            context.For<ContentItemMetadata>(metadata =>
            {
                if (metadata.DisplayRouteValues == null)
                {
                    metadata.DisplayRouteValues = new RouteValueDictionary {
                        {"Area", "Etch.OrchardCore.SEO"},
                        {"Controller", "Redirect"},
                        {"Action", "Index"},
                        {"ContentItemId", context.ContentItem.ContentItemId}
                    };
                }
            });

            return Task.CompletedTask;
        }

        public override Task PublishedAsync(PublishContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl))
            {
                _entries.AddEntry(part.ContentItem.ContentItemId, part.FromUrl);
            }

            return Task.CompletedTask;
        }

        public override Task RemovedAsync(RemoveContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl))
            {
                _entries.RemoveEntry(part.ContentItem.ContentItemId, part.FromUrl);
            }

            return Task.CompletedTask;
        }

        public override Task UnpublishedAsync(PublishContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl))
            {
                _entries.RemoveEntry(part.ContentItem.ContentItemId, part.FromUrl);
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
