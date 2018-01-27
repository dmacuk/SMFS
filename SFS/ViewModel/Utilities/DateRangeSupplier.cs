using System;
using System.Collections.Generic;

namespace SMFS.ViewModel.Utilities
{
    internal class DateRangeSupplier
    {
        public IEnumerable<IDateRange> getDateRanges()
        {
            return new List<IDateRange>
            {
                new AnyDateRange(),
                new TodayDateRange(),
                new YesterdayDateRange(),
                new LastThreeDaysDateRange(),
                new WeekToDateDateRange(),
                new MonthToDateDateReange(),
                new PreviousMonthDareRange(),
                new Last30DaysDateRange(),
                new Last90DaysDateRange(),
                new Last180DaysDateRange(),
                new Last365DaysDateRange(),
                new DateRange()
            };
        }
    }

    internal abstract class AbstractDateRange : IDateRange
    {
        public DateRangePeriod DateRangePeriod { get; set; }
        public string Display { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        public string ReportText
        {
            get
            {
                return DateRangePeriod == DateRangePeriod.Any ? "All Transactions" : $"Transactions between {StartDate:d} and {EndDate:d}";
            }
            set { }
        }
    }

    internal class AnyDateRange : AbstractDateRange
    {
        public AnyDateRange()
        {
            DateRangePeriod = DateRangePeriod.Any;
            Display = "Any";
            StartDate = new DateTime(2010, 1, 1).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class DateRange : AbstractDateRange
    {
        public DateRange()
        {
            DateRangePeriod = DateRangePeriod.DateRange;
            Display = "Date Range";
            StartDate = new DateTime(2010, 1, 1).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class Last180DaysDateRange : AbstractDateRange
    {
        public Last180DaysDateRange()
        {
            DateRangePeriod = DateRangePeriod.Last180Days;
            Display = "Last 180 Days";
            StartDate = DateTime.Now.AddDays(-180).Date;
            EndDate = DateTime.Now.Date;

        }
    }

    internal class Last30DaysDateRange : AbstractDateRange
    {
        public Last30DaysDateRange()
        {
            DateRangePeriod = DateRangePeriod.Last30Days;
            Display = "Last 30 Days";
            StartDate = DateTime.Now.AddDays(-30).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class Last365DaysDateRange : AbstractDateRange
    {
        public Last365DaysDateRange()
        {
            DateRangePeriod = DateRangePeriod.Last365Days;
            Display = "Last 365 Days";
            StartDate = DateTime.Now.AddDays(-30).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class Last90DaysDateRange : AbstractDateRange
    {
        public Last90DaysDateRange()
        {
            DateRangePeriod = DateRangePeriod.Last90Days;
            Display = "Last 90 Days";
            StartDate = DateTime.Now.AddDays(-90).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class LastThreeDaysDateRange : AbstractDateRange
    {
        public LastThreeDaysDateRange()
        {
            DateRangePeriod = DateRangePeriod.LastThreeDays;
            Display = "Last 3 Days";
            StartDate = DateTime.Now.AddDays(-3).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class MonthToDateDateReange : AbstractDateRange
    {
        public MonthToDateDateReange()
        {
            DateRangePeriod = DateRangePeriod.MonthToDate;
            Display = "Month To Date";
            StartDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class PreviousMonthDareRange : AbstractDateRange
    {
        public PreviousMonthDareRange()
        {
            DateRangePeriod = DateRangePeriod.PreviousMonth;
            Display = "Previous Month";
            StartDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(-1).Date;
            EndDate = DateTime.Now.AddDays(-DateTime.Now.Day);
        }
    }

    internal class TodayDateRange : AbstractDateRange
    {
        public TodayDateRange()
        {
            DateRangePeriod = DateRangePeriod.Today;
            Display = "Today";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class WeekToDateDateRange : AbstractDateRange
    {
        public WeekToDateDateRange()
        {
            DateRangePeriod = DateRangePeriod.WeekToDate;
            Display = "Week to date";

            var diff = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
                diff += 7;

            StartDate = DateTime.Now.AddDays(-1 * diff).Date;
            EndDate = DateTime.Now.Date;
        }
    }

    internal class YesterdayDateRange : AbstractDateRange
    {
        public YesterdayDateRange()
        {
            DateRangePeriod = DateRangePeriod.Yesterday;
            Display = "Yesterday";
            StartDate = DateTime.Now.AddDays(-1).Date;
            EndDate = DateTime.Now.Date;
        }
    }

}