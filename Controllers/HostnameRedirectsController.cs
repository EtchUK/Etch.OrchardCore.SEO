using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;
using OrchardCore.Settings;
using System.Net.Mime;

namespace Moov2.OrchardCore.SEO.Controllers
{
    [Feature("Moov2.OrchardCore.SEO.HostnameRedirects")]
    public class HostnameRedirectsController : Controller
    {
        #region Dependencies

        private readonly ISiteService _siteService;

        #endregion

        #region Constructor

        public HostnameRedirectsController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        #endregion

        #region Actions

        public IActionResult Index()
        {
            return Content("", MediaTypeNames.Text.Plain);
        }

        #endregion
    }
}
