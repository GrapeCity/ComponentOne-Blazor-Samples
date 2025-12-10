using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorExplorer
{

    public class CarVirtualCollection : C1VirtualDataCollection<Car>
    {
        private readonly C1DataCollection<Car> _source;
        public CarVirtualCollection()
        {
            var source = new List<Car>();
            var filler = DataProvider.GetCarDataCollection(DataProvider.GetCarTable()).ToList();
            // Uses one set  of items to fill the virtual collection.
            for (int i = 0; i < 30; i++)
            {
                source.AddRange(filler);
            }
            _source = new C1DataCollection<Car>(source);
            this.PageSize = 50;
        }

        public override bool CanSort(params SortDescription[] sortDescriptions)
        {
            return true;
        }

        public override bool CanFilter(FilterExpression filterExpression)
        {
            return true;
        }

        ///<inheritdoc/>
        protected override async Task<Tuple<int, IReadOnlyList<Car>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default)
        {
            // Simulate fetching data process.
            await Task.Delay(300);

            // Filter
            await _source.RemoveFilterAsync();
            if (filterExpression != null)
            {
                await _source.FilterAsync(filterExpression);
            }
            if (sortDescriptions != null)
            {
                await _source.SortAsync(sortDescriptions.ToArray());
            }
            // Form result
            startingIndex = Math.Min(startingIndex, _source.Count);
            count = Math.Min(count, _source.Count - startingIndex);

            var result = _source.ToList().GetRange(startingIndex, count).Select(item => (Car)item).ToList();
            return new Tuple<int, IReadOnlyList<Car>>(_source.Count, result);
        }
    }
}
