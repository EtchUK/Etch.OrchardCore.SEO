using Etch.OrchardCore.Fields.Dictionary.Models;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.MetaTags.Settings
{
    public class DefaultMetaTags
    {
        public IList<DictionaryItem> Custom { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
    }
}
