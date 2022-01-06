using BlazorExplorer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using C1.DataCollection;
using System.Reflection;
using C1.DataCollection.Serialization;

namespace BlazorExplorer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private static List<Stock> _stocks = GetFinancialData();
        private readonly ILogger<CustomerController> logger;

        public StockController(ILogger<CustomerController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<StockResponse> Get()
        {
            var skip = 0;
            var take = 10;
            int.TryParse(Request.Query?["skip"].FirstOrDefault(), out skip);
            int.TryParse(Request.Query?["take"].FirstOrDefault(), out take);

            var stocks = _stocks;

            #region filter
            var filter = Request.Query?["filter"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter() } };
                var filterExpression = JsonSerializer.Deserialize<FilterExpression>(filter, options);
                var filterCollection = new C1FilterDataCollection<Stock>(stocks);
                await filterCollection.FilterAsync(filterExpression);
                stocks = filterCollection.ToList();
            }
            #endregion

            #region sorting
            var sort = Request.Query?["sort"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var options = new JsonSerializerOptions { Converters = { new SortDescriptionJsonConverter() } };
                var sortDescriptions = JsonSerializer.Deserialize<SortDescription[]>(sort, options);
                var sortCollection = new C1SortDataCollection<Stock>(stocks);
                await sortCollection.SortAsync(sortDescriptions);
                stocks = sortCollection.ToList();
            }
            #endregion

            return new StockResponse { TotalCount = stocks.Count, Stocks = stocks.Skip(skip).Take(take) };
        }

        public static List<Stock> GetFinancialData()
        {
            var list = new List<Stock>();
            var rnd = new Random(0);

            var assembly = typeof(StockController).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("BlazorExplorer.Server.Data.StockSymbols.txt");
            using (var sr = new System.IO.StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    var sn = sr.ReadLine().Split('\t');
                    if (sn.Length > 1 && sn[0].Trim().Length > 0)
                    {
                        var data = new Stock();
                        data.Symbol = sn[0];
                        data.Name = sn[1];
                        data.Bid = rnd.Next(1, 1000);
                        data.Ask = data.Bid + (decimal)rnd.NextDouble();
                        //data.LastSale = data.Bid;
                        data.LastSale = rnd.Next(1, 1000);
                        data.BidSize = rnd.Next(10, 500);
                        data.AskSize = rnd.Next(10, 500);
                        data.LastSize = rnd.Next(10, 500);
                        data.Volume = rnd.Next(10000, 20000);
                        data.QuoteTime = DateTime.Now;
                        data.TradeTime = DateTime.Now;
                        list.Add(data);
                    }
                }
            }
            return list;
        }


    }
}
