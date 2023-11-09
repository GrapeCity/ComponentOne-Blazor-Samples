using InvestmentCalculator.Client.Pages;
using InvestmentCalculator.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestmentResultController : Controller
    {
        [HttpGet]
        public async Task<InvestmentResult> Get()
        {
            double initialAmt = 0, rate = 0, depositAmt = 0;
            int investmentYears = 0, numDeposits = 0;
            double.TryParse(Request.Query?["initialAmt"].FirstOrDefault(), out initialAmt);
            double.TryParse(Request.Query?["rate"].FirstOrDefault(), out rate);
            double.TryParse(Request.Query?["depositAmt"].FirstOrDefault(), out depositAmt);
            int.TryParse(Request.Query?["investmentYears"].FirstOrDefault(), out investmentYears);
            int.TryParse(Request.Query?["numDeposits"].FirstOrDefault(), out numDeposits);
            return InvestmentResultDetail.CalculateReturn(initialAmt, rate, depositAmt, investmentYears, numDeposits);
        }
    }
}
