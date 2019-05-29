using Etch.OrchardCore.SEO.MetaTags.Models;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public interface IMetaTagsService
    {
        void RegisterDefaults(MetaTagsPart metaTags);
        void RegisterOpenGraph(MetaTagsPart metaTags);
        void RegisterTwitter(MetaTagsPart metaTags);
    }
}
