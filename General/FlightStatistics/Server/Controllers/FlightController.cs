using FlightStatistics.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FlightStatistics.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly AirportContext _db;

        private readonly ILogger<FlightController> _logger;

        public FlightController(ILogger<FlightController> logger, AirportContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Airport> Get()
        {
            var airportsData = _db.Airports.ToList();
            var airportsMonthlyData = _db.AirportMonthlyData.ToList();

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            List<Airport> List = new List<Airport>();

            foreach (Airport airport in airportsData)
            {
                var ap = airportsMonthlyData.Where(x => x.AirportCode.Trim() == airport.AirportCode.Trim());
                List.Add(new Airport
                {
                    AirportCode = airport.AirportCode.Trim(),
                    AirportCity = myTI.ToTitleCase(airport.AirportCity.Trim().ToLower()),
                    CountryCode = airport.CountryCode.Trim(),
                    CountryName = airport.CountryName.Trim(),
                    Delayed = airport.Delayed,
                    AverageDelay = airport.AverageDelay,
                    Region = airport.Region,
                    Flights = airport.Flights,
                    UserRating = airport.UserRating,
                    CompletionFactor = airport.CompletionFactor,
                    AirportMonthlyData = ap,
                    OnTimeRanking = airport.OnTimeRanking
                });
            }

            return List;
        }
    }

}
