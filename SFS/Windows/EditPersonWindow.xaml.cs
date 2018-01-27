using System.ComponentModel;
using System.Windows;
using SMFS.Model;
using SMFS.ViewModel;
using Utils.Window;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    public partial class EditPersonWindow
    {
        public EditPersonWindow(Person person)
        {
            InitializeComponent();
            Person = person;
        }

        public Person Person { get; private set; }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Person = null;
            Close();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            var model = (PersonViewModel) DataContext;
            Person.FirstName = model.FirstName;
            Person.LastName = model.LastName;
            Person.Address = model.Address;
            Person.Phone = model.Phone;
            Person.Email = model.Email;
            Person.Notes = model.Notes;
            Person.Hidden = model.Hidden;
            Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveSettings();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.LoadSettings();
            var model = (PersonViewModel) DataContext;
            model.SetPerson(Person);
        }
    }
}
