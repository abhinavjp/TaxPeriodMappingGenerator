using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPeriodMappingGenerator
{
    public static class TaxPeriodMappingService
    {
        public static List<TaxPeriodMapping> GenerateMappings(TaxPeriodMappingConfiguration taxPeriodMappingConfiguration)
        {
            if (!taxPeriodMappingConfiguration.IsAllRequiredFieldsAdded)
            {
                var errorMessage = taxPeriodMappingConfiguration.GetErrorMessages();
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    throw new ArgumentException(errorMessage);
                }
            }
            var configuration = new DerivedTaxPeriodMappingConfiguration(taxPeriodMappingConfiguration);
            if (configuration.IsWeekEndingBasedOnYearStartDate)
            {
                configuration.WeekEndingDay = configuration.YearStartDate.AddDays(-1).DayOfWeek;
            }

            var yearEndDate = configuration.YearStartDate.AddYears(1).AddDays(-1);
            var mappings = new List<TaxPeriodMapping>();
            var taxWeek = 0;
            var taxMonth = 1;
            var startWeekOfFortnight = 1;
            var startWeekOfFourWeek = 1;
            var endWeekOfFortnight = 0;
            var endWeekOfFourWeek = 0;
            var startDateOfWeek = configuration.FirstPeriodStartDate;
            var startDateOfFortNight = configuration.FirstPeriodStartDate;
            var startDateOfFourWeek = configuration.FirstPeriodStartDate;
            var weekNumberForMonth = 1;

            var lastStartWeekOfMonth = 1;
            var lastWeekEndDate = configuration.FirstPeriodStartDate.AddDays(-1);
            for (var currentDate = configuration.FirstPeriodStartDate; currentDate <= yearEndDate; currentDate = currentDate.AddDays(1))
            {
                if (currentDate.DayOfWeek != configuration.WeekEndingDay) continue;
                taxWeek++;

                var taxFortnight = Math.DivRem(taxWeek + 1, 2, out var taxFortnightRemainder);
                if (taxFortnightRemainder == 0)
                {
                    startWeekOfFortnight = taxWeek;
                    endWeekOfFortnight = taxWeek + 1;
                    startDateOfFortNight = currentDate;
                }
                var taxFourWeek = Math.DivRem(taxWeek + 3, 4, out var taxFourWeekRemainder);
                if (taxFourWeekRemainder == 0)
                {
                    startWeekOfFourWeek = taxWeek;
                    endWeekOfFourWeek = taxWeek + 3;
                    startDateOfFourWeek = currentDate;
                }

                var taxMonthDetails = GetMonth(configuration.YearStartDate, currentDate, taxWeek, lastWeekEndDate, taxMonth, lastStartWeekOfMonth);
                lastStartWeekOfMonth = taxMonthDetails.StartTaxWeek;
                weekNumberForMonth++;
                if (taxMonth != taxMonthDetails.TaxMonth)
                {
                    weekNumberForMonth = 1;
                }

                taxMonth = taxMonthDetails.TaxMonth;
                var mapping = new TaxPeriodMapping
                {
                    TaxWeek = taxWeek,
                    TaxFortnight = taxFortnight,
                    TaxFourWeek = taxFourWeek,
                    TaxMonth = taxMonthDetails.TaxMonth,
                    WeekNumberForTaxFortnight = taxFortnightRemainder + 1,
                    WeekNumberForTaxFourWeek = taxFourWeekRemainder + 1,
                    WeekNumberForTaxMonth = weekNumberForMonth,
                    TaxYear = configuration.TaxYear,
                    StartWeekOfMonth = taxMonthDetails.StartTaxWeek,
                    LastWeekOfMonth = taxMonthDetails.LastTaxWeek,
                    EndDateOfTaxWeek = startDateOfWeek.AddDays(6),
                    StartDateOfTaxWeek = startDateOfWeek,
                    StartDateOfTaxMonth = taxMonthDetails.StartDate,
                    EndDateOfTaxMonth = taxMonthDetails.EndDate,
                    CalendarStartDateOfTaxMonth = taxMonthDetails.CalendarStartDate,
                    CalendarEndDateOfTaxMonth = taxMonthDetails.CalendarEndDate,
                    CalendarStartDateOfTaxWeek = startDateOfWeek,
                    CalendarEndDateOfTaxWeek = startDateOfWeek.AddDays(6),
                    StartWeekOfFortnight = startWeekOfFortnight,
                    LastWeekOfFortnight = endWeekOfFortnight,
                    StartWeekOfFourWeek = startWeekOfFourWeek,
                    LastWeekOfFourWeek = endWeekOfFourWeek,
                    StartDateOfFortnight = startDateOfFortNight,
                    EndDateOfFortnight = startDateOfFortNight.AddDays(13),
                    StartDateOfFourWeek = startDateOfFourWeek,
                    EndDateOfFourWeek = startDateOfFourWeek.AddDays(25)
                };
                mappings.Add(mapping);
            }

            return mappings;
        }

        private static TaxMonthDetails GetMonth(DateTime yearStartDate, DateTime currentDate, int taxWeek, DateTime lastWeekEndDate, int lastTaxMonth, int lastStartWeekOfMonth)
        {
            var taxMonthDetails = new TaxMonthDetails
            {
                TaxMonth = lastTaxMonth,
                StartTaxWeek = lastStartWeekOfMonth
            };
            var currentAndPeriodMonthDateDiff = currentDate.Day - yearStartDate.Day;
            var totalDaysInCurrentMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            if (currentAndPeriodMonthDateDiff < 0)
            {
                currentAndPeriodMonthDateDiff *= -1;
            }
            else if (currentAndPeriodMonthDateDiff > 0)
            {
                currentAndPeriodMonthDateDiff = totalDaysInCurrentMonth - currentAndPeriodMonthDateDiff;
            }

            var nextMonthStartDate = currentDate.AddDays(currentAndPeriodMonthDateDiff);
            if (nextMonthStartDate <= yearStartDate)
            {
                nextMonthStartDate = yearStartDate.AddMonths(1);
            }
            var previousMonthStartDate = nextMonthStartDate.AddMonths(-1);

            if (lastWeekEndDate < previousMonthStartDate && previousMonthStartDate <= currentDate && previousMonthStartDate > yearStartDate)
            {
                taxMonthDetails.TaxMonth++;
                taxMonthDetails.StartTaxWeek = taxWeek;
            }

            if (currentAndPeriodMonthDateDiff < 7)
            {
                taxMonthDetails.LastTaxWeek = taxWeek;
            }

            return taxMonthDetails;
        }
    }
}
