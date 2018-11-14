using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moov2.OrchardCore.SEO.HostnameRedirects.Models;
using Moov2.OrchardCore.SEO.HostnameRedirects.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.HostnameRedirects.Drivers
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
            }

            return await EditAsync(settings, context);
        }

        #endregion
    }
}
