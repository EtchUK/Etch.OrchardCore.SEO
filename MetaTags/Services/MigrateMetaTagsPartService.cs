using Etch.OrchardCore.SEO.MetaTags.Extensions;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using YesSql.Services;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public class MigrateMetaTagsPartService : IMigrateMetaTagsPartService
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly ILogger<MigrateMetaTagsPartService> _logger;
        private readonly ISession _session;

        #endregion

        #region Constructor

        public MigrateMetaTagsPartService(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager, ILogger<MigrateMetaTagsPartService> logger, ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _logger = logger;
            _session = session;
        }

        #endregion

        #region Implementation

        public async Task MigrateAsync()
        {
            _logger.LogInformation("Migrate meta tags content from part to fields");

            var contentTypes = _contentDefinitionManager.LoadTypeDefinitions().Where(x => x.Parts.Any(p => p.Name.Equals(nameof(MetaTagsPart)))).Select(x => x.Name);
            var contentItems = await _session.Query<ContentItem>()
                                .With<ContentItemIndex>(x => x.Latest)
                                .Where(x => x.ContentType.IsIn(contentTypes))
                                .ListAsync();

            _logger.LogInformation($"Migrating meta tags content for {contentItems.Count()} content items");

            foreach (var contentItem in contentItems)
            {
                var part = contentItem.As<MetaTagsPart>();

                if (part == null || part.IsEmpty)
                {
                    continue;
                }

                part.UpdateDescription(part.Description);
                part.UpdateImage(part.Images);
                part.UpdateTitle(part.Title);

                contentItem.Apply(part);
                await _contentManager.UpdateAsync(contentItem);
            }

            _logger.LogInformation("Migration of meta tags complete");
        }

        #endregion
    }
}
