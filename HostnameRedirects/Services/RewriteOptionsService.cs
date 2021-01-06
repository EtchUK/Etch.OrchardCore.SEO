using Castle.Core.Logging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Etch.OrchardCore.SEO.HostnameRedirects.Models;
using System;
using System.Linq;
using System.Net;
using OrchardCore.Admin;
using Microsoft.Extensions.Options;

namespace Etch.OrchardCore.SEO.HostnameRedirects.Services {
    public class RewriteOptionsService : IRewriteOptionsSevice, IRule 
    {
        #region Dependencies

        private readonly AdminOptions _adminOptions;
        private readonly IHostRedirectService _hostRedirectService;
        public ILogger Logger { get; set; } = new NullLogger();

        #endregion

        #region Constructor

        public RewriteOptionsService(IOptions<AdminOptions> adminOptions, IHostRedirectService hostRedirectService) 
        {
            _adminOptions = adminOptions.Value;
            _hostRedirectService = hostRedirectService;
        }

        #endregion

        #region Properties

        public int StatusCode { get; } = (int) HttpStatusCode.MovedPermanently;

        #endregion

        #region Implementation

        public void ApplyRule(RewriteContext context) {
            var hostnameRedirectsSettings = _hostRedirectService.GetSettingsAsync().GetAwaiter().GetResult();

            var url = GetURL(context);

            if (CheckIfIgnored(hostnameRedirectsSettings, url) || context.HttpContext.Request.Method.ToLower() != "get") 
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var consistentRequest = new Uri(url);
            consistentRequest = ValidateWWW(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateNonWWW(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateSSL(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateRedirectToSiteUrl(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateTrailingSlashes(hostnameRedirectsSettings, consistentRequest);

            var consistentRequestUrl = consistentRequest.ToString();

            if (!consistentRequestUrl.Equals(url)) {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCode;
                response.Headers[HeaderNames.Location] = consistentRequestUrl + context.HttpContext.Request.QueryString;
                context.Result = RuleResult.EndResponse;
            }
        }

        #endregion

        #region Helpers

        private string GetURL(RewriteContext context) {
            var request = context.HttpContext.Request;

            return $"{request.Scheme}://{request.Host.Value}{request.PathBase}{request.Path}";
        }

        private bool CheckIfIgnored(HostnameRedirectsSettings settings, string url) {
            if (IsAdmin(url))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(settings.IgnoredUrls)) {
                return false;
            }

            return settings.IgnoredUrls.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Any(x => url.StartsWith(x));
        }

        private bool IsAdmin(string url) 
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var lowerUrl = url.ToLower();
            return lowerUrl.EndsWith($"/{_adminOptions.AdminUrlPrefix.ToLower()}") || lowerUrl.Contains($"/{_adminOptions.AdminUrlPrefix.ToLower()}/");
        }

        private bool IsStaticFile(string path) {
            return path.Contains(".");
        }

        private Uri ValidateWWW(HostnameRedirectsSettings settings, Uri url) {
            if (settings.Redirect != HostnameRedirectModes.NonWWW) {
                return url;
            }

            if (url.Authority.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) {
                return new Uri(url.ToString().Replace("www.", ""));
            }

            return url;
        }

        private Uri ValidateNonWWW(HostnameRedirectsSettings settings, Uri url) {
            if (settings.Redirect != HostnameRedirectModes.WWW) {
                return url;
            }

            if (!url.Authority.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) {
                return new Uri(url.ToString().Replace(string.Format("{0}://", url.Scheme), string.Format("{0}://www.", url.Scheme)));
            }

            return url;
        }

        private Uri ValidateSSL(HostnameRedirectsSettings settings, Uri url) {
            if (!settings.ForceSSL) {
                return url;
            }

            if (url.ToString().StartsWith("http://", StringComparison.OrdinalIgnoreCase)) {
                return new Uri(url.ToString().Replace("http://", "https://"));
            }

            return url;
        }

        private Uri ValidateRedirectToSiteUrl(HostnameRedirectsSettings settings, Uri uri) {
            if (settings.Redirect != HostnameRedirectModes.Domain || string.IsNullOrWhiteSpace(settings.RedirectToSiteUrl)) {
                return uri;
            }

            Uri requestedUri;

            try {
                requestedUri = new Uri(settings.RedirectToSiteUrl);
            } catch (Exception e) {
                Logger.Error(string.Format("{0}, Error parsing RedirectToSiteURL, skipping redirect", e));
                return uri;
            }

            if (requestedUri.Host == null || requestedUri.Host.Equals(uri.Host, StringComparison.OrdinalIgnoreCase)) {
                return uri;
            }

            var builder = new UriBuilder(uri);
            builder.Host = requestedUri.Host;
            return builder.Uri;
        }

        private Uri ValidateTrailingSlashes(HostnameRedirectsSettings settings, Uri uri) {
            if (settings.TrailingSlashes == TrailingSlashesModes.None)
            {
                return uri;
            }

            var lastSegment = uri.Segments.Last();

            // ignore as request is for homepage.
            if (lastSegment == "/" || IsStaticFile(lastSegment))
            {
                return uri;
            }

            var endsWith = lastSegment.EndsWith("/");

            if ((endsWith && settings.TrailingSlashes == TrailingSlashesModes.Append) || (!endsWith && settings.TrailingSlashes == TrailingSlashesModes.Remove))
            {
                return uri;
            }

            var builder = new UriBuilder(uri);

            if (settings.TrailingSlashes == TrailingSlashesModes.Append)
            {
                builder.Path += "/";
            }

            if (settings.TrailingSlashes == TrailingSlashesModes.Remove)
            {
                builder.Path = builder.Path.Substring(0, builder.Path.Length - 1);
            }

            return builder.Uri;
        }



        #endregion
    }
}