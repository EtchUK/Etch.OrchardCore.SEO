using Microsoft.AspNetCore.Mvc;
using Etch.OrchardCore.SEO.Redirects.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Modules;
using System.Threading.Tasks;
using OrchardCore.Environment.Shell;
using Etch.OrchardCore.SEO.Redirects.Services;

namespace Etch.OrchardCore.SEO.Controllers
{
    [Feature("Etch.OrchardCore.SEO.Redirects")]
    public class RedirectController : Controller
    {
        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly ITenantService _tenantService;

        #endregion

        #region Constructor

        public RedirectController(IContentManager contentManager, ITenantService tenantService)
        {
            _contentManager = contentManager;
            _tenantService = tenantService;
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

            if (part.ToUrl.StartsWith("/", System.StringComparison.Ordinal))
            {
                return new RedirectResult($"{_tenantService.GetTenantUrl()}{part.ToUrl}", part.IsPermanent);
            }

            return new RedirectResult(part.ToUrl, part.IsPermanent);
        }

        #endregion
    }
}
