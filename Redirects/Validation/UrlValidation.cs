using System;

namespace Etch.OrchardCore.SEO.Redirects.Validation
{
    public static class UrlValidation
    {
        #region Constants / Statics

        public static char[] InvalidCharactersForFromUrl = ":?#[]@!$&'()*+,.;=<>\\|%".ToCharArray();
        public static char[] InvalidCharactersForToUrl = "?#[]@!$&'()*+,;=<>\\|%".ToCharArray();

        public const int MaxPathLength = 1024;

        #endregion

        public static bool IsRelativeUrl(string url)
        {
            Uri result;
            return !Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static bool IsWithinLengthLimit(string url)
        {
            return url == null || url.Length <= MaxPathLength;
        }

        public static bool ValidFromUrl(string fromUrl)
        {
            return fromUrl != null && fromUrl.IndexOfAny(InvalidCharactersForFromUrl) == -1 && fromUrl.IndexOf(' ') == -1;
        }

        public static bool ValidToUrl(string toUrl)
        {
            return toUrl != null && toUrl.IndexOfAny(InvalidCharactersForToUrl) == -1 && toUrl.IndexOf(' ') == -1;
        }
    }
}
