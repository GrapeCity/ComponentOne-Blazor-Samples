using C1.DataCollection.Serialization;
using C1.DataCollection;
using InvestmentCalculator.Client.Pages;
using InvestmentCalculator.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

            var result = new C1DataCollection<InvestmentResultDetail>(InvestmentResultDetail.DetailList);

            #region filter
            var filter = Request.Query?["filter"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter() } };
                var filterExpression = JsonSerializer.Deserialize<FilterExpression>(filter, options);
                await result.FilterAsync(filterExpression);
            }
            #endregion

            #region sort
            var sort = Request.Query?["sort"].FirstOrDefault();
            if(!string.IsNullOrEmpty(sort))
            {
                var sortDescriptions = JsonSerializer.Deserialize<IReadOnlyList<SortDescription>>(sort);
                await result.SortAsync(sortDescriptions.ToArray());
            }
            #endregion

            return new InvestmentResultDetailResponse { TotalCount = result.Count, InvestmentResultDetails = result.Skip(skip).Take(take).Select(o => o as InvestmentResultDetail) };
        }
    }
}
