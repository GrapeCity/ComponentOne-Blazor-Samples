using BlazorExplorer.Pages;
using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorExplorer
{
    public class FinancialDataCollection : C1VirtualDataCollection<FinancialData>
    {

        private readonly List<FinancialData> financialData;

        public FinancialDataCollection()
        {
            financialData = FinancialData.GetFinancialData();
        }
        public override bool CanSort(params SortDescription[] sortDescriptions)
        {
            return true;
        }

        public override bool CanFilter(FilterExpression filterExpression)
        {
            return !(filterExpression is FilterPredicateExpression);
        }

        protected override async Task<Tuple<int, IReadOnlyList<FinancialData>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(500, cancellationToken);//Simulates network traffic.
            var dataCollection = new C1DataCollection<FinancialData>(financialData);

            if (filterExpression != null)
            {
                await dataCollection.FilterAsync(filterExpression);
            }

            if (sortDescriptions?.Count > 0)
            {
                await dataCollection.SortAsync(sortDescriptions.ToArray());
            }

            var data = dataCollection.Skip(startingIndex).Take(count);
            return new Tuple<int, IReadOnlyList<FinancialData>>(dataCollection.Count, data.Select(x => (FinancialData)x).ToList());
        }
    }
}
