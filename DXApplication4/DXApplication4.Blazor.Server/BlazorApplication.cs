using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;

namespace DXApplication4.Blazor.Server
{
    public class DXApplication4BlazorApplication : BlazorApplication
    {
        public DXApplication4BlazorApplication()
        {
            ApplicationName = "DXApplication4";
            CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            DatabaseVersionMismatch += DXApplication4BlazorApplication_DatabaseVersionMismatch;
        }

        protected override void OnSetupStarted()
        {
            base.OnSetupStarted();
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }

        private void DXApplication4BlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
#if DEBUG
            e.Updater.Update();
            e.Handled = true;
#else
            // Production’da otomatik güncelleme yapma
            throw new InvalidOperationException(
                "The application cannot connect to the specified database, because the database doesn't exist, " +
                "its version is older than that of the application, or its schema does not match the ORM data model structure. " +
                "See https://www.devexpress.com/kb=T367835 for solutions.");
#endif
        }
    }
}
