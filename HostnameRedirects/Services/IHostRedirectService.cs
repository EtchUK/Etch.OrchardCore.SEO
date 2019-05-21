using Etch.OrchardCore.SEO.HostnameRedirects.Models;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.HostnameRedirects.Services {
    public interface IHostRedirectService {
        Task<HostnameRedirectsSettings> GetSettingsAsync();
    }
}
