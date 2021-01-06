using Etch.OrchardCore.Fields.Dictionary.Models;
using Etch.OrchardCore.SEO.MetaTags.Extensions;
using Etch.OrchardCore.SEO.MetaTags.Models;
using Microsoft.AspNetCore.Http;
using OrchardCore.Media;
using OrchardCore.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.SEO.MetaTags.Services
{
    public class MetaTagsService : IMetaTagsService
    {
        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IResourceManager _resourceManager;

        #endregion

        #region Constructor

        public MetaTagsService(IHttpContextAccessor httpContextAccessor, IMediaFileStore mediaFileStore, IResourceManager resourceManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _mediaFileStore = mediaFileStore;
            _resourceManager = resourceManager;
        }

        #endregion

        #region Implementation

        public void Register(MetaTagsPart part)
        {
            var customMetaTags = part.GetCustom();
            var description = part.GetDescription();
            var imagePath = part.GetImage();
            var noIndex = part.GetNoIndex();
            var title = part.GetTitle();

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

        private string GetHostUrl()
        {
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

        private bool HasCustomMetaTag(IList<DictionaryItem> customMetaTags, string name)
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
