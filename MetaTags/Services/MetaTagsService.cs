using Etch.OrchardCore.Fields.Dictionary.Fields;
using Etch.OrchardCore.Fields.Dictionary.Models;
using Etch.OrchardCore.SEO.MetaTags.Extensions;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Etch.OrchardCore.SEO.MetaTags.Settings;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Liquid;
using OrchardCore.Media;
using OrchardCore.Media.Fields;
using OrchardCore.ResourceManagement;
using OrchardCore.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public class MetaTagsService : IMetaTagsService
    {
        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IResourceManager _resourceManager;
        private readonly ISiteService _siteService;

        #endregion

        #region Private Properties

        private ISite _site;

        #endregion

        #region Constructor

        public MetaTagsService(IHttpContextAccessor httpContextAccessor, ILiquidTemplateManager liquidTemplateManager, IMediaFileStore mediaFileStore, IResourceManager resourceManager, ISiteService siteService)
        {
            _httpContextAccessor = httpContextAccessor;
            _liquidTemplateManager = liquidTemplateManager;
            _mediaFileStore = mediaFileStore;
            _resourceManager = resourceManager;
            _siteService = siteService;
        }

        #endregion

        #region Implementation

        public async Task RegisterAsync(MetaTagsPart part)
        {
            _site = await _siteService.GetSiteSettingsAsync();

            var defaultMetaTags = await GetDefaultMetaTagsAsync(_site, part);

            var customMetaTags = part.GetCustom(defaultMetaTags.Custom);
            var description = part.GetDescription() ?? defaultMetaTags.Description;
            var imagePath = part.GetImage() ?? defaultMetaTags.ImagePath;
            var noIndex = part.GetNoIndex();
            var title = part.GetTitle() ?? defaultMetaTags.Title;

            if (string.IsNullOrWhiteSpace(title))
            {
                title = part.ContentItem.DisplayText;
            }

            RegisterCustom(customMetaTags);
            RegisterDefaults(title, description, customMetaTags);
            RegisterNoIndex(noIndex, customMetaTags);
            RegisterOpenGraph(title, description, imagePath, customMetaTags);
            RegisterTwitter(title, description, imagePath, customMetaTags);
        }

        #endregion

        #region Helpers

        private void AddMetaEntry(string name, string content, IList<DictionaryItem> customMetaTags)
        {
            if (!string.IsNullOrWhiteSpace(content) && !HasCustomMetaTag(customMetaTags, name))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = name, Content = content });
            }
        }

        private async Task<DefaultMetaTags> GetDefaultMetaTagsAsync(ISite siteSettings, MetaTagsPart part)
        {
            var settings = siteSettings.As<ContentItem>("DefaultMetaTags")?.Get<ContentPart>("DefaultMetaTags");

            if (settings == null)
            {
                return new DefaultMetaTags();
            }

            var imagePath = settings.Get<MediaField>("Image")?.Paths?.FirstOrDefault() ?? string.Empty;
            var values = new Dictionary<string, FluidValue>() { ["ContentItem"] = new ObjectValue(part.ContentItem) };

            return new DefaultMetaTags
            {
                Custom = settings.Get<DictionaryField>("Custom")?.Data,
                Description = await _liquidTemplateManager.RenderStringAsync(settings.Get<TextField>("Description")?.Text, NullEncoder.Default, null, values),
                ImagePath = imagePath,
                Title = await _liquidTemplateManager.RenderStringAsync(settings.Get<TextField>("Title")?.Text, NullEncoder.Default, null, values)
            };
        }

        private string GetHostUrl()
        {
            if (!string.IsNullOrWhiteSpace(_site.BaseUrl))
            {
                return _site.BaseUrl;
            }

            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }

        public string GetMediaUrl(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            var imageUrl = _mediaFileStore.MapPathToPublicUrl(path);
            return imageUrl.StartsWith("http") ? imageUrl : $"{GetHostUrl()}{imageUrl}";
        }

        private string GetPageUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{GetHostUrl()}{request.PathBase}{request.Path}";
        }

        private static bool HasCustomMetaTag(IList<DictionaryItem> customMetaTags, string name)
        {
            if (customMetaTags == null)
            {
                return false;
            }

            return customMetaTags.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        private void RegisterCustom(IList<DictionaryItem> customMetaTags)
        {
            if (customMetaTags == null)
            {
                return;
            }

            foreach (var metaTag in customMetaTags)
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = metaTag.Name, Content = metaTag.Value });
            }
        }

        private void RegisterDefaults(string title, string description, IList<DictionaryItem> customMetaTags = null)
        {
            AddMetaEntry("title", title, customMetaTags);
            AddMetaEntry("description", description, customMetaTags);
        }

        private void RegisterNoIndex(bool noIndex, IList<DictionaryItem> customMetaTags = null)
        {
            if (!noIndex)
            {
                return;
            }

            AddMetaEntry("robots", "noindex", customMetaTags);
        }

        private void RegisterOpenGraph(string title, string description, string imagePath, IList<DictionaryItem> customMetaTags = null)
        {
            AddMetaEntry("og:type", "website", customMetaTags);
            AddMetaEntry("og:url", GetPageUrl(), customMetaTags);
            AddMetaEntry("og:title", title, customMetaTags);
            AddMetaEntry("og:description", description, customMetaTags);
            AddMetaEntry("og:image", GetMediaUrl(imagePath), customMetaTags);

            _resourceManager.RegisterMeta(new MetaEntry { Name = "image", Property = "og:image", Content = GetMediaUrl(imagePath) });
        }

        private void RegisterTwitter(string title, string description, string imagePath, IList<DictionaryItem> customMetaTags = null)
        {
            AddMetaEntry("twitter:card", "summary_large_image", customMetaTags);
            AddMetaEntry("twitter:url", GetPageUrl(), customMetaTags);
            AddMetaEntry("twitter:title", title, customMetaTags);
            AddMetaEntry("twitter:description", description, customMetaTags);
            AddMetaEntry("twitter:image", GetMediaUrl(imagePath), customMetaTags);
        }

        #endregion
    }
}
