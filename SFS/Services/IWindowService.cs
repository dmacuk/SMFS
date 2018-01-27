using SMFS.Model;

namespace SMFS.Services
{
    public interface IWindowService
    {
        Person EditPerson(Person person);
        Person FindPerson(IDataService dataService);
        Transaction AddTransaction();
        void PaymentsReport();
        void PrintTransaction(Receipt receipt);
    }
}