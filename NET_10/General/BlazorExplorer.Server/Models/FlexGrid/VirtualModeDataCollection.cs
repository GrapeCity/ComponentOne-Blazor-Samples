using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorExplorer
{
    public class VirtualModeDataCollection : C1VirtualDataCollection<Customer>
    {
        static private readonly List<Customer> customers = Enumerable.Range(0, 100_000).Select(i => new Customer(i)).ToList();

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
            await Task.Delay(500, cancellationToken);//Simulates network traffic.
            var dataCollection = new C1DataCollection<Customer>(customers);

            if (filterExpression != null)
            {
                await dataCollection.FilterAsync(filterExpression);
            }

            if (sortDescriptions?.Count > 0)
            {
                await dataCollection.SortAsync(sortDescriptions.ToArray());
            }
            
            return new Tuple<int, IReadOnlyList<Customer>>(dataCollection.Count, dataCollection.Skip(startingIndex).Take(count).Select(x => (Customer)x).ToList());
        }
    }
}
