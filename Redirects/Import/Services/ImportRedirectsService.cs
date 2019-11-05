using Etch.OrchardCore.SEO.Redirects.Import.Models;
using Etch.OrchardCore.SEO.Redirects.Indexes;
using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Validation;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Etch.OrchardCore.SEO.Redirects.Import.Services
{
    public class ImportRedirectsService : IImportRedirectsService
    {
        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly YesSql.ISession _session;

        #endregion

        #region Constructor

        public ImportRedirectsService(
            IContentManager contentManager,
            IHttpContextAccessor httpContextAccessor,
            YesSql.ISession session
        )
        {
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;
            _session = session;
        }

        #endregion

        #region IImportRedirectsService

        public async Task<ImportRedirectsResult> ImportRedirectsAsync(IEnumerable<ImportRedirectRow> rows)
        {
            var result = new ImportRedirectsResult();

            var i = 0;
            foreach (var row in rows)
            {
                if (await ImportRowAsync(row))
                    result.Success++;
                else
                    result.Skipped.Add(i + 1);
                i++;
            }

            return result;
        }

        #endregion

        #region Helpers

        private async Task<bool> ImportRowAsync(ImportRedirectRow row)
        {
            if (!ValidateRow(row))
            {
                return false;
            }

            var existing = await _session.Query<ContentItem>()
                .With<RedirectPartIndex>(x => x.Url == row.FromUrl)
                .ListAsync();

            ContentItem redirectItem = null;
            var newItem = false;

            if (existing.Any())
            {
                redirectItem = existing.FirstOrDefault(x => x.Published);
            }

            if (redirectItem == null)
            {
                redirectItem = await _contentManager.NewAsync(Constants.RedirectContentType);
                redirectItem.Owner = _httpContextAccessor.HttpContext.User.Identity.Name;
                redirectItem.CreatedUtc = DateTime.UtcNow;
                newItem = true;
            }

            if (redirectItem == null)
            {
                return false;
            }

            var redirectPart = redirectItem.Get<RedirectPart>(nameof(RedirectPart));

            if (redirectPart == null)
            {
                return false;
            }

            redirectItem.Author = _httpContextAccessor.HttpContext.User.Identity.Name;
            redirectItem.DisplayText = row.Title;
            redirectPart.FromUrl = row.FromUrl?.Trim();
            redirectPart.ToUrl = row.ToUrl?.Trim();
            redirectPart.IsPermanent = true;
            redirectPart.Apply();
            redirectItem.Apply(nameof(RedirectPart), redirectPart);
            ContentExtensions.Apply(redirectItem, redirectItem);

            if (newItem)
            {
                await _contentManager.CreateAsync(redirectItem);
            }
            else
            {
                await _contentManager.UpdateAsync(redirectItem);
            }

            await _contentManager.PublishAsync(redirectItem);

            return true;
        }

        private bool ValidateRow(ImportRedirectRow row)
        {
            if (!UrlValidation.IsRelativeUrl(row.FromUrl))
            {
                return false;
            }
            if (!UrlValidation.ValidFromUrl(row.FromUrl))
            {
                return false;
            }
            if (!UrlValidation.IsWithinLengthLimit(row.FromUrl))
            {
                return false;
            }
            if (!UrlValidation.ValidToUrl(row.ToUrl))
            {
                return false;
            }
            if (!UrlValidation.IsWithinLengthLimit(row.ToUrl))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
