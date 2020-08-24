using C1.DataCollection;
using FlexGridDataVirtualization.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FlexGridDataVirtualization.Client.Pages
{
    public class VirtualModeCollectionView : C1VirtualDataCollection<Customer>
    {
        public HttpClient Http { get; set; }

        public override bool CanSort(params SortDescription[] sortDescriptions)
        {
            return sortDescriptions?.Length <= 1;
        }

        public override bool CanFilter(FilterExpression filterExpression)
        {
            return filterExpression is FilterOperationExpression operation && operation.FilterOperation == FilterOperation.Contains;
        }

        protected override async Task<Tuple<int, IReadOnlyList<Customer>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            string url = $"Customer?skip={startingIndex}&take={count}";
            if (sortDescriptions?.Count == 1)
            {
                url += $"&sort={sortDescriptions[0].SortPath}";
                if(sortDescriptions[0].Direction == SortDirection.Descending)
                    url += "&sortDirection=desc";
            }
            if(filterExpression != null)
            {
                url += $"&filter={Uri.EscapeUriString(((FilterOperationExpression)filterExpression).Value as string)}";
            }
            var response = await Http.GetFromJsonAsync<CustomerResponse>(new Uri(url, UriKind.Relative), cancellationToken);
            return new Tuple<int, IReadOnlyList<Customer>>(response.TotalCount, response.Customers.ToList());
        }
    }
}
