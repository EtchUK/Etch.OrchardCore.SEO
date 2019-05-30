using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Etch.OrchardCore.SEO.HostnameRedirects.Models;
using Etch.OrchardCore.SEO.HostnameRedirects.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.HostnameRedirects.Drivers
{
    public class HostnameRedirectsSettingsDisplayDriver :  SectionDisplayDriver<ISite, HostnameRedirectsSettings>
    {
        #region Constants

        public const string GroupId = "hostnameredirects";

        #endregion

        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public HostnameRedirectsSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(HostnameRedirectsSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageHostnameRedirects))
            {
                return null;
            }

            return Initialize<HostnameRedirectsSettingsViewModel>("HostnameRedirectsSettings_Edit", model =>
            {
                model.Redirect = settings.Redirect;
                model.ForceSSL = settings.ForceSSL;
                model.RedirectToSiteUrl = settings.RedirectToSiteUrl;
                model.IgnoredUrls = settings.IgnoredUrls;
                model.TrailingSlashes = settings.TrailingSlashes;
            }).Location("Content:3").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(HostnameRedirectsSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageHostnameRedirects))
            {
                return null;
            }

            if (context.GroupId == GroupId)
            {
                var model = new HostnameRedirectsSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Redirect = model.Redirect;
                settings.ForceSSL = model.ForceSSL;
                settings.RedirectToSiteUrl = model.RedirectToSiteUrl;
                settings.IgnoredUrls = model.IgnoredUrls;
                settings.TrailingSlashes = model.TrailingSlashes;
            }

            return await EditAsync(settings, context);
        }

        #endregion
    }
}
