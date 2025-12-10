using System;

#nullable disable

namespace FlightStatistics.Shared
{
    public partial class AirportMonthlyData
    {
        public int Sno { get; set; }
        public string AirportCode { get; set; }
        public string AirportCity { get; set; }
        public string Region { get; set; }
        public DateTime RecordedDate { get; set; }
        public string RecordedMonth
        {
            get
            {
                return RecordedDate.ToString("MMM");
            }
        }
        public double Delay { get; set; }
    }
}
