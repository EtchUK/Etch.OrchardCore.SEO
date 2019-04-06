using Moov2.OrchardCore.SEO.MetaTags.Models;

namespace Moov2.OrchardCore.SEO.MetaTags.Services
{
    public interface IMetaTagsService
    {
        void RegisterDefaults(MetaTagsPart metaTags);
        void RegisterOpenGraph(MetaTagsPart metaTags);
        void RegisterTwitter(MetaTagsPart metaTags);
    }
}
