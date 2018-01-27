using System;
using SMFS.Model;
using SMFS.ViewModel;
using System.ComponentModel;
using System.Windows;
using Utils.Window;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow
    {
        private readonly TransactionViewModel _model;

        public TransactionWindow()
        {
            InitializeComponent();
            _model = (TransactionViewModel)DataContext;
        }

        public Transaction Transaction { get; private set; }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Transaction = null;
            Close();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            Transaction = new Transaction
            {
                TransactionDate = _model.Date,
                Amount = Convert.ToDecimal(_model.Amount),
                Payee = _model.Payee,
                Reference = _model.Reference
                
            };
            Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveSettings();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.LoadSettings();
            _model.Initialise();
        }
    }
}