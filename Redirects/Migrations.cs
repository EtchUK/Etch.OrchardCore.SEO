using Etch.OrchardCore.SEO.Redirects.Indexes;
using Etch.OrchardCore.SEO.Redirects.Validation;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using System.Threading.Tasks;
using YesSql;
using YesSql.Sql;

namespace Etch.OrchardCore.SEO.Redirects
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ISession _session;

        public Migrations(IContentDefinitionManager contentDefinitionManager, ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _session = session;
        }

        public async Task<int> CreateAsync()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("RedirectPart", part => part
                .WithDescription("Properties for redirects."));

            await _contentDefinitionManager.AlterTypeDefinitionAsync("Redirect", type => type
                .WithPart("TitlePart")
                .WithPart("RedirectPart")
                .Creatable()
                .Listable());

            return 1;
        }

        public async Task<int> UpdateFrom1Async()
        {
            await SchemaBuilder.CreateMapIndexTableAsync<RedirectPartIndex>(table => table
                .Column<string>("ContentItemId", c => c.WithLength(26))
                .Column<string>("Url", col => col.WithLength(UrlValidation.MaxPathLength))
                .Column<bool>("Published")
            );

            await SchemaBuilder.AlterTableAsync(nameof(RedirectPartIndex), table => table
                .CreateIndex("IDX_RedirectPartIndex_ContentItemId", "ContentItemId")
            );

            return 2;
        }

        public async Task<int> UpdateFrom2Async()
        {
            await SchemaBuilder.AlterIndexTableAsync<RedirectPartIndex>(table => table.AddColumn<bool>("Latest"));

            var contentItems = await _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == "Redirect").ListAsync();

            foreach (var contentItem in contentItems)
            {
                await _session.SaveAsync(contentItem);
            }

            return 3;
        }
    }
}
