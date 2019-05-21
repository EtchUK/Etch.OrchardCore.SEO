using OrchardCore.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace Etch.OrchardCore.SEO.Redirects.Models
{
    public class RedirectPart : ContentPart
    {
        [Required]
        public string FromUrl { get; set; }

        [Required]
        public string ToUrl { get; set; }

        public bool IsPermanent { get; set; }
    }
}
