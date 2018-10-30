using Microsoft.Extensions.Localization;
using Moov2.OrchardCore.SEO.Redirects.Models;
using Moov2.OrchardCore.SEO.Redirects.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.Redirects.Drivers
{
    public class RedirectPartDisplay : ContentPartDisplayDriver<RedirectPart>
    {
        #region Constants / Statics

        public static char[] InvalidCharactersForFromUrl = ":?#[]@!$&'()*+,.;=<>\\|%".ToCharArray();
        public static char[] InvalidCharactersForToUrl = "?#[]@!$&'()*+,;=<>\\|%".ToCharArray();

        public const int MaxPathLength = 1024;

        #endregion

        # region Dependencies

        private readonly IStringLocalizer<RedirectPartDisplay> T;

        #endregion

        #region Constructor

        public RedirectPartDisplay(IStringLocalizer<RedirectPartDisplay> localizer)
        {
            T = localizer;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Edit(RedirectPart part)
        {
            return Initialize<RedirectPartEditViewModel>("RedirectPart_Edit", model =>
            {
                model.FromUrl = CleanFromUrl(part.FromUrl);
                model.ToUrl = part.ToUrl;

                return Task.CompletedTask;
            });
        }

        public async override Task<IDisplayResult> UpdateAsync(RedirectPart part, IUpdateModel updater)
        {
            var viewModel = new RedirectPartEditViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                part.FromUrl = viewModel.FromUrl?.Trim();
                part.ToUrl = viewModel.ToUrl?.Trim();
            }

            ValidateUrls(part, updater);

            return Edit(part);
        }

        #endregion

        #region Helper Methods

        private string CleanFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            return url.StartsWith("/") ? url.Substring(1) : url;
        }

        private bool IsRelativeUrl(string url)
        {
            Uri result;
            return !Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        private void ValidateUrls(RedirectPart part, IUpdateModel updater)
        {
            if (!IsRelativeUrl(part.FromUrl))
            {
                updater.ModelState.AddModelError(Prefix, nameof(part.FromUrl), T["Your From URL must be relative"]);
            }

            if (part.FromUrl?.IndexOfAny(InvalidCharactersForFromUrl) > -1 || part.FromUrl?.IndexOf(' ') > -1)
            {
                var invalidCharactersForMessage = string.Join(", ", InvalidCharactersForFromUrl.Select(c => $"\"{c}\""));
                updater.ModelState.AddModelError(Prefix, nameof(part.FromUrl), T["Please do not use any of the following characters in your From URL: {0}. No spaces are allowed (please use dashes or underscores instead).", invalidCharactersForMessage]);
            }

            if (part.FromUrl?.Length > MaxPathLength)
            {
                updater.ModelState.AddModelError(Prefix, nameof(part.FromUrl), T["Your From URL is too long. The permalink can only be up to {0} characters.", MaxPathLength]);
            }

            if (part.ToUrl?.IndexOfAny(InvalidCharactersForToUrl) > -1 || part.ToUrl?.IndexOf(' ') > -1)
            {
                var invalidCharactersForMessage = string.Join(", ", InvalidCharactersForToUrl.Select(c => $"\"{c}\""));
                updater.ModelState.AddModelError(Prefix, nameof(part.ToUrl), T["Please do not use any of the following characters in your To URL: {0}. No spaces are allowed (please use dashes or underscores instead).", invalidCharactersForMessage]);
            }

            if (part.ToUrl?.Length > MaxPathLength)
            {
                updater.ModelState.AddModelError(Prefix, nameof(part.ToUrl), T["Your To URL is too long. The permalink can only be up to {0} characters.", MaxPathLength]);
            }
        }

        #endregion
    }
}
