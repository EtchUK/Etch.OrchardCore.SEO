using Etch.OrchardCore.Fields.Dictionary.Fields;
using Etch.OrchardCore.Fields.Dictionary.Models;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.MetaTags.Extensions
{
    public static class DictionaryFieldExtensions
    {
        public static IList<DictionaryItem> GetDictionaryFieldValue(this ContentPart part, string name)
        {
            return part?.Get<DictionaryField>(name)?.Data;
        }
    }
}
