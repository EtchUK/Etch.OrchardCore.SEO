using Etch.OrchardCore.SEO.MetaTags.Models;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public interface IMetaTagsService
    {
        Task RegisterAsync(MetaTagsPart part);
    }
}
