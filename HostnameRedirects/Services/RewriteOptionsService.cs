using Castle.Core.Logging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Moov2.OrchardCore.SEO.HostnameRedirects.Models;
using System;
using System.Linq;
using System.Net;

namespace Moov2.OrchardCore.SEO.HostnameRedirects.Services {
    public class RewriteOptionsService : IRewriteOptionsSevice, IRule {

        #region Dependencies

        private readonly IHostRedirectService _hostRedirectService;

        #endregion

        #region Constructor

        public RewriteOptionsService(IHostRedirectService hostRedirectService) {
            _hostRedirectService = hostRedirectService;
        }

        #endregion

        public int StatusCode { get; } = (int) HttpStatusCode.MovedPermanently;
        public ILogger Logger { get; set; } = new NullLogger();

        public void ApplyRule(RewriteContext context) {
            var hostnameRedirectsSettings = _hostRedirectService.GetSettingsAsync().GetAwaiter().GetResult();

            var url = GetURL(context);

            if (CheckIfIgnored(hostnameRedirectsSettings, url)) {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var consistentRequest = new Uri(url);
            consistentRequest = ValidateWWW(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateNonWWW(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateSSL(hostnameRedirectsSettings, consistentRequest);
            consistentRequest = ValidateRedirectToSiteUrl(hostnameRedirectsSettings, consistentRequest);


            if (!consistentRequest.ToString().Equals(url)) {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCode;
                response.Headers[HeaderNames.Location] = consistentRequest.ToString();
                context.Result = RuleResult.EndResponse;
                return;
            }
        }

        #region Helpers

        private string GetURL(RewriteContext context) {
            var request = context.HttpContext.Request;

            return $"{request.Scheme}://{request.Host.Value}{request.PathBase}{request.Path}{request.QueryString}";
        }

        private bool CheckIfIgnored(HostnameRedirectsSettings settings, string url) {
            if (string.IsNullOrWhiteSpace(settings.IgnoredUrls)) {
                return false;
            }

            return settings.IgnoredUrls.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Any(x => url.StartsWith(x));
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



        #endregion
    }
}