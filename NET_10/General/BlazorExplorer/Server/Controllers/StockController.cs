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

        [HttpPost]
        public async Task<StockResponse> Get()
        {
            var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter(), new SortDescriptionJsonConverter() } };
            var stockRequest = await JsonSerializer.DeserializeAsync<StockRequest>(Request.Body, options);

            var stocks = _stocks;

            #region filter

            if (stockRequest.FilterExpression != null)
            {
                var filterCollection = new C1FilterDataCollection<Stock>(stocks);
                await filterCollection.FilterAsync(stockRequest.FilterExpression);
                stocks = filterCollection.ToList();
            }
            #endregion

            #region sorting

            if (stockRequest.SortDescriptions?.Count > 0)
            {
                var sortCollection = new C1SortDataCollection<Stock>(stocks);
                await sortCollection.SortAsync(stockRequest.SortDescriptions.ToArray());
                stocks = sortCollection.ToList();
            }
            #endregion

            return new StockResponse { TotalCount = stocks.Count, Stocks = stocks.Skip(stockRequest.Skip).Take(stockRequest.Take) };
        }

        private static List<Stock> _financialData;
        public static List<Stock> GetFinancialData()
        {
            if (_financialData == null)
            {
                _financialData = new List<Stock>();              
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
                            _financialData.Add(data);
                        }
                    }
                }
            }
            return _financialData.ToList();
        }


    }
}
