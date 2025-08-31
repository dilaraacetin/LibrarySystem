using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DXApplication4.Module.BusinessObjects; 
using DXApplication4.Module.Dashboards;     
namespace DXApplication4.Module.Controllers
{
    public class LibraryDashboardController : ViewController
    {
        public LibraryDashboardController()
        {
            var showDashboard = new SimpleAction(this, "ShowLibraryDashboard", PredefinedCategory.View)
            {
                Caption = "Dashboard",
                ImageName = "BO_Dashboard"
            };
            showDashboard.Execute += ShowDashboard_Execute;
        }

        private void ShowDashboard_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var composite = (CompositeObjectSpace)Application.CreateObjectSpace(typeof(LibraryDashboard));

            var efOs = Application.CreateObjectSpace(typeof(Book));
            composite.AdditionalObjectSpaces.Add(efOs);

            var dashboard = composite.CreateObject<LibraryDashboard>();

            var dv = Application.CreateDetailView(composite, dashboard, true);
            e.ShowViewParameters.CreatedView = dv;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
        }
    }
}
