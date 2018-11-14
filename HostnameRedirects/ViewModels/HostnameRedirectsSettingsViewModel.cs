namespace Moov2.OrchardCore.SEO.HostnameRedirects.ViewModels
{
    public class HostnameRedirectsSettingsViewModel
    {
        public int Redirect { get; set; }
        public string RedirectToSiteUrl { get; set; }
        public bool ForceSSL { get; set; }
        public string IgnoredUrls { get; set; }
    }
}
