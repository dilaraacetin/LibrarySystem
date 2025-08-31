using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace DXApplication4.Module.Controllers
{
    public class OnlyExcelPdfExportController : ViewController<ListView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            var exportController = Frame.GetController<ExportController>();
            if (exportController == null)
                return;

            foreach (var item in exportController.ExportAction.Items.ToList())
            {
                var id = (item.Id ?? string.Empty).ToLowerInvariant();
                bool keep = id == "exporttopdf" || id == "exporttoxlsx" || id == "exporttoexcel";
                if (!keep)
                {
                    item.Active["OnlyPdfXlsx"] = false;
                }
            }
        }
    }
}
