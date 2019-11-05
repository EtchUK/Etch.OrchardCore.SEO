using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OrchardCore.Environment.Shell;

namespace Etch.OrchardCore.SEO.Redirects.Services
{
    public class TenantService : ITenantService
    {
        #region Dependencies

        private readonly ShellSettings _currentShellSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public TenantService(ShellSettings currentShellSettings, IHttpContextAccessor httpContextAccessor)
        {
            _currentShellSettings = currentShellSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion


        public string GetTenantUrl()
        {
            var requestHostInfo = _httpContextAccessor.HttpContext.Request.Host;
            var tenantUrlHost = _currentShellSettings.RequestUrlHost?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? requestHostInfo.Host;

            if (requestHostInfo.Port.HasValue)
            {
                tenantUrlHost += ":" + requestHostInfo.Port;
            }

            var result = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{tenantUrlHost}";

            if (!string.IsNullOrEmpty(_currentShellSettings.RequestUrlPrefix))
            {
                result += "/" + _currentShellSettings.RequestUrlPrefix;
            }

            return result;
        }
    }
}
