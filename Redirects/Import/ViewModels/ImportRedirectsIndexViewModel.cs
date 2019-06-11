using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Etch.OrchardCore.SEO.Redirects.Import.ViewModels
{
    public class ImportRedirectsIndexViewModel
    {
        [Required(ErrorMessage = "You must add a file to import from")]
        public IFormFile ImportedRedirectsFile { get; set; }
    }
}
