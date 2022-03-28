using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentCalculator.Shared
{
    public class InvestmentResultDetail
    {
        public int Period { get; set; }
        public double InitialBalance { get; set; }
        public double InterestEarned { get; set; }
        public double NewDeposit { get; set; }
        public double NewBalance { get; set; }

        public static List<InvestmentResultDetail> DetailList;

        //Method to calculate investment return
        public static InvestmentResult CalculateReturn(double initialAmt, double rate, double depositAmt, int investmentYears, int numDeposits)
        {
            InvestmentResult result = new InvestmentResult();
            //Calculate total number of periods and assign to respective grid cell
            int totalPeriods = investmentYears * numDeposits;

            //Make sure total number of periods is not more than 360
            if (totalPeriods <= 360)
            {
                result.TotalPeriod = totalPeriods;
            }
            else
            {
                result.Status = false;
            }
            DetailList = new List<InvestmentResultDetail>();
            //Calculate investment return for each period in investment duration
            for (int period = 1; period <= totalPeriods; period++)
            {
                var investmentResultDetail = new InvestmentResultDetail();
                investmentResultDetail.Period = period;
                if (period == 1)
                {
                    investmentResultDetail.InitialBalance = initialAmt;
                }
                else
                {
                    investmentResultDetail.InitialBalance = DetailList.Last().NewBalance;
                }
                investmentResultDetail.InterestEarned = (((rate / numDeposits) * investmentResultDetail.InitialBalance) / 100);
                investmentResultDetail.NewDeposit = depositAmt;
                investmentResultDetail.NewBalance = investmentResultDetail.InitialBalance 
                    + investmentResultDetail.InterestEarned + investmentResultDetail.NewDeposit;
                DetailList.Add(investmentResultDetail);
            }

            //Calculat Future Value of investment/Ending Balance
            double Rate = rate / (numDeposits * 100);
            double NPer = Convert.ToDouble(totalPeriods);
            double Pmt = Convert.ToInt32(depositAmt);
            double PV = Convert.ToInt32(initialAmt);
            double fv = -(Financial.FV(Rate, NPer, Pmt, PV));
            result.EndingBalance = fv;

            //Calculate total interest income
            double totalInterestIncome = fv - initialAmt - (depositAmt * totalPeriods);
            result.TotalInterestIncome = totalInterestIncome;

            return result;
        }
    }
    public class InvestmentResultDetailResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<InvestmentResultDetail> InvestmentResultDetails { get; set; }
    }
}
