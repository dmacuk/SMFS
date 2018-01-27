using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomDataSource.Model;
using SMFS.Services;
using SMFS.ViewModel.Utilities;
using Utils.Preference;

namespace SMFS.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PaymentsReportViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private const string PaymentReportDateRange = "PaymentReport.DateRange";
        private const string PaymentReportEndDate = "PaymentReport.EndDate";
        private const string PaymentReportStartDate = "PaymentReport.StartDate";
        private bool _datePickerEnabled;
        private DateTime? _endDate;
        private readonly IDateRange _lastDateRange;
        private IDateRange _selectedDateRange;
        private DateTime? _startDate;
        private string _dateRangeText;

        public PaymentsReportViewModel(IDataService dataService)
        {
            _dataService = dataService;
            var dateRangeSupplier = new DateRangeSupplier();
            DateRanges = new List<IDateRange>(dateRangeSupplier.getDateRanges());
            _lastDateRange = DateRanges[DateRanges.Count - 1];
        }

        public bool DatePickerEnabled
        {
            get => _datePickerEnabled;
            set { Set(() => DatePickerEnabled, ref _datePickerEnabled, value); }
        }

        public List<IDateRange> DateRanges { get; set; }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (!Set(() => EndDate, ref _endDate, value)) return;
                if (SelectedDateRange.DateRangePeriod == _lastDateRange.DateRangePeriod)
                    _lastDateRange.EndDate = value;
            }
        }

        public IDateRange SelectedDateRange
        {
            get => _selectedDateRange;
            set
            {
                if (!Set(() => SelectedDateRange, ref _selectedDateRange, value)) return;
                StartDate = value.StartDate;
                EndDate = value.EndDate;
                DatePickerEnabled = value.DateRangePeriod == DateRangePeriod.DateRange;
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (!Set(() => StartDate, ref _startDate, value)) return;
                if (SelectedDateRange.DateRangePeriod == _lastDateRange.DateRangePeriod)
                    _lastDateRange.StartDate = value;
            }
        }

        public string DateRangeText
        {
            get => SelectedDateRange?.ReportText;
            set { Set(()=>DateRangeText, ref _dateRangeText, value); }
        }

        public void LoadLastValues()
        {
            var dateRange = PreferenceManager.GetPreference(PaymentReportDateRange, 0);
            SelectedDateRange = DateRanges[dateRange];
            if (SelectedDateRange.DateRangePeriod != DateRangePeriod.DateRange)
            {
                DatePickerEnabled = false;
                return;
            }
            StartDate = PreferenceManager.GetPreference(PaymentReportStartDate,
                _lastDateRange.StartDate);
            EndDate = PreferenceManager.GetPreference(PaymentReportEndDate,
                _lastDateRange.EndDate);
            _lastDateRange.StartDate = StartDate;
            _lastDateRange.EndDate = EndDate;
        }

        public void SaveLastValues()
        {
            PreferenceManager.SetPreference(PaymentReportDateRange, DateRanges.IndexOf(SelectedDateRange));
            PreferenceManager.SetPreference(PaymentReportStartDate, StartDate);
            PreferenceManager.SetPreference(PaymentReportEndDate, EndDate);
        }

        public async Task<List<PaymentReportEntry>> LoadTrasnactions()
        {
            return await _dataService.SampleReportData();
        }
    }

}