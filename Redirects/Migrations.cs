using Etch.OrchardCore.SEO.Redirects.Drivers;
using Etch.OrchardCore.SEO.Redirects.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Etch.OrchardCore.SEO.Redirects
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("RedirectPart", part => part
                .WithDescription("Properties for redirects."));

            _contentDefinitionManager.AlterTypeDefinition("Redirect", type => type
                .WithPart("TitlePart")
                .WithPart("RedirectPart")
                .Creatable()
                .Listable());

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(RedirectPartIndex), table => table
                .Column<string>("ContentItemId", c => c.WithLength(26))
                .Column<string>("Url", col => col.WithLength(RedirectPartDisplay.MaxPathLength))
                .Column<bool>("Published")
            );

            SchemaBuilder.AlterTable(nameof(RedirectPartIndex), table => table
                .CreateIndex("IDX_RedirectPartIndex_ContentItemId", "ContentItemId")
            );

            return 2;
        }
    }
}
