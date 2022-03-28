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
    public class InvestmentResultDetailController : Controller
    {
        [HttpGet]
        public async Task<InvestmentResultDetailResponse> Get()
        {
            var skip = 0;
            var take = 10;
            int.TryParse(Request.Query?["skip"].FirstOrDefault(), out skip);
            int.TryParse(Request.Query?["take"].FirstOrDefault(), out take);
            if (InvestmentResultDetail.DetailList == null) return null;
            return new InvestmentResultDetailResponse { TotalCount = InvestmentResultDetail.DetailList.Count, InvestmentResultDetails = InvestmentResultDetail.DetailList.Skip(skip).Take(take) };
        }
    }
}
