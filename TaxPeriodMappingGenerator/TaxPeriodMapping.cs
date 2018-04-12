using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPeriodMappingGenerator
{
    public class TaxPeriodMapping
    {
        public Dictionary<string, object> AdditionalData { get; set; }

        public int TaxWeek { get; set; }
        public int TaxFortnight { get; set; }
        public int TaxFourWeek { get; set; }
        public int TaxMonth { get; set; }
        public int WeekNumberForTaxFortnight { get; set; }
        public int WeekNumberForTaxFourWeek { get; set; }
        public int WeekNumberForTaxMonth { get; set; }
        public int TaxYear { get; set; }
        public int StartWeekOfMonth { get; set; }
        public int LastWeekOfMonth { get; set; }
        public DateTime EndDateOfTaxWeek { get; set; }
        public DateTime StartDateOfTaxWeek { get; set; }
        public DateTime StartDateOfTaxMonth { get; set; }
        public DateTime EndDateOfTaxMonth { get; set; }
        public DateTime CalendarEndDateOfTaxWeek { get; set; }
        public DateTime CalendarStartDateOfTaxWeek { get; set; }
        public DateTime CalendarStartDateOfTaxMonth { get; set; }
        public DateTime CalendarEndDateOfTaxMonth { get; set; }
        public int StartWeekOfFortnight { get; set; }
        public int LastWeekOfFortnight { get; set; }
        public int StartWeekOfFourWeek { get; set; }
        public int LastWeekOfFourWeek { get; set; }
        public DateTime StartDateOfFortnight { get; set; }
        public DateTime EndDateOfFortnight { get; set; }
        public DateTime StartDateOfFourWeek { get; set; }
        public DateTime EndDateOfFourWeek { get; set; }
    }

    internal class TaxMonthDetails
    {
        public int TaxMonth { get; set; }
        public int StartTaxWeek { get; set; }
        public int LastTaxWeek { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CalendarStartDate { get; set; }
        public DateTime CalendarEndDate { get; set; }
    }
}
