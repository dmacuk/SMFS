using SMFS.Model;
using SMFS.Windows;

namespace SMFS.Services.Implementations
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class WindowService:IWindowService
    {
        public Person EditPerson(Person person)
        {
            var wind = new EditPersonWindow(person);
            wind.ShowDialog();
            return wind.Person;
        }

        public Person FindPerson(IDataService dataService)
        {
            var wind = new FindPersonWindow(dataService);
            wind.ShowDialog();
            return wind.Person;
        }

        public Transaction AddTransaction()
        {
            var wind = new TransactionWindow();
            wind.ShowDialog();
            return wind.Transaction;
        }

        public void PaymentsReport()
        {
            var wind = new PaymentsReportWindow();
            wind.ShowDialog();
        }

        public void PrintTransaction(Receipt receipt)
        {
            var wind = new Windows.PaymentReceiptWindow(receipt);
            wind.ShowDialog();
        }
    }
}
