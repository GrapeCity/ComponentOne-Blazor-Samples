using BlazorExplorer.Pages;
using C1.DataCollection;
using C1.DataCollection.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorExplorer
{
    public class VirtualModeDataCollection : C1VirtualDataCollection<Customer>
    {
        public HttpClient Http { get; set; }

        public override bool CanSort(params SortDescription[] sortDescriptions)
        {
            return true;
        }

        public override bool CanFilter(FilterExpression filterExpression)
        {
            return !(filterExpression is FilterPredicateExpression);
        }

        protected override async Task<Tuple<int, IReadOnlyList<Customer>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new CustomerRequest()
            {
                Skip = startingIndex,
                Take = count,
                FilterExpression = filterExpression,
                SortDescriptions = sortDescriptions,
            };
            var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter() } };
            var content = JsonContent.Create(request, options: options);
            var response = await Http.PostAsync(new Uri("Customer", UriKind.Relative), content, cancellationToken);
            var response2 = await response.Content.ReadFromJsonAsync<CustomerResponse>(cancellationToken: cancellationToken);
            return new Tuple<int, IReadOnlyList<Customer>>(response2.TotalCount, response2.Customers.ToList());
        }
    }
}
