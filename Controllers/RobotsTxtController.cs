using Microsoft.AspNetCore.Mvc;
using Etch.OrchardCore.SEO.RobotsTxt.Models;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Settings;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Controllers
{
    [Feature("Etch.OrchardCore.SEO.RobotsTxt")]
    public class RobotsTxtController : Controller
    {
        #region Dependencies

        private readonly ISiteService _siteService;

        #endregion

        #region Constructor

        public RobotsTxtController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var robotsSettings = siteSettings.As<RobotsTxtSettings>();

            if (robotsSettings == null || robotsSettings.Mode == RobotsTxtModes.NotDefined)
                return NotFound();

            return Content(RobotsTxtModes.GetOutput(robotsSettings), MediaTypeNames.Text.Plain);
        }

        #endregion
    }
}
