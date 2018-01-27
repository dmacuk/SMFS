using System.ComponentModel;
using System.Windows;
using SMFS.Model;
using SMFS.Services;
using SMFS.ViewModel;
using Utils.Window;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for FindPersonWindow.xaml
    /// </summary>
    public partial class FindPersonWindow : Window
    {
        private readonly IDataService _dataService;
        private readonly FindPersonViewModel _model;

        public FindPersonWindow(IDataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            _model = (FindPersonViewModel) DataContext;
        }

        public Person Person { get; private set; }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.LoadSettings();
            await _model.LoadData(_dataService);
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveSettings();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            var person = _model.Selected;
            if (person != null)
            {
                Person = person;
                Close();
                return;
            }
            MessageBox.Show("No person selected", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Person = null;
            Close();
        }
    }
}
