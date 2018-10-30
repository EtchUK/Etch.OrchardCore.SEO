using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Moov2.OrchardCore.SEO.Redirects
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
    }
}
