using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Utils.Preference;

namespace SMFS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            PreferenceManager.GetInstance("SFS.json");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PreferenceManager.SavePreferences();
        }
    }
}
