using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SMFS.ViewModel;
using Utils.Window;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.LoadSettings(true);
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            ViewModelLocator.Cleanup();
            this.SaveSettings(true);
        }

        private void PrintTransaction(object sender, MouseButtonEventArgs e)
        {
            var model = (MainViewModel) DataContext;
            model.PrintTransaction();
        }
    }
}