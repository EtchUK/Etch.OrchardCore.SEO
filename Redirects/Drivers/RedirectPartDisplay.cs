using Moov2.OrchardCore.SEO.Redirects.Models;
using Moov2.OrchardCore.SEO.Redirects.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.Redirects.Drivers
{
    public class RedirectPartDisplay : ContentPartDisplayDriver<RedirectPart>
    {
        public override IDisplayResult Edit(RedirectPart part)
        {
            return Initialize<RedirectPartEditViewModel>("RedirectPart_Edit", model =>
            {
                model.FromUrl = part.FromUrl;
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

            return Edit(part);
        }
    }
}
