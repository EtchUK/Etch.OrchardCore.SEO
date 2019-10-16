using Etch.OrchardCore.SEO.MetaTags.Extensions;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Etch.OrchardCore.SEO.MetaTags.Services;
using Etch.OrchardCore.SEO.MetaTags.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.MetaTags.Drivers
{
    public class MetaTagsPartDisplay : ContentPartDisplayDriver<MetaTagsPart>
    {
        #region Dependencies

        private readonly IMetaTagsService _metaTagsService;

        #endregion

        #region Constructor

        public MetaTagsPartDisplay(IMetaTagsService metaTagsService)
        {
            _metaTagsService = metaTagsService;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(MetaTagsPart metaTagsPart, BuildPartDisplayContext context)
        {
            if (context.DisplayType != "Detail")
            {
                return null;
            }

            var customMetaTags = metaTagsPart.GetDictionaryFieldValue(Constants.CustomFieldName);

            _metaTagsService.RegisterCustom(customMetaTags);
            _metaTagsService.RegisterDefaults(metaTagsPart, customMetaTags);
            _metaTagsService.RegisterOpenGraph(metaTagsPart, customMetaTags);
            _metaTagsService.RegisterTwitter(metaTagsPart, customMetaTags);

            return Initialize<MetaTagsPartViewModel>("MetaTagsPart", model =>
            {
                model.Title = metaTagsPart.Title;
            })
            .Location("Detail", "Content:1");
        }

        public override IDisplayResult Edit(MetaTagsPart metaTagsPart)
        {
            return Initialize<MetaTagsPartViewModel>("MetaTagsPart_Edit", model =>
            {
                model.Description = metaTagsPart.Description;
                model.Images = JsonConvert.SerializeObject(metaTagsPart.Images?.ToList() ?? new List<string>());
                model.Title = metaTagsPart.Title;
                return Task.CompletedTask;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MetaTagsPart part, IUpdateModel updater)
        {
            var model = new MetaTagsPartViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix, t => t.Description, t => t.Title, t => t.Images))
            {
                part.Title = model.Title;
                part.Description = model.Description;

                part.Images = string.IsNullOrWhiteSpace(model.Images)
                    ? Array.Empty<string>()
                    : JsonConvert.DeserializeObject<IList<string>>(model.Images).ToArray();

            }

            return Edit(part);
        }

        #endregion
    }
}