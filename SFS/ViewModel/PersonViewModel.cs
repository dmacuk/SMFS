using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using SMFS.Model;

namespace SMFS.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PersonViewModel:ViewModelBase
    {
        private string _address;
        private string _email;
        private string _firstName;
        private bool _hidden;
        private string _lastName;
        private string _notes;
        private string _phone;



        public string Address
        {
            get => _address;
            set => Set(() => Address, ref _address, value);
        }

        public string Email
        {
            get => _email;
            set => Set(() => Email, ref _email, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(() => FirstName, ref _firstName, value);
        }


        public bool Hidden
        {
            get => _hidden;
            set => Set(() => Hidden, ref _hidden, value);
        }

        public string LastName
        {
            get => _lastName;
            set => Set(() => LastName, ref _lastName, value);
        }

        public string Notes
        {
            get => _notes;
            set => Set(() => Notes, ref _notes, value);
        }

        public string Phone
        {
            get => _phone;
            set => Set(() => Phone, ref _phone, value);
        }

        public void SetPerson(Person person)
        {
            Address = person.Address;
            Email = person.Email;
            FirstName = person.FirstName;
            Hidden = person.Hidden;
            LastName = person.LastName;
            Notes = person.Notes;
            Phone = person.Phone;
        }
    }
}