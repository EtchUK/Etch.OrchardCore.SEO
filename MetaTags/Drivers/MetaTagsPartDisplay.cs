using Etch.OrchardCore.SEO.MetaTags.Extensions;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Etch.OrchardCore.SEO.MetaTags.Services;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
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

        public override async Task<IDisplayResult> DisplayAsync(MetaTagsPart part, BuildPartDisplayContext context)
        {
            if (context.DisplayType == "Detail")
            {
                await _metaTagsService.RegisterAsync(part);
            }

            return await base.DisplayAsync(part, context);
        }

        #endregion
    }
}