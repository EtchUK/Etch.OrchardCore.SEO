using Moov2.OrchardCore.SEO.MetaTags.Models;
using Moov2.OrchardCore.SEO.MetaTags.Services;
using Moov2.OrchardCore.SEO.MetaTags.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.MetaTags.Drivers
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

        public override IDisplayResult Display(MetaTagsPart metaTagsPart)
        {
            _metaTagsService.RegisterDefaults(metaTagsPart);
            _metaTagsService.RegisterOpenGraph(metaTagsPart);
            _metaTagsService.RegisterTwitter(metaTagsPart);

            return Initialize<MetaTagsPartViewModel>("MetaTagsPart", model =>
            {
                model.Title = metaTagsPart.Title;
            })
            .Location("Detail", "Header:1");
        }

        public override IDisplayResult Edit(MetaTagsPart metaTagsPart)
        {
            return Initialize<MetaTagsPartViewModel>("MetaTagsPart_Edit", model =>
            {
                model.Description = metaTagsPart.Description;
                model.Images = JsonConvert.SerializeObject(metaTagsPart.Images.Select(x => new MetaTagImage { Path = x }).ToList());
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
                    : JsonConvert.DeserializeObject<IList<MetaTagImage>>(model.Images).Select(x => x.Path).ToArray();

            }

            return Edit(part);
        }

        #endregion
    }
}