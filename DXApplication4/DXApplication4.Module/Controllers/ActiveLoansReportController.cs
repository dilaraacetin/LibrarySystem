using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace DXApplication4.Module.Controllers
{
    public class ActiveLoansReportController : ViewController
    {
        public ActiveLoansReportController()
        {
            var reportAction = new SimpleAction(
                this,
                "Report_WhoHasWhichBook",
                PredefinedCategory.Reports 
            )
            {
                Caption = "Hangi kitap kimde?",
                ImageName = "BO_Report"
            };
            reportAction.Execute += ReportAction_Execute;
        }

        private void ReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var os = Application.CreateObjectSpace(typeof(BusinessObjects.Loan));
            var listViewId = Application.FindListViewId(typeof(BusinessObjects.Loan));
            var cs = Application.CreateCollectionSource(os, typeof(BusinessObjects.Loan), listViewId);

            cs.Criteria["ActiveOnly"] = CriteriaOperator.Parse("[Durum] = 0");

            var view = Application.CreateListView(listViewId, cs, true);
            e.ShowViewParameters.CreatedView = view;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow; 
        }
    }
}
