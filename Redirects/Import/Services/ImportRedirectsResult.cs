using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.Redirects.Import.Services
{
    public class ImportRedirectsResult
    {
        public int Success { get; set; }
        public IList<int> Skipped { get; set; } = new List<int>();
    }
}
