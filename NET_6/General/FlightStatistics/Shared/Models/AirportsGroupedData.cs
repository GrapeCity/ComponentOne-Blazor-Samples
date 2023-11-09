namespace FlightStatistics.Shared
{
    public class AirportsGroupedDataByRegion
    {
        public string Region { get; set; }
        public double AverageDelayAvg { get; set; }
        public double OnTimeAvg { get; set; }
        public int TotalFlights { get; set; }
    }

    public class AirportsGroupedDataByCity
    {
        public string Region { get; set; }
        public string AirportCity { get; set; }
        public double OnTimeAvg { get; set; }
        public int TotalFlights { get; set; }
    }
}
