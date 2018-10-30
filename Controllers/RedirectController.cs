using Microsoft.AspNetCore.Mvc;
using Moov2.OrchardCore.SEO.Redirects.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.Controllers
{
    [Feature("Moov2.OrchardCore.SEO.Redirects")]
    public class RedirectController : Controller
    {
        #region Dependencies

        private readonly IContentManager _contentManager;

        #endregion

        #region Constructor

        public RedirectController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId);

            if (contentItem == null)
            {
                return NotFound();
            }

            var part = contentItem.As<RedirectPart>();

            return new RedirectResult(part.ToUrl);
        }

        #endregion
    }
}
