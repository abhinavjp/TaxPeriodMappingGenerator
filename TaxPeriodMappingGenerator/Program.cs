using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPeriodMappingGenerator
{
    class Program
    {
        static void Main()
        {
            var configuration = new TaxPeriodMappingConfiguration
            {
                TaxYear = 2017,
                FirstPaymentDate = new DateTime(2017,4,7),
                YearStartDate = new DateTime(2017,4,6),
                IsWeekEndingBasedOnYearStartDate = true
            };
            var taxPeriodMappings = TaxPeriodMappingService.GenerateMappings(configuration);
        }
    }
}
