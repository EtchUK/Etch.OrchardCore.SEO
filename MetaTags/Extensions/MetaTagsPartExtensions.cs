using Etch.OrchardCore.Fields.Dictionary.Fields;
using Etch.OrchardCore.Fields.Dictionary.Models;
using Etch.OrchardCore.SEO.MetaTags.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.SEO.MetaTags.Extensions
{
    public static class MetaTagsPartExtensions
    {
        public static IList<DictionaryItem> GetCustom(this MetaTagsPart part)
        {
            return part.Get<DictionaryField>(Constants.CustomFieldName)?.Data;
        }

        public static string GetDescription(this MetaTagsPart part)
        {
            return part?.Get<TextField>(Constants.DescriptionFieldName)?.Text ?? string.Empty;
        }

        public static string GetImage(this MetaTagsPart part)
        {
            return part?.Get<MediaField>(Constants.ImageFieldName)?.Paths?.FirstOrDefault() ?? string.Empty;
        }

        public static string GetTitle(this MetaTagsPart part)
        {
            return part?.Get<TextField>(Constants.TitleFieldName)?.Text ?? string.Empty;
        }

        public static void UpdateDescription(this MetaTagsPart part, string description)
        {
            var field = part.GetOrCreate<TextField>(Constants.DescriptionFieldName);

            if (field != null)
            {
                field.Text = description;
                part.Apply(Constants.DescriptionFieldName, field);
            }
        }

        public static void UpdateImage(this MetaTagsPart part, string[] paths)
        {
            var field = part.GetOrCreate<MediaField>(Constants.ImageFieldName);

            if (field != null)
            {
                field.Paths = paths;
                part.Apply(Constants.ImageFieldName, field);
            }
        }

        public static void UpdateTitle(this MetaTagsPart part, string title)
        {
            var field = part.GetOrCreate<TextField>(Constants.TitleFieldName);

            if (field != null)
            {
                field.Text = title;
                part.Apply(Constants.TitleFieldName, field);
            }
        }
    }
}
