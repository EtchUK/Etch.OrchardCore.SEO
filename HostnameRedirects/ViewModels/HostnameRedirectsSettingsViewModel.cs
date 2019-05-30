namespace Etch.OrchardCore.SEO.HostnameRedirects.ViewModels
{
    public class HostnameRedirectsSettingsViewModel
    {
        public int Redirect { get; set; }
        public string RedirectToSiteUrl { get; set; }
        public bool ForceSSL { get; set; }
        public string IgnoredUrls { get; set; }
        public int TrailingSlashes { get; set; }
    }
}
