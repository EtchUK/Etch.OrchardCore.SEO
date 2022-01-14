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

        #endregion Dependencies

        #region Constructor

        public RedirectController(IContentManager contentManager, ITenantService tenantService)
        {
            _contentManager = contentManager;
            _tenantService = tenantService;
        }

        #endregion Constructor

        #region Actions

        public async Task<IActionResult> Index(string contentItemId)
        {
            var contentItem = await _contentManager.GetAsync(contentItemId);

            if (contentItem == null)
            {
                return NotFound();
            }

            var part = contentItem.As<RedirectPart>();
            var toUrl = part.ToUrl;

            if (toUrl.StartsWith("https://") || toUrl.StartsWith("http://"))
            {
                return new RedirectResult(toUrl, part.IsPermanent);
            }

            if (!toUrl.StartsWith("/", System.StringComparison.Ordinal))
            {
                toUrl = $"/{toUrl}";
            }

            return new RedirectResult($"{_tenantService.GetTenantUrl()}{toUrl}", part.IsPermanent);
        }

        #endregion Actions
    }
}