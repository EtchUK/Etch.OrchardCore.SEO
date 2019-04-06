using Microsoft.AspNetCore.Http;
using Moov2.OrchardCore.SEO.MetaTags.Models;
using OrchardCore.Media;
using OrchardCore.ResourceManagement;
using System.Linq;

namespace Moov2.OrchardCore.SEO.MetaTags.Services
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

        public void RegisterDefaults(MetaTagsPart metaTags)
        {
            if (!string.IsNullOrWhiteSpace(metaTags.Title))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "description", Content = metaTags.Description });
            }
        }

        public void RegisterOpenGraph(MetaTagsPart metaTags)
        {
            _resourceManager.RegisterMeta(new MetaEntry { Name = "og:type", Content = "website" });
            _resourceManager.RegisterMeta(new MetaEntry { Name = "og:url", Content = GetPageUrl() });

            if (!string.IsNullOrWhiteSpace(metaTags.Title))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "og:title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "og:description", Content = metaTags.Description });
            }

            if (metaTags.Images.Any())
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "og:image", Content = $"{GetHostUrl()}{_mediaFileStore.MapPathToPublicUrl(metaTags.Images[0])}" });
            }
        }

        public void RegisterTwitter(MetaTagsPart metaTags)
        {
            _resourceManager.RegisterMeta(new MetaEntry { Name = "twitter:card", Content = "summary_large_image" });
            _resourceManager.RegisterMeta(new MetaEntry { Name = "twitter:url", Content = GetPageUrl() });

            if (!string.IsNullOrWhiteSpace(metaTags.Title))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "twitter:title", Content = metaTags.Title });
            }

            if (!string.IsNullOrWhiteSpace(metaTags.Description))
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "twitter:description", Content = metaTags.Description });
            }

            if (metaTags.Images.Any())
            {
                _resourceManager.RegisterMeta(new MetaEntry { Name = "twitter:image", Content = $"{GetHostUrl()}{_mediaFileStore.MapPathToPublicUrl(metaTags.Images[0])}" });
            }
        }

        #endregion

        #region Private Methods

        private string GetHostUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }

        private string GetPageUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{GetHostUrl()}{request.PathBase}{request.Path}";
        }

        #endregion
    }
}
