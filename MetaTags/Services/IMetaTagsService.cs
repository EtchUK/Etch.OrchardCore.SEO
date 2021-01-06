using Etch.OrchardCore.SEO.MetaTags.Models;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public interface IMetaTagsService
    {
        void Register(MetaTagsPart part);
    }
}
