using Etch.OrchardCore.SEO.Redirects.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Etch.OrchardCore.SEO.Redirects.Indexes
{
    public class RedirectPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Url { get; set; }
        public bool Published { get; set; }
    }

    public class RedirectPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<RedirectPartIndex>()
                .Map(contentItem =>
                {
                    var url = contentItem.As<RedirectPart>()?.FromUrl;
                    if (!string.IsNullOrEmpty(url) && (contentItem.Published || contentItem.Latest))
                    {
                        return new RedirectPartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            Url = url,
                            Published = contentItem.Published
                        };
                    }

                    return null;
                });
        }
    }
}
