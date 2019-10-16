using Etch.OrchardCore.Fields.Dictionary.Models;
using Etch.OrchardCore.SEO.MetaTags.Models;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public interface IMetaTagsService
    {
        void RegisterDefaults(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null);
        void RegisterOpenGraph(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null);
        void RegisterTwitter(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null);
        void RegisterCustom(IList<DictionaryItem> customMetaTags);
    }
}
