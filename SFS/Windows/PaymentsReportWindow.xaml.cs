using JetBrains.Annotations;
using SMFS.ViewModel;
using Syncfusion.Windows.Reports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using Utils.Window;

namespace SMFS.Windows
{
    /// <summary>
    /// Interaction logic for PaymentsReportWindow.xaml
    /// </summary>
    public partial class PaymentsReportWindow
    {
        [NotNull]
        private readonly PaymentsReportViewModel _model;

        public PaymentsReportWindow()
        {
            InitializeComponent();
            _model = (PaymentsReportViewModel)DataContext;
            Generate(null, null);
        }

        private static Paragraph TotalLine(string description, decimal total)
        {
            var paragraph = new Paragraph() { TextAlignment = TextAlignment.Right };
            paragraph.Inlines.Add(new Run($"{description}: ") { FontWeight = FontWeights.Bold });
            paragraph.Inlines.Add(new Run($"{total}"));
            return paragraph;
        }

        private async void Generate(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "SMFS.Reports.PaymentsReport.rdlc";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Viewer.LoadReport(stream);
            }

            var parameters = new List<ReportParameter>();
            var parameter = new ReportParameter { Name = "ReportRange" };
            parameter.Values.Add(_model?.SelectedDateRange?.ReportText);
            parameters.Add(parameter);
            Viewer.SetParameters(parameters);

            Viewer.DataSources.Clear();
            var transactions = await _model.LoadTrasnactions();
            var dataSource = new ReportDataSource("Transactions", transactions);
            Viewer.DataSources.Add(dataSource);

            Viewer.RefreshReport();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveSettings();
            _model.SaveLastValues();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.LoadSettings();
            _model.LoadLastValues();
        }
    }
}