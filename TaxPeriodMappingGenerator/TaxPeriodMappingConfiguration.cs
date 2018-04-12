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
        public bool IsWeekEndingBasedOnYearStartDate { get; set; } = true;
        public DayOfWeek? WeekEndingDay { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public int PaymentDateToPeriodEndDayDifference { get; set; }
        /************************Fortnightly and Fourweekly Configuration******************/
        public int WeekNumberOfFortnight { get; set; } = 2;
        /*******************************************************************/
        /************************Fourweekly Configuration******************/
        public int WeekNumberOfFourWeek { get; set; } = 4;

        /*******************************************************************/
        /************************Monthly Configuration******************/

        /*******************************************************************/

        public bool IsAllRequiredFieldsAdded => TaxYear.HasValue && YearStartDate.HasValue && FirstPaymentDate.HasValue && (IsWeekEndingBasedOnYearStartDate || WeekEndingDay.HasValue);

        public string GetErrorMessages()
        {
            if (IsAllRequiredFieldsAdded)
                return null;
            var sb = new StringBuilder();
            if (!TaxYear.HasValue)
                sb.Append("Tax year was not configured.\n");
            if (!YearStartDate.HasValue)
                sb.Append("Year start date was not configured.\n");
            if (!FirstPaymentDate.HasValue)
                sb.Append("First payment date was not configured.\n");
            if (!IsWeekEndingBasedOnYearStartDate && !WeekEndingDay.HasValue)
                sb.Append("Week ending day was not configured as week ending day is not based on year start date.\n");
            return sb.Length <= 0 ? null : sb.ToString();
        }
    }

    internal class DerivedTaxPeriodMappingConfiguration
    {
        public DerivedTaxPeriodMappingConfiguration(TaxPeriodMappingConfiguration configuration)
        {
            TaxYear = configuration.TaxYear.Value;
            YearStartDate = configuration.YearStartDate.Value;
            FirstPaymentDate = configuration.FirstPaymentDate.Value;
            IsWeekEndingBasedOnYearStartDate = configuration.IsWeekEndingBasedOnYearStartDate;
            WeekEndingDay = configuration.WeekEndingDay ?? YearStartDate.AddDays(-1).DayOfWeek;
            FirstPaymentDate = configuration.FirstPaymentDate.Value;
        }
        public int TaxYear { get; set; }
        public DateTime YearStartDate { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public bool IsWeekEndingBasedOnYearStartDate { get; set; } = true;
        public DayOfWeek WeekEndingDay { get; set; }
        public DateTime YearEndDate => YearStartDate.AddYears(1).AddDays(-1);
        public int PaymentDateToPeriodEndDayDifference { get; set; }
        /************************Fortnightly and Fourweekly Configuration******************/
        public int WeekNumberOfFortnight { get; set; }
        /*******************************************************************/
        /************************Fourweekly Configuration******************/
        public int WeekNumberOfFourWeek { get; set; }

    }

    public enum PayrollFrequency
    {
        Weekly,
        Fortnightly,
        Fourweekly,
        Monthly
    }
}
