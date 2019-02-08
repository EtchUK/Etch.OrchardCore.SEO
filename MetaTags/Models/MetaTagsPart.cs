using OrchardCore.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace Moov2.OrchardCore.SEO.MetaTags.Models
{
    public class MetaTagsPart : ContentPart
    {
        [Required]
        public string Data { get; set; }

        public bool HasData => !string.IsNullOrWhiteSpace(Data);
    }
}
