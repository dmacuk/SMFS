using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SMFS.Model;
using SMFS.Services;

// ReSharper disable All

namespace SMFS.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IWindowService _windowService;
        private string _address;
        private bool _busy;
        private string _email;
        private string _firstName;
        private bool _hidden;
        private string _lastName;
        private string _notes;
        private Person _person;
        private string _phone;
        private decimal _total;
        private ObservableCollection<Transaction> _transactions;

        public MainViewModel(IDataService dataService, IWindowService windowService)
        {
            _dataService = dataService;
            _windowService = windowService;
            Person person = new Person() {FirstName = "David", LastName = "McCallum", Address = "My Address"};

            Transaction transaction = new Transaction()
            {
                TransactionDate = DateTime.Now,
                Amount = 1234.45m,
                Payee = "The Payee",
                Reference = "The reference"
            };
            var receipt = new Receipt(person, transaction);
//            _windowService.PrintTransaction(receipt);
        }

        public static ICommand AboutCommand => new RelayCommand(() =>
            MessageBox.Show("© 2017 DM Software Services", "About", MessageBoxButton.OK, MessageBoxImage.Information));

        public ICommand AddCommand => new RelayCommand(async () => { await EditPerson(new Person()); });

        public string Address
        {
            get => _address;
            set => Set(() => Address, ref _address, value);
        }

        public ICommand AddTransactionCommand =>
            new RelayCommand(async () => { await AddTransaction(); }, () => _person != null);

        public bool Busy
        {
            get => _busy;
            set => Set(() => Busy, ref _busy, value);
        }

        public ICommand EditCommand => new RelayCommand(async () => { await EditPerson(_person); });

        public string Email
        {
            get => _email;
            set => Set(() => Email, ref _email, value);
        }

        public ICommand FindCommand => new RelayCommand(async () => { await FindPerson(); });

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

        public ICommand PaymentsReportCommand => new RelayCommand(() => { _windowService.PaymentsReport(); });

        public string Phone
        {
            get => _phone;
            set => Set(() => Phone, ref _phone, value);
        }

        public decimal Total
        {
            get => _total;
            set { Set(() => Total, ref _total, value); }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set { Set(() => Transactions, ref _transactions, value); }
        }

        public Transaction Transaction { get; set; }

        private async Task AddTransaction()
        {
            var transaction = _windowService.AddTransaction();
            if (transaction == null) return;
            await _dataService.AddTransaction(_person.Id, transaction);
            var insertPoint = InsertPoint(transaction);
            Transactions.Insert(insertPoint, transaction);
            _windowService.PrintTransaction(new Receipt(_person, transaction));
        }

        private int InsertPoint(Transaction transaction)
        {
            var count = 0;
            foreach (var t in Transactions)
            {
                if (t.TransactionDate < transaction.TransactionDate)
                {
                    count++;
                    continue;
                }

                if (t.TransactionDate == transaction.TransactionDate)
                {
                    if (t.Created < transaction.Created)
                    {
                        count++;
                        continue;
                    }
                }

                break;
            }

            return count;
        }

        private async Task EditPerson(Person person)
        {
            var result = _windowService.EditPerson(person);
            if (result == null) return;
            Busy = true;
            _person = await _dataService.SavePerson(result);
            await UpdateGui();
            Busy = false;
        }

        private async Task FindPerson()
        {
            var result = _windowService.FindPerson(_dataService);
            if (result == null) return;
            _person = result;
            await UpdateGui();
        }

        private async Task UpdateGui()
        {
            FirstName = _person.FirstName;
            LastName = _person.LastName;
            Address = _person.Address;
            Phone = _person.Phone;
            Email = _person.Email;
            Notes = _person.Notes;
            Hidden = _person.Hidden;
            Transactions = new ObservableCollection<Transaction>(await _dataService.GetTransactions(_person.Id));
            Transactions.CollectionChanged += UpdateTotal;
            UpdateTotal(null, null);
        }

        private void UpdateTotal(object sender, NotifyCollectionChangedEventArgs e)
        {
            Total = Transactions.Sum(t => t.Amount);
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
        public void PrintTransaction()
        {
            if (Transaction == null) return;
            Receipt receipt = new Receipt(_person, Transaction);
            _windowService.PrintTransaction(receipt);
        }
    }
}