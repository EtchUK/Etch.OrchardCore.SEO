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
