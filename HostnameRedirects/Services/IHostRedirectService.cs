using Moov2.OrchardCore.SEO.HostnameRedirects.Models;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.HostnameRedirects.Services {
    public interface IHostRedirectService {
        Task<HostnameRedirectsSettings> GetSettingsAsync();
    }
}
