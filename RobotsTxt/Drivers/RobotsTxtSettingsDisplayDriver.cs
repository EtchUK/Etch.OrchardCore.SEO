using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moov2.OrchardCore.SEO.RobotsTxt.Models;
using Moov2.OrchardCore.SEO.RobotsTxt.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.SEO.RobotsTxt.Drivers
{
    public class RobotsTxtSettingsDisplayDriver :  SectionDisplayDriver<ISite, RobotsTxtSettings>
    {
        #region Constants

        public const string GroupId = "robotstxt";

        #endregion

        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public RobotsTxtSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(RobotsTxtSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageRobotsTxt))
            {
                return null;
            }

            return Initialize<RobosTxtSettingsViewModel>("RobotsTxtSettings_Edit", model =>
            {
                model.Mode = settings.Mode;
                model.CustomContent = settings.CustomContent;
            }).Location("Content:3").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(RobotsTxtSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageRobotsTxt))
            {
                return null;
            }

            if (context.GroupId == GroupId)
            {
                var model = new RobosTxtSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Mode = model.Mode;
                settings.CustomContent = model.CustomContent;
            }

            return await EditAsync(settings, context);
        }

        #endregion
    }
}
