using System;

namespace SMFS.Model
{
    public class Receipt
    {
        private readonly Person _person;
        private readonly Transaction _transaction;

        public Receipt(Person person, Transaction transaction)
        {
            _person = person;
            _transaction = transaction;
        }

        public string Name => GetName();

        private string GetName()
        {
            return string.Format($"{_person.FirstName.Trim()} {_person.LastName.Trim()}");
        }

        public string Address => _person.Address;

        public string Email => _person.Email;

        public string FirstName => _person.FirstName;

        public bool Hidden => _person.Hidden;

        public string LastName => _person.LastName;

        public string Notes => _person.Notes;

        public string Phone => _person.Phone;

        public long Id => _transaction.Id;

        public DateTime TransactionDate => _transaction.TransactionDate;

        public DateTime Created => _transaction.Created;

        public string Payee => _transaction.Payee;

        public string Reference => _transaction.Reference;

        public decimal Amount => _transaction.Amount;

    }
}