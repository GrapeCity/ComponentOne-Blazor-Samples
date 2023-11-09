using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataFilterExplorer.Server
{
    public class Car
    {
        public Car()
        {
            Random gen = new Random();
            int range = 25 * 365;
            DateProductionLine = DateTime.Today.AddDays(-gen.Next(range));
            IsSportVersion = gen.NextDouble() > 0.5;
            IsLimitedSeries = gen.NextDouble() > 0.5 ? true : gen.NextDouble() > 0.5? false: null;
        }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string TransmissSpeedCount { get; set; }
        public string TransmissAutomatic { get; set; }
        public DateTime DateProductionLine { get; set; }
        public bool IsSportVersion{ get; set; }
        public bool? IsLimitedSeries{ get; set; }

        [Browsable(false)]
        public int ID { get; set; }
        [Browsable(false)]
        public Int16 HP { get; set; }
        [Browsable(false)]
        public double Liter { get; set; }
        [Browsable(false)]
        public Int16 Cyl { get; set; }
        [Browsable(false)]
        public Int16 MPG_City { get; set; }
        [Browsable(false)]
        public Int16 MPG_Highway { get; set; }
        [Browsable(false)]
        public string Description { get; set; }
        [Browsable(false)]
        public string Hyperlink { get; set; }
        [Browsable(false)]
#if BLAZOR
        public Lazy<string> Picture { get; set; }
#else
        public byte[] Picture { get; set; }
#endif
    }
}