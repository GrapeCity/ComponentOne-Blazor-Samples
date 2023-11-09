using FlexGridExplorer.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using C1.DataCollection;
using C1.DataCollection.Serialization;

namespace FlexGridExplorer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private static List<Customer> _customers = Enumerable.Range(0, 100_000).Select(i => new Customer(i)).ToList();
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public async Task<CustomerResponse> Get()
        {
            var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter(), new SortDescriptionJsonConverter() } };
            var customerRequest = await JsonSerializer.DeserializeAsync<CustomerRequest>(Request.Body, options);

            var customers = _customers;

            #region filter

            if (customerRequest.FilterExpression != null)
            {
                var filterCollection = new C1FilterDataCollection<Customer>(customers);
                await filterCollection.FilterAsync(customerRequest.FilterExpression);
                customers = filterCollection.ToList();
            }
            #endregion

            #region sorting

            if (customerRequest.SortDescriptions?.Count > 0)
            {
                var sortCollection = new C1SortDataCollection<Customer>(customers);
                await sortCollection.SortAsync(customerRequest.SortDescriptions.ToArray());
                customers = sortCollection.ToList();
            }
            #endregion

            return new CustomerResponse { TotalCount = customers.Count, Customers = customers.Skip(customerRequest.Skip).Take(customerRequest.Take) };
        }
    }
}
