using System;

namespace SMFS.ViewModel.Utilities
{
    public interface IDateRange
    {
        DateRangePeriod DateRangePeriod { get; set; }
        string Display { get; set; }
        DateTime? EndDate { get; set; }
        DateTime? StartDate { get; set; }
        string ReportText { get; set; }
    }
}