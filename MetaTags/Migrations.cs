using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using Etch.OrchardCore.Fields.Dictionary.Fields;
using Etch.OrchardCore.Fields.Dictionary.Settings;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using System.Threading.Tasks;
using Etch.OrchardCore.SEO.MetaTags.Services;
using Etch.OrchardCore.SEO.MetaTags.Models;

namespace Etch.OrchardCore.SEO.MetaTags
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IMigrateMetaTagsPartService _migrateMetaTagsPartService;

        public Migrations(IContentDefinitionManager contentDefinitionManager, IMigrateMetaTagsPartService migrateMetaTagsPartService)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _migrateMetaTagsPartService = migrateMetaTagsPartService;
        }

        public async Task<int> CreateAsync()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .Attachable()
                .WithDescription("Provides meta tags for your content item."));

            await AddMetaTagFieldsAsync();

            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.CustomFieldName, field => field
                    .OfType(nameof(DictionaryField))
                    .WithDisplayName(Constants.CustomFieldName)
                    .WithPosition("5")
                    .WithSettings(new DictionaryFieldSettings
                    {
                        Hint = "Apply custom meta tags that will override the defaults applied through defining image, title & description."
                    })
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.NoIndexFieldName, field => field
                    .OfType(nameof(BooleanField))
                    .WithDisplayName(Constants.NoIndexFieldDisplayName)
                    .WithPosition("4")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Label = "Hide from search engines",
                        Hint = "Prevent page from appearing in search engines using 'noindex' meta tag.",
                    })
                )
            );

            return 5;
        }

        public async Task<int> UpdateFrom1Async()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.CustomFieldName, field => field
                    .OfType(nameof(DictionaryField))
                    .WithDisplayName(Constants.CustomFieldName)
                    .WithPosition("5")
                    .WithSettings(new DictionaryFieldSettings
                    {
                        Hint = "Apply custom meta tags that will override the defaults applied through defining image, title & description."
                    })
                )
            );

            return 2;
        }

        public async Task<int> UpdateFrom2Async()
        {
            await AddMetaTagFieldsAsync();
            return 3;
        }

        public async Task<int> UpdateFrom3Async()
        {
            await _migrateMetaTagsPartService.MigrateAsync();
            return 4;
        }

        public async Task<int> UpdateFrom4Async()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.NoIndexFieldName, field => field
                    .OfType(nameof(BooleanField))
                    .WithDisplayName(Constants.NoIndexFieldDisplayName)
                    .WithPosition("4")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Label = "Hide from search engines",
                        Hint = "Prevent page from appearing in search engines using 'noindex' meta tag.",
                    })
                )
            );

            return 5;
        }

        public async Task<int> UpdateFrom5Async()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .WithField(Constants.Defaults.Title, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName(Constants.Defaults.Title)
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Keep your title around 60 characters and put the keywords you’re focusing on first. Don't go overboard with keywords, at most stick to two.",
                    })
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .WithField(Constants.Defaults.Description, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName(Constants.Defaults.Description)
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "The meta description often serves as a pitch to people who find your website on Google or social media sites. While it's not required and Google can use text from you website instead of what you specify in the meta data, it's better to control the description text where you can.",
                    })
                    .WithEditor("TextArea")
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .WithField(Constants.Defaults.Image, field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName(Constants.Defaults.Image)
                    .WithPosition("3")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "With the visual nature of the web your Meta Tag Image is the most valuable graphic content you can create to encourage users to click and visit your website. Recommend 1200×628.",
                        Multiple = false
                    })
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .WithField(Constants.Defaults.Custom, field => field
                    .OfType(nameof(DictionaryField))
                    .WithDisplayName(Constants.Defaults.Custom)
                    .WithPosition("5")
                    .WithSettings(new DictionaryFieldSettings
                    {
                        Hint = "Apply custom meta tags that will override the defaults applied through defining image, title & description."
                    })
                )
            );

            await _contentDefinitionManager.AlterTypeDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .Stereotype("CustomSettings")
                .DisplayedAs("Default Meta Tags")
                .WithPart(Constants.Defaults.ContentType));

            return 6;
        }

        public async Task<int> UpdateFrom6Async()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(Constants.Defaults.ContentType, builder => builder
                .WithField(Constants.Defaults.Image, field => field
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "Due to the visual nature of the web, your Meta Tag Image is the most valuable graphic content you can create to encourage users to click and visit your website. 1200×630 is the recommended size. If you don't have an image of these exact dimensions, select something large enough so that the site can crop it to this ratio for you.",
                        Multiple = false
                    })
                )
            );

            return 7;
        }

        private async Task AddMetaTagFieldsAsync()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.TitleFieldName, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName(Constants.TitleFieldDisplayName)
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Keep your title around 60 characters and put the keywords you’re focusing on first. Don't go overboard with keywords, at most stick to two.",
                    })
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.DescriptionFieldName, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName(Constants.DescriptionFieldDisplayName)
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "The meta description often serves as a pitch to people who find your website on Google or social media sites. While it's not required and Google can use text from you website instead of what you specify in the meta data, it's better to control the description text where you can.",
                    })
                    .WithEditor("TextArea")
                )
            );

            await _contentDefinitionManager.AlterPartDefinitionAsync(nameof(MetaTagsPart), builder => builder
                .WithField(Constants.ImageFieldName, field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName(Constants.ImageFieldDisplayName)
                    .WithPosition("3")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "With the visual nature of the web your Meta Tag Image is the most valuable graphic content you can create to encourage users to click and visit your website. Recommend 1200×628.",
                        Multiple = false
                    })
                )
            );
        }
    }
}
