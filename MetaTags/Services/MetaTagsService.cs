using Microsoft.AspNetCore.Http;
using Etch.OrchardCore.SEO.MetaTags.Models;
using OrchardCore.Media;
using OrchardCore.ResourceManagement;
using System.Linq;
using Etch.OrchardCore.Fields.Dictionary.Models;
using System.Collections.Generic;
using System;

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

        public void RegisterCustom(IList<DictionaryItem> customMetaTags)
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

        public void RegisterDefaults(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null)
        {
            if (!string.IsNullOrWhiteSpace(metaTags.Title) && !HasCustomMetaTag(customMetaTags, "title"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description) && !HasCustomMetaTag(customMetaTags, "description"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "description", Content = metaTags.Description });
            }
        }

        public void RegisterOpenGraph(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null)
        {
            if (!HasCustomMetaTag(customMetaTags, "og:type"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "og:type", Content = "website" });
            }

            if (!HasCustomMetaTag(customMetaTags, "og:url"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "og:url", Content = GetPageUrl() });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Title) && !HasCustomMetaTag(customMetaTags, "og:title"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "og:title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description) && !HasCustomMetaTag(customMetaTags, "og:description"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "og:description", Content = metaTags.Description });
            }

            if (metaTags.Images != null && metaTags.Images.Any() && !HasCustomMetaTag(customMetaTags, "og:image"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "og:image", Content = GetMediaUrl(metaTags.Images[0]) });
            }
        }

        public void RegisterTwitter(MetaTagsPart metaTags, IList<DictionaryItem> customMetaTags = null)
        {
            if (!HasCustomMetaTag(customMetaTags, "twitter:card"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "twitter:card", Content = "summary_large_image" });
            }

            if (!HasCustomMetaTag(customMetaTags, "twitter:url"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "twitter:url", Content = GetPageUrl() });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Title) && !HasCustomMetaTag(customMetaTags, "twitter:title"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "twitter:title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description) && !HasCustomMetaTag(customMetaTags, "twitter:description"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "twitter:description", Content = metaTags.Description });
            }

            if (metaTags.Images != null && metaTags.Images.Any() && !HasCustomMetaTag(customMetaTags, "twitter:image"))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Property = "twitter:image", Content = GetMediaUrl(metaTags.Images[0]) });
            }
        }

        #endregion

        #region Private Methods

        private string GetHostUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }

        public string GetMediaUrl(string path)
        {
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


        #endregion
    }
}
