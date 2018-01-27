using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SMFS.Model
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Transaction : IComparable<Transaction>
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime Created { get; set; }
        public string Payee { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }

        public Transaction()
        {
            Id = 0;
            Created = DateTime.Now;
            Payee = "";
            Reference = "";
            Amount = 0.00m;
        }

        public Transaction(IDataRecord rs)
        {
            Id = (long)rs["id"];
            TransactionDate = (DateTime)rs["transactiondate"];
            Payee = (string)rs["payee"];
            Reference = (string)rs["reference"];
            Amount = (decimal)rs["amount"];
            Created = (DateTime)rs["Created"];

        }

        public int CompareTo(Transaction other)
        {
            var result = TransactionDate.Date.CompareTo(other.TransactionDate.Date);
            if (result == 0) result = Created.CompareTo(other.Created);
            return result;

        }

        protected bool Equals(Transaction other)
        {
            return Id == other.Id
                && TransactionDate.Equals(other.TransactionDate)
                && Created.Equals(other.Created)
                && string.Equals(Payee, other.Payee)
                && string.Equals(Reference, other.Reference)
                && Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Transaction)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ TransactionDate.GetHashCode();
                hashCode = (hashCode * 397) ^ Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (Payee != null ? Payee.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Reference != null ? Reference.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                return hashCode;
            }
        }
    }
}
