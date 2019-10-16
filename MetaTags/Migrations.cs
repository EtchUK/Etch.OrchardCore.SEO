using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using Etch.OrchardCore.Fields.Dictionary.Fields;

namespace Etch.OrchardCore.SEO.MetaTags
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("MetaTagsPart", builder => builder
                .Attachable()
                .WithDescription("Provides meta tags for your content item."));

            return 1;
        }

        public int UpdateFrom1()
        {

            _contentDefinitionManager.AlterPartDefinition("MetaTagsPart", builder => builder
                .WithField(Constants.CustomFieldName, field => field
                    .OfType(typeof(DictionaryField).Name)
                    .WithSetting("Hint", "Apply custom meta tags that will override the defaults applied through defining image, title & description.")
                )
            );

            return 2;
        }
    }
}
