using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPeriodMappingGenerator
{
    public class TaxPeriodMappingConfiguration
    {
        public int? TaxYear { get; set; }
        public DateTime? YearStartDate { get; set; }
        public DateTime? FirstPeriodStartDate { get; set; }
        public bool IsWeekEndingBasedOnYearStartDate { get; set; } = true;
        public DayOfWeek? WeekEndingDay { get; set; }
        public bool IsAllRequiredFieldsAdded => TaxYear.HasValue && YearStartDate.HasValue && FirstPeriodStartDate.HasValue && (IsWeekEndingBasedOnYearStartDate || WeekEndingDay.HasValue);

        public string GetErrorMessages()
        {
            if (IsAllRequiredFieldsAdded)
                return null;
            var sb = new StringBuilder();
            if (!TaxYear.HasValue)
                sb.Append("Tax year not configured.\n");
            if (!YearStartDate.HasValue)
                sb.Append("Year start date not configured.\n");
            if (!FirstPeriodStartDate.HasValue)
                sb.Append("First period start date not configured.\n");
            if (!IsWeekEndingBasedOnYearStartDate && !WeekEndingDay.HasValue)
                sb.Append("Week ending day not configured as week ending day is not based on year start date.\n");
            return sb.Length <= 0 ? null : sb.ToString();
        }
    }

    internal class DerivedTaxPeriodMappingConfiguration
    {
        public DerivedTaxPeriodMappingConfiguration(TaxPeriodMappingConfiguration configuration)
        {
            TaxYear = configuration.TaxYear.Value;
            YearStartDate = configuration.YearStartDate.Value;
            FirstPeriodStartDate = configuration.FirstPeriodStartDate.Value;
            IsWeekEndingBasedOnYearStartDate = configuration.IsWeekEndingBasedOnYearStartDate;
            WeekEndingDay = configuration.WeekEndingDay ?? YearStartDate.AddDays(-1).DayOfWeek;
        }
        public int TaxYear { get; set; }
        public DateTime YearStartDate { get; set; }
        public DateTime FirstPeriodStartDate { get; set; }
        public bool IsWeekEndingBasedOnYearStartDate { get; set; } = true;
        public DayOfWeek WeekEndingDay { get; set; }
        public DateTime YearEndDate => YearStartDate.AddYears(1).AddDays(-1);

    }
}
