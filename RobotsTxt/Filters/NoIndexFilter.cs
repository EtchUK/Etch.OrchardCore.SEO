using Etch.OrchardCore.SEO.RobotsTxt.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.Entities;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.RobotsTxt.Filters
{
    public class NoIndexFilter : IAsyncResultFilter
    {
        #region Dependencies

        private readonly ISiteService _siteService;

        #endregion

        #region Constructor

        public NoIndexFilter(ISiteService siteService)
        {
            _siteService = siteService;    
        }

        #endregion

        #region Implementation

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var robotsSettings = siteSettings.As<RobotsTxtSettings>();

            if (robotsSettings == null || !robotsSettings.NoIndex)
            {
                await next.Invoke();
                return;
            }

            context.HttpContext.Response.Headers["X-Robots-Tag"] = "noindex";
            await next.Invoke();
        }

        #endregion
    }
}
