using OrchardCore.ContentManagement;

namespace Moov2.OrchardCore.SEO.MetaTags.Models
{
    public class MetaTagsPart : ContentPart
    {
        public string Description { get; set; }
        public string[] Images { get; set; }
        public string Title { get; set; }
    }
}
