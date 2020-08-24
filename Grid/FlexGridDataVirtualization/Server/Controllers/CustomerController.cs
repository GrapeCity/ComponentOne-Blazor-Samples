using FlexGridDataVirtualization.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public CustomerResponse Get()
        {
            var skip = 0;
            var take = 10;
            int.TryParse(Request.Query?["skip"].FirstOrDefault(), out skip);
            int.TryParse(Request.Query?["take"].FirstOrDefault(), out take);

            var customers = _customers;

            #region filter
            var filter = Request.Query?["filter"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var properties = typeof(Customer).GetProperties();
                customers = customers.Where(c => properties.Any(p => p.GetValue(c) is string str && (str?.Contains(filter, StringComparison.CurrentCultureIgnoreCase) ?? false))).ToList();
            }
            #endregion

            #region sorting
            var sort = Request.Query?["sort"].FirstOrDefault();
            var sortDirection = Request.Query?["sortDirection"].FirstOrDefault()?.ToLower() != "desc";

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var property = typeof(Customer).GetProperty(sort);
                if (sortDirection)
                    customers = customers.OrderBy(c => property.GetValue(c)).ToList();
                else
                    customers = customers.OrderByDescending(c => property.GetValue(c)).ToList();
            }
            #endregion

            return new CustomerResponse { TotalCount = customers.Count, Customers = customers.Skip(skip).Take(take) };
        }
    }
}
