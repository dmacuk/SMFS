using System;
using GalaSoft.MvvmLight;

namespace SMFS.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TransactionViewModel:ViewModelBase
    {
        private DateTime _date;
        private string _payee;
        private string _reference;
        private double _amount;

        public DateTime Date
        {
            get => _date;
            set { Set(()=>Date, ref _date, value); }
        }

        public string Payee
        {
            get => _payee;
            set { Set(()=>Payee, ref _payee, value); }
        }

        public string Reference
        {
            get => _reference;
            set { Set(()=>Reference, ref _reference, value); }
        }

        public double Amount
        {
            get => _amount;
            set { Set(()=>Amount, ref _amount, value); }
        }

        public void Initialise()
        {
            Date = DateTime.Now.Date;
            Payee = string.Empty;
            Reference=string.Empty;
            Amount = 0;
        }

    }
}