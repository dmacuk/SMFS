using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace CustomDataSource.Model
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PaymentReportEntry : IComparable<PaymentReportEntry>
    {

        public PaymentReportEntry(long id, string name, string address, string phone, string email, DateTime transactionDate, string payee, string reference, decimal amount)
        {
            var xaddress = address;
            Amount = amount;
            Email = email;
            Id = id;
            var xname = name;
            Payee = payee;
            Phone = phone;
            Reference = reference;
            TransactionDate = transactionDate;
            Details = xname + Environment.NewLine + xaddress;
        }

        public PaymentReportEntry(IDataRecord rs)
        {
            Id = (long)rs["id"];
            var xname = (string)rs["lastname"]+", "+(string)rs["firstname"];
            var xaddress = (string)rs["address"];
            Phone = (string)rs["phone"];
            Email = (string)rs["email"];
            TransactionDate = (DateTime)rs["transactiondate"];
            Payee = (string)rs["payee"];
            Reference = (string)rs["reference"];
            Amount = (decimal)rs["amount"];
            Details = xname + Environment.NewLine + xaddress;
        }

        public decimal Amount { get; }
        public string Email { get; }
        public long Id { get; }
        public string Payee { get; }
        public string Phone { get; }
        public string Reference { get; }
        public DateTime TransactionDate { get; }
        public string Details { get; set; }

        public int CompareTo(PaymentReportEntry other)
        {
            var result = string.Compare(Details, other.Details, StringComparison.Ordinal);
            if (result == 0) result = TransactionDate.CompareTo(other.TransactionDate);
            return result;
        }
    }
}