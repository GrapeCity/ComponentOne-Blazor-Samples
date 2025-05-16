using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BlazorExplorer
{
    public class FinancialData : INotifyPropertyChanged
    {
        const int HISTORY_SIZE = 5;
        List<decimal> _askHistory = new List<decimal>();
        List<decimal> _bidHistory = new List<decimal>();
        List<decimal> _saleHistory = new List<decimal>();

        public string Symbol
        {
            get { return (string)GetProp("Symbol"); }
            set { SetProp("Symbol", value); }
        }
        public string Name
        {
            get { return (string)GetProp("Name"); }
            set { SetProp("Name", value); }
        }
        public decimal Bid
        {
            get { return (decimal)GetProp("Bid"); }
            set
            {
                AddHistory(_bidHistory, value);
                SetProp("Bid", value);
            }
        }
        public decimal Ask
        {
            get { return (decimal)GetProp("Ask"); }
            set
            {
                AddHistory(_askHistory, value);
                SetProp("Ask", value);
            }
        }
        public decimal LastSale
        {
            get { return (decimal)GetProp("LastSale"); }
            set
            {
                AddHistory(_saleHistory, value);
                SetProp("LastSale", value);
            }
        }

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
        public int BidSize
        {
            get { return (int)GetProp("BidSize"); }
            set { SetProp("BidSize", value); }
        }
        public int AskSize
        {
            get { return (int)GetProp("AskSize"); }
            set { SetProp("AskSize", value); }
        }
        public int LastSize
        {
            get { return (int)GetProp("LastSize"); }
            set { SetProp("LastSize", value); }
        }
        public int Volume
        {
            get { return (int)GetProp("Volume"); }
            set { SetProp("Volume", value); }
        }
        public DateTime QuoteTime
        {
            get { return (DateTime)GetProp("QuoteTime"); }
            set { SetProp("QuoteTime", value); }
        }
        public DateTime TradeTime
        {
            get { return (DateTime)GetProp("TradeTime"); }
            set { SetProp("TradeTime", value); }
        }
        public List<decimal> GetHistory(string propName)
        {
            switch (propName)
            {
                case "Ask":
                    return _askHistory;
                case "Bid":
                    return _bidHistory;
                case "LastSale":
                    return _saleHistory;
            }
            return null;
        }
        void AddHistory(List<decimal> list, decimal value)
        {
            while (list.Count >= HISTORY_SIZE)
            {
                list.RemoveAt(0);
            }
            while (list.Count < HISTORY_SIZE)
            {
                list.Add(value);
            }
        }

        Dictionary<string, object> _propBag = new Dictionary<string, object>();
        object GetProp(string propName)
        {
            object value = null;
            _propBag.TryGetValue(propName, out value);
            return value;
        }
        void SetProp(string propName, object value)
        {
            if (_propBag.ContainsKey(propName))
            {
                var savedValue = GetProp(propName);
                if (savedValue == null && value == null || savedValue.Equals(value))
                    return;
            }
            _propBag[propName] = value;
            OnPropertyChanged(new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private static List<FinancialData> _financialData;
        // get a default list of financial items
        public static List<FinancialData> GetFinancialData()
        {
            if (_financialData == null)
            {
                _financialData = new List<FinancialData>();
                var rnd = new Random(0);
                var assembly = typeof(FinancialData).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream("BlazorExplorer.Data.StockSymbols.txt"))
                {
                    using (var sr = new System.IO.StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            var sn = sr.ReadLine().Split('\t');
                            if (sn.Length > 1 && sn[0].Trim().Length > 0)
                            {
                                var data = new FinancialData();
                                data.Symbol = sn[0];
                                data.Name = sn[1];
                                data.Bid = rnd.Next(1, 1000);
                                data.Ask = data.Bid + (decimal)rnd.NextDouble();
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
            }

            return _financialData.ToList();
        }

        public static List<StockSymbol> GetStockSymbolData()
        {
            var list = new List<StockSymbol>();
            list.Add(new StockSymbol { Low = 61.79, High = 63.34, Open = 62.4, Close = 62.88, Volume = 37617413, Date = new DateTime(2014, 6, 9) });
            list.Add(new StockSymbol { Low = 63.5, High = 65.82, Open = 63.53, Close = 65.77, Volume = 69338140, Date = new DateTime(2014, 6, 10) });
            list.Add(new StockSymbol { Low = 64.9, High = 65.8, Open = 65.32, Close = 65.78, Volume = 44241926, Date = new DateTime(2014, 6, 11) });
            list.Add(new StockSymbol { Low = 64.06, High = 66.47, Open = 65.85, Close = 64.29, Volume = 55729828, Date = new DateTime(2014, 6, 12) });
            list.Add(new StockSymbol { Low = 63.83, High = 64.97, Open = 64.7, Close = 64.5, Volume = 29418910, Date = new DateTime(2014, 6, 13) });
            list.Add(new StockSymbol { Low = 63.75, High = 64.88, Open = 64.16, Close = 64.19, Volume = 31045811, Date = new DateTime(2014, 6, 16) });
            list.Add(new StockSymbol { Low = 63.93, High = 64.88, Open = 64.1, Close = 64.4, Volume = 27714816, Date = new DateTime(2014, 6, 17) });
            list.Add(new StockSymbol { Low = 64.05, High = 65.75, Open = 64.49, Close = 65.6, Volume = 35570154, Date = new DateTime(2014, 6, 18) });
            list.Add(new StockSymbol { Low = 64.21, High = 65.58, Open = 65.46, Close = 64.34, Volume = 34245182, Date = new DateTime(2014, 6, 19) });
            list.Add(new StockSymbol { Low = 63.35, High = 64.81, Open = 64.46, Close = 64.5, Volume = 46466073, Date = new DateTime(2014, 6, 20) });
            list.Add(new StockSymbol { Low = 64.22, High = 65.66, Open = 64.32, Close = 65.37, Volume = 34560121, Date = new DateTime(2014, 6, 23) });
            list.Add(new StockSymbol { Low = 65.27, High = 67.17, Open = 65.36, Close = 65.72, Volume = 57334867, Date = new DateTime(2014, 6, 24) });
            list.Add(new StockSymbol { Low = 65.57, High = 67.48, Open = 65.58, Close = 67.44, Volume = 44308249, Date = new DateTime(2014, 6, 25) });
            list.Add(new StockSymbol { Low = 66.9, High = 68, Open = 68, Close = 67.13, Volume = 47713944, Date = new DateTime(2014, 6, 26) });
            list.Add(new StockSymbol { Low = 66.84, High = 67.7, Open = 67.31, Close = 67.6, Volume = 46460627, Date = new DateTime(2014, 6, 27) });
            list.Add(new StockSymbol { Low = 67.13, High = 67.92, Open = 67.46, Close = 67.29, Volume = 27201749, Date = new DateTime(2014, 6, 30) });
            list.Add(new StockSymbol { Low = 67.39, High = 68.44, Open = 67.58, Close = 68.06, Volume = 33243166, Date = new DateTime(2014, 7, 1) });
            list.Add(new StockSymbol { Low = 65.79, High = 68.3, Open = 68.04, Close = 66.45, Volume = 41895220, Date = new DateTime(2014, 7, 2) });
            list.Add(new StockSymbol { Low = 65.76, High = 67, Open = 66.86, Close = 66.29, Volume = 25203215, Date = new DateTime(2014, 7, 3) });
            list.Add(new StockSymbol { Low = 65.12, High = 66.57, Open = 66.3, Close = 65.29, Volume = 28745099, Date = new DateTime(2014, 7, 7) });
            list.Add(new StockSymbol { Low = 63.15, High = 65.12, Open = 63.41, Close = 62.76, Volume = 68926036, Date = new DateTime(2014, 7, 8) });
            list.Add(new StockSymbol { Low = 63.05, High = 65.34, Open = 63.31, Close = 64.87, Volume = 51431582, Date = new DateTime(2014, 7, 9) });
            list.Add(new StockSymbol { Low = 64.79, High = 66.59, Open = 65.28, Close = 66.34, Volume = 44421915, Date = new DateTime(2014, 7, 10) });
            list.Add(new StockSymbol { Low = 64.79, High = 66.59, Open = 65.28, Close = 66.34, Volume = 39212022, Date = new DateTime(2014, 7, 11) });
            list.Add(new StockSymbol { Low = 66.9, High = 68.17, Open = 67.13, Close = 67.9, Volume = 38536924, Date = new DateTime(2014, 7, 14) });
            list.Add(new StockSymbol { Low = 66.26, High = 68.09, Open = 67.96, Close = 67.16, Volume = 44292944, Date = new DateTime(2014, 7, 15) });
            list.Add(new StockSymbol { Low = 67.07, High = 67.94, Open = 67.54, Close = 67.66, Volume = 29593589, Date = new DateTime(2014, 7, 16) });
            list.Add(new StockSymbol { Low = 66.04, High = 67.85, Open = 67.03, Close = 66.41, Volume = 38188432, Date = new DateTime(2014, 7, 17) });
            list.Add(new StockSymbol { Low = 66.16, High = 68.46, Open = 66.8, Close = 68.42, Volume = 42455649, Date = new DateTime(2014, 7, 18) });
            list.Add(new StockSymbol { Low = 68.5, High = 69.96, Open = 68.81, Close = 69.4, Volume = 49539121, Date = new DateTime(2014, 7, 21) });
            list.Add(new StockSymbol { Low = 68.61, High = 69.77, Open = 69.76, Close = 69.27, Volume = 40397693, Date = new DateTime(2014, 7, 22) });
            list.Add(new StockSymbol { Low = 69.61, High = 71.33, Open = 69.74, Close = 71.29, Volume = 78434716, Date = new DateTime(2014, 7, 23) });
            list.Add(new StockSymbol { Low = 74.51, High = 76.74, Open = 75.96, Close = 74.98, Volume = 124167936, Date = new DateTime(2014, 7, 24) });
            list.Add(new StockSymbol { Low = 74.66, High = 75.67, Open = 74.99, Close = 75.19, Volume = 45917435, Date = new DateTime(2014, 7, 25) });
            list.Add(new StockSymbol { Low = 73.85, High = 75.5, Open = 75.17, Close = 74.92, Volume = 41725249, Date = new DateTime(2014, 7, 28) });
            list.Add(new StockSymbol { Low = 73.42, High = 74.92, Open = 74.72, Close = 73.71, Volume = 41324470, Date = new DateTime(2014, 7, 29) });
            list.Add(new StockSymbol { Low = 74.13, High = 75.19, Open = 74.21, Close = 74.68, Volume = 36853018, Date = new DateTime(2014, 7, 30) });
            list.Add(new StockSymbol { Low = 72.44, High = 74.16, Open = 74, Close = 72.65, Volume = 43991772, Date = new DateTime(2014, 7, 31) });
            list.Add(new StockSymbol { Low = 71.55, High = 73.22, Open = 72.22, Close = 72.36, Volume = 43535314, Date = new DateTime(2014, 8, 1) });
            list.Add(new StockSymbol { Low = 72.36, High = 73.88, Open = 72.36, Close = 73.51, Volume = 30776819, Date = new DateTime(2014, 8, 4) });
            list.Add(new StockSymbol { Low = 72.18, High = 73.59, Open = 73.51, Close = 72.69, Volume = 34986147, Date = new DateTime(2014, 8, 5) });
            list.Add(new StockSymbol { Low = 71.79, High = 73.72, Open = 72.02, Close = 72.47, Volume = 30985533, Date = new DateTime(2014, 8, 6) });
            list.Add(new StockSymbol { Low = 72.7, High = 74, Open = 73, Close = 73.17, Volume = 38140550, Date = new DateTime(2014, 8, 7) });
            list.Add(new StockSymbol { Low = 72.56, High = 73.43, Open = 73.4, Close = 73.06, Volume = 27202325, Date = new DateTime(2014, 8, 8) });
            list.Add(new StockSymbol { Low = 73.06, High = 73.91, Open = 73.46, Close = 73.44, Volume = 24591177, Date = new DateTime(2014, 8, 11) });
            list.Add(new StockSymbol { Low = 72.22, High = 73.33, Open = 73.09, Close = 72.83, Volume = 27418983, Date = new DateTime(2014, 8, 12) });
            list.Add(new StockSymbol { Low = 73.05, High = 74.25, Open = 73.12, Close = 73.77, Volume = 29265662, Date = new DateTime(2014, 8, 13) });
            list.Add(new StockSymbol { Low = 73.69, High = 74.38, Open = 73.97, Close = 74.3, Volume = 22207019, Date = new DateTime(2014, 8, 14) });
            list.Add(new StockSymbol { Low = 73, High = 74.65, Open = 74.32, Close = 73.63, Volume = 38909161, Date = new DateTime(2014, 8, 15) });
            return list;

        }
    }



    public class StockSymbol
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Volume { get; set; }
    }

}
