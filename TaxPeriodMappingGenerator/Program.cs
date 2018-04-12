using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPeriodMappingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new TaxPeriodMappingConfiguration
            {
                TaxYear = 2017,
                FirstPeriodStartDate = new DateTime(2017,3,27),
                YearStartDate = new DateTime(2017,4,6),
                IsWeekEndingBasedOnYearStartDate = true
            };
            var taxPeriodMappings = TaxPeriodMappingService.GenerateMappings(configuration);
        }
    }
}
