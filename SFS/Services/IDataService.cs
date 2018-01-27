using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomDataSource.Model;
using SMFS.Model;

namespace SMFS.Services
{
    public interface IDataService
    {
        Person GetPerson(long id);
        Task<Person> SavePerson(Person person);
        Task<List<Person>> LoadAll();
        Task<List<Transaction>> GetTransactions(long personId);
        Task AddTransaction(long personId, Transaction transaction);
        Task<List<PaymentReportEntry>> GetPaymentReportEntries(long? personId, DateTime? startDate, DateTime? endDate);
        Task<List<PaymentReportEntry>> SampleReportData();
    }
}
