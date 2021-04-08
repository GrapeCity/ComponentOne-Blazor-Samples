using FlexGridDataVirtualization.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using C1.DataCollection.Serialization;
using C1.DataCollection;

namespace FlexGridDataVirtualization.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private static List<Customer> _customers = Enumerable.Range(0, 1000).Select(i => new Customer(i)).ToList();
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<CustomerResponse> Get()
        {
            int skip;
            int take;
            if (!int.TryParse(Request.Query?["skip"].FirstOrDefault(), out skip)) skip = 0;
            if (!int.TryParse(Request.Query?["take"].FirstOrDefault(), out take)) take = 10;

            var customers = _customers;

            #region filter
            var filter = Request.Query?["filter"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(filter))

            {

                var options = new JsonSerializerOptions
                {
                    Converters = { new FilterExpressionJsonConverter() }
                };

                var filterExpression = JsonSerializer.Deserialize<FilterExpression>(filter, options);
                var filterCollection = new C1FilterDataCollection<Customer>(customers);
                await filterCollection.FilterAsync(filterExpression);
                customers = filterCollection.ToList();

            }
            #endregion

            #region sorting
            var sort = Request.Query?["sort"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(sort))
            {

                var options = new JsonSerializerOptions
                {
                    Converters = { new SortDescriptionJsonConverter() }
                };
                var sortDescriptions = JsonSerializer.Deserialize<SortDescription[]>(sort, options);
                var sortCollection = new C1SortDataCollection<Customer>(customers);
                await sortCollection.SortAsync(sortDescriptions);
                customers = sortCollection.ToList();

            }
            #endregion
            return new CustomerResponse { TotalCount = customers.Count, Customers = customers.Skip(skip).Take(take) };
        }
    }
}
