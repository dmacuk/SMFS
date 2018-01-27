using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using SMFS.Model;
using Syncfusion.Windows.Reports;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for PaymentReceiptWindow.xaml
    /// </summary>
    public partial class PaymentReceiptWindow : Window
    {
        private readonly Receipt _receipt;

        public PaymentReceiptWindow(Receipt receipt)
        {
            _receipt = receipt;
            InitializeComponent();
            Generate();
        }
        private void Generate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "SMFS.Reports.Report1.rdlc";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Viewer.LoadReport(stream);
            }

            var parameters = new List<ReportParameter>
            {
                BuildParameter("ReceiptType", _receipt.Amount<0?"Refund":"Receipt"),
                BuildParameter("Name", _receipt.Name),
                BuildParameter("Address", _receipt.Address),
                BuildParameter("Amount", Math.Abs(_receipt.Amount).ToString("N2", CultureInfo.InvariantCulture)),
                BuildParameter("Payee", _receipt.Payee),
                BuildParameter("Reference", _receipt.Reference),
                BuildParameter("Date", _receipt.TransactionDate.ToShortDateString())
            };
            Viewer.SetParameters(parameters);

//            Viewer.DataSources.Clear();
//            var transactions = await _model.LoadTrasnactions();
//            var dataSource = new ReportDataSource("Transactions", transactions);
//            Viewer.DataSources.Add(dataSource);

            Viewer.RefreshReport();
        }

        private ReportParameter BuildParameter(string paramName, string paramValue)
        {
            var parameter = new ReportParameter {Name = paramName};
            parameter.Values.Add(paramValue);
            return parameter;
        }
    }
}
