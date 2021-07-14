using Etch.OrchardCore.SEO.Redirects.Models;
using Etch.OrchardCore.SEO.Redirects.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql.Indexes;

namespace Etch.OrchardCore.SEO.Redirects.Indexes
{
    public class RedirectPartIndex : MapIndex
    {
        public int DocumentId { get; set; }
        public string ContentItemId { get; set; }
        public string Url { get; set; }
        public bool Published { get; set; }
        public bool Latest { get; set; }
    }


    public class RedirectPartIndexProvider : ContentHandlerBase, IScopedIndexProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<ContentItem> _itemRemoved = new HashSet<ContentItem>();
        private readonly HashSet<string> _partRemoved = new HashSet<string>();
        private IContentDefinitionManager _contentDefinitionManager;

        public RedirectPartIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task RemovedAsync(RemoveContentContext context)
        {
            if (context.NoActiveVersionLeft)
            {
                var part = context.ContentItem.As<RedirectPart>();

                if (part != null)
                {
                    _itemRemoved.Add(context.ContentItem);
                }
            }

            return Task.CompletedTask;
        }

        public override async Task PublishedAsync(PublishContentContext context)
        {
            var part = context.ContentItem.As<RedirectPart>();

            // Validate that the content definition contains this part, this prevents indexing parts
            // that have been removed from the type definition, but are still present in the elements.            
            if (part != null)
            {
                // Lazy initialization because of ISession cyclic dependency.
                _contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();

                // Search for this part.
                var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType);
                if (!contentTypeDefinition.Parts.Any(ctpd => ctpd.Name == nameof(RedirectPart)))
                {
                    context.ContentItem.Remove<RedirectPart>();
                    _partRemoved.Add(context.ContentItem.ContentItemId);

                    // When the part has been removed enlist an update for after the session has been committed.
                    var redirectEntries = _serviceProvider.GetRequiredService<IRedirectEntries>();
                    await redirectEntries.UpdateEntriesAsync();
                }
            }
        }

        public string CollectionName { get; set; }
        public Type ForType() => typeof(ContentItem);
        public void Describe(IDescriptor context) => Describe((DescribeContext<ContentItem>)context);

        public void Describe(DescribeContext<ContentItem> context)
        {
            context.For<RedirectPartIndex>()
                .When(contentItem => contentItem.Has<RedirectPart>() || _partRemoved.Contains(contentItem.ContentItemId))
                .Map(contentItem =>
                {
                    // If the content item was removed, a record is still added.
                    var itemRemoved = _itemRemoved.Contains(contentItem);

                    if (!contentItem.Published && !contentItem.Latest && !itemRemoved)
                    {
                        return null;
                    }

                    // If the part was removed from the type definition, a record is still added.
                    var partRemoved = _partRemoved.Contains(contentItem.ContentItemId);
                    var part = contentItem.As<RedirectPart>();

                    if (!partRemoved && (part == null || String.IsNullOrEmpty(part.FromUrl)))
                    {
                        return null;
                    }

                    var results = new List<RedirectPartIndex>
                    {
                        // If the part is disabled or was removed, a record is still added but with a null path.
                        new RedirectPartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            Url = !partRemoved ? part.FromUrl : null,
                            Published = contentItem.Published,
                            Latest = contentItem.Latest
                        }
                    };

                    if (partRemoved || itemRemoved)
                    {
                        return results;
                    }

                    return results;
                });
        }
    }
}
