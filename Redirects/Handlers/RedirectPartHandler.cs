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

        public override async Task PublishedAsync(PublishContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl))
            {
                // Update entries from the index table after the session is committed.
                await _entries.UpdateEntriesAsync();
            }
        }

        public override async Task UnpublishedAsync(PublishContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl))
            {
                // Update entries from the index table after the session is committed.
                await _entries.UpdateEntriesAsync();
            }
        }

        public override async Task RemovedAsync(RemoveContentContext context, RedirectPart part)
        {
            if (!string.IsNullOrWhiteSpace(part.FromUrl) && context.NoActiveVersionLeft)
            {
                // Update entries from the index table after the session is committed.
                await _entries.UpdateEntriesAsync();
            }
        }

        #endregion
    }
}
