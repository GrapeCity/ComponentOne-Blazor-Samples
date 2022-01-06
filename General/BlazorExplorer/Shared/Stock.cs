using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BlazorExplorer
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal LastSale { get; set; }
        public decimal Change
        {
            get
            {
                if (Ask == 0)
                {
                    return 0;
                }
                return 1 - (LastSale / Ask);
            }
        }
        public int BidSize { get; set; }
        public int AskSize { get; set; }
        public int LastSize { get; set; }
        public int Volume { get; set; }
        public DateTime QuoteTime { get; set; }
        public DateTime TradeTime { get; set; }
    }

    public class StockResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<Stock> Stocks { get; set; }
    }
}
