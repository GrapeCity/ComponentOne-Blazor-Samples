using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentCalculator.Shared
{
    public class InvestmentResult
    {
        public int TotalPeriod { get; set; }
        public double TotalInterestIncome { get; set; }
        public double EndingBalance { get; set; }
        public bool Status { get; set; } = true;
    }
}
