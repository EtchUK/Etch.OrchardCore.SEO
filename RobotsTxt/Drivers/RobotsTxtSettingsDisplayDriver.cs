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

        #region Constructor

        public RobotsTxtSettingsDisplayDriver()
        {

        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(RobotsTxtSettings settings, BuildEditorContext context)
        {
            return Initialize<RobosTxtSettingsViewModel>("RobotsTxtSettings_Edit", model =>
            {
                model.Mode = settings.Mode;
            }).Location("Content:3").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(RobotsTxtSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == GroupId)
            {
                var model = new RobosTxtSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Mode = model.Mode;
            }

            return await EditAsync(settings, context);
        }

        #endregion
    }
}
