using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace C1ListViewExplorer.Pages
{
    public class VirtualModeDataCollection : C1VirtualDataCollection<Customer>
    {
        protected override async Task<Tuple<int, IReadOnlyList<Customer>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(500, cancellationToken);//Simulates network traffic.
            return new Tuple<int, IReadOnlyList<Customer>>(10_000, Enumerable.Range(startingIndex, count).Select(i => new Customer(i)).ToList());
        }
    }
}
