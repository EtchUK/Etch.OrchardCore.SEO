using System.ComponentModel.DataAnnotations;

namespace Moov2.OrchardCore.SEO.Redirects.ViewModels
{
    public class RedirectPartEditViewModel
    {
        [Required(ErrorMessage = "The From Url field is required")]
        public string FromUrl { get; set; }

        [Required(ErrorMessage = "The To Url field is required")]
        public string ToUrl { get; set; }

        public bool IsPermanent { get; set; }
    }
}
