using Etch.OrchardCore.SEO.Redirects.Import.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Redirects.Import.Services
{
    public interface IImportRedirectsService
    {
        Task<ImportRedirectsResult> ImportRedirectsAsync(IEnumerable<ImportRedirectRow> rows);
    }
}
