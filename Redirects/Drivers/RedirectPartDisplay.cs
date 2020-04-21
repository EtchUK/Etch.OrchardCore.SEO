using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Services;
using Etch.OrchardCore.SEO.Redirects.Validation;
using Etch.OrchardCore.SEO.Redirects.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Drivers
{
    public class RedirectPartDisplay : ContentPartDisplayDriver<RedirectPart>
    {
        # region Dependencies

        private readonly IStringLocalizer<RedirectPartDisplay> T;
        private readonly ITenantService _tenantService;

        #endregion

        #region Constructor

        public RedirectPartDisplay(IStringLocalizer<RedirectPartDisplay> localizer, ITenantService tenantService)
        {
            T = localizer;
            _tenantService = tenantService;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Edit(RedirectPart part)
        {
            return Initialize<RedirectPartEditViewModel>("RedirectPart_Edit", model =>
            {
                model.FromUrl = part.FromUrl;
                model.ToUrl = part.ToUrl;
                model.IsPermanent = part.ContentItem.Id == 0 ? true : part.IsPermanent;
                model.TenantUrl = _tenantService.GetTenantUrl();
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(RedirectPart part, IUpdateModel updater)
        {
            var viewModel = new RedirectPartEditViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                part.FromUrl = viewModel.FromUrl?.Trim();
                part.ToUrl = viewModel.ToUrl?.Trim();
                part.IsPermanent = viewModel.IsPermanent;
            }

            ValidateUrls(part, updater);

            return Edit(part);
        }

        #endregion

        #region Helper Methods

        private void ValidateUrls(RedirectPart part, IUpdateModel updater)
        {
            if (!UrlValidation.ValidFromUrl(part.FromUrl))
            {
                var invalidCharactersForMessage = string.Join(", ", UrlValidation.InvalidCharactersForFromUrl.Select(c => $"\"{c}\""));
                updater.ModelState.AddModelError(Prefix, nameof(part.FromUrl), T["Please do not use any of the following characters in your From URL: {0}. No spaces are allowed (please use dashes or underscores instead).", invalidCharactersForMessage]);
            }

            if (!UrlValidation.IsWithinLengthLimit(part.FromUrl))
            {
                updater.ModelState.AddModelError(Prefix, nameof(part.FromUrl), T["Your From URL is too long. The permalink can only be up to {0} characters.", UrlValidation.MaxPathLength]);
            }

            if (!UrlValidation.ValidToUrl(part.ToUrl))
            {
                var invalidCharactersForMessage = string.Join(", ", UrlValidation.InvalidCharactersForToUrl.Select(c => $"\"{c}\""));
                updater.ModelState.AddModelError(Prefix, nameof(part.ToUrl), T["Please do not use any of the following characters in your To URL: {0}. No spaces are allowed (please use dashes or underscores instead).", invalidCharactersForMessage]);
            }

            if (!UrlValidation.IsWithinLengthLimit(part.ToUrl))
            {
                updater.ModelState.AddModelError(Prefix, nameof(part.ToUrl), T["Your To URL is too long. The permalink can only be up to {0} characters.", UrlValidation.MaxPathLength]);
            }
        }

        #endregion
    }
}
