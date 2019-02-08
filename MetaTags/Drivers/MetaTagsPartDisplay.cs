using Moov2.OrchardCore.SEO.MetaTags.Models;
using Moov2.OrchardCore.SEO.MetaTags.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ResourceManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.MetaTags.Drivers
{
    public class MetaTagsPartDisplay : ContentPartDisplayDriver<MetaTagsPart>
    {
        #region Dependencies

        private readonly IResourceManager _resourceManager;

        #endregion

        #region Constructor

        public MetaTagsPartDisplay(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(MetaTagsPart metaTagsPart)
        {
            if (!metaTagsPart.HasData)
            {
                return null;
            }

            foreach (var entry in JsonConvert.DeserializeObject<IList<MetaEntry>>(metaTagsPart.Data))
            {
                _resourceManager.RegisterMeta(entry);
            }

            return null;
        }

        public override IDisplayResult Edit(MetaTagsPart metaTagsPart)
        {
            return Initialize<MetaTagsPartViewModel>("MetaTagsPart_Edit", model =>
            {
                model.Data = metaTagsPart.Data;
                return Task.CompletedTask;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MetaTagsPart model, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(model, Prefix, t => t.Data);

            return Edit(model);
        }

        #endregion
    }
}