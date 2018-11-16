using Moov2.OrchardCore.SEO.HostnameRedirects.Models;
using OrchardCore.Entities;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.HostnameRedirects.Services {

    public class HostRedirectService : IHostRedirectService {
        private readonly ISiteService _siteService;

        public HostRedirectService(ISiteService siteService) {
            _siteService = siteService;
        }

        public async Task<HostnameRedirectsSettings> GetSettingsAsync() {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            return siteSettings.As<HostnameRedirectsSettings>();
        }
    }

}
