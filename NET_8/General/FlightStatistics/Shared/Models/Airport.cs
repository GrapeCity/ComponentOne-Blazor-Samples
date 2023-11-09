using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace FlightStatistics.Shared
{
    public partial class Airport
    {
        public string AirportCode { get; set; }
        public string AirportCity { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Region { get; set; }
        public int OnTimeRanking { get; set; }
        public int Flights { get; set; }
        public double CompletionFactor { get; set; }
        public double OnTime
        {
            get
            {
                return (1 - Delayed);
            }
        }
        public double Delayed { get; set; }
        public double AverageDelay { get; set; }
        public int UserRating { get; set; }

        [NotMapped]
        public IEnumerable<AirportMonthlyData> AirportMonthlyData { get; set; }
    }
}
