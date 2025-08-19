using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Utils;

namespace DXApplication4.Win
{
    public class DXApplication4WindowsFormsApplication : WinApplication
    {
        public DXApplication4WindowsFormsApplication()
        {
            SplashScreen = new DXSplashScreen(typeof(XafSplashScreen), new DefaultOverlayFormOptions());
            ApplicationName = "DXApplication4";
            CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            UseOldTemplates = false;

            DatabaseVersionMismatch += DXApplication4WindowsFormsApplication_DatabaseVersionMismatch;
            CustomizeLanguagesList += DXApplication4WindowsFormsApplication_CustomizeLanguagesList;
        }

        protected override void OnSetupStarted()
        {
            base.OnSetupStarted();
#if DEBUG
            if (CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }

        private void DXApplication4WindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e)
        {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if (userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1)
            {
                e.Languages.Add(userLanguageName);
            }
        }

        private void DXApplication4WindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
#if DEBUG
            e.Updater.Update();
            e.Handled = true;
#else
            // Production’da otomatik güncelleme yapma
            throw new InvalidOperationException(
                "The application cannot connect to the specified database because it is missing, " +
                "outdated, or incompatible with the current data model. See https://www.devexpress.com/kb=T367835 for solutions.");
#endif
        }
    }
}
