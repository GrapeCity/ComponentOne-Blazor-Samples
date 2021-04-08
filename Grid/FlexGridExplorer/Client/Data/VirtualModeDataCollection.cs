using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using C1.DataCollection.Serialization;

namespace FlexGridExplorer.Pages
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
            string url = $"Customer?skip={startingIndex}&take={count}";
            if (sortDescriptions?.Count > 0)
            {
                url += $"&sort={Uri.EscapeUriString(JsonSerializer.Serialize<IReadOnlyList<SortDescription>>(sortDescriptions))}";
            }
            if (filterExpression != null)
            {
                var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter() } };
                url += $"&filter={Uri.EscapeUriString(JsonSerializer.Serialize(filterExpression, options))}";
            }
            var response = await Http.GetFromJsonAsync<CustomerResponse>(new Uri(url, UriKind.Relative), cancellationToken);
            return new Tuple<int, IReadOnlyList<Customer>>(response.TotalCount, response.Customers.ToList());
        }
    }
}
