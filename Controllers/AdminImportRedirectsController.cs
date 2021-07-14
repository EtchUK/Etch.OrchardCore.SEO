using Etch.OrchardCore.SEO.Redirects.Import.Models;
using Etch.OrchardCore.SEO.Redirects.Import.Services;
using Etch.OrchardCore.SEO.Redirects.Import.ViewModels;
using FluentExcel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.SEO.Controllers
{
    [Admin]
    [Feature("Etch.OrchardCore.SEO.Redirects.Import")]
    public class AdminImportRedirectsController : Controller
    {
        #region Dependencies

        private readonly IImportRedirectsService _importRedirectsService;
        private readonly INotifier _notifier;

        #endregion Dependencies

        #region PublicProperties

        public IHtmlLocalizer T { get; }

        #endregion PublicProperties

        #region Constructor

        public AdminImportRedirectsController(
                IHtmlLocalizer<AdminImportRedirectsController> localizer,
                IImportRedirectsService importRedirectsService,
                INotifier notifier
            )
        {
            T = localizer;
            _importRedirectsService = importRedirectsService;
            _notifier = notifier;
        }

        #endregion Constructor

        #region Actions

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion Index

        #region Import

        [HttpPost]
        public async Task<ActionResult> Import(ImportRedirectsIndexViewModel model)
        {
            IEnumerable<ImportRedirectRow> rows;

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            using (var stream = model.ImportedRedirectsFile.OpenReadStream())
            {
                rows = Excel.Load<ImportRedirectRow>(stream, "Sheet1");
            }

            var result = await _importRedirectsService.ImportRedirectsAsync(rows);

            _notifier.Success(T["Successfully imported {0} redirects,", result.Success]);

            if (result.Skipped.Any())
            {
                _notifier.Warning(T["Skipped rows {0}.", string.Join(", ", result.Skipped)]);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion Import

        #endregion Actions
    }
}
