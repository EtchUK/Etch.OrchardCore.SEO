using Microsoft.AspNetCore.Mvc;
using Etch.OrchardCore.SEO.Redirects.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Controllers
{
    [Feature("Etch.OrchardCore.SEO.Redirects")]
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

            return new RedirectResult(part.ToUrl, part.IsPermanent);
        }

        #endregion
    }
}
