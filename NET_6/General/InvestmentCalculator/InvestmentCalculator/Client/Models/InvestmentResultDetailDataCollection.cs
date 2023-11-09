using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using InvestmentCalculator.Shared;
using C1.DataCollection.Serialization;

namespace InvestmentCalculator.Client
{
    public class InvestmentResultDetailDataCollection : C1VirtualDataCollection<InvestmentResultDetail>
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

        protected override async Task<Tuple<int, IReadOnlyList<InvestmentResultDetail>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            string url = $"InvestmentResultDetail?skip={startingIndex}&take={count}";
            if (filterExpression != null)
            {
                var options = new JsonSerializerOptions { Converters = { new FilterExpressionJsonConverter() } };
                url += $"&filter={Uri.EscapeDataString(JsonSerializer.Serialize(filterExpression, options))}";
            }
            if (sortDescriptions?.Count > 0)
            {
                url += $"&sort={Uri.EscapeDataString(JsonSerializer.Serialize(sortDescriptions))}";
            }

            var response = await Http.GetFromJsonAsync<InvestmentResultDetailResponse>(new Uri(url, UriKind.Relative), cancellationToken);
            return new Tuple<int, IReadOnlyList<InvestmentResultDetail>>(response.TotalCount, response.InvestmentResultDetails.ToList());
        }
    }
}
