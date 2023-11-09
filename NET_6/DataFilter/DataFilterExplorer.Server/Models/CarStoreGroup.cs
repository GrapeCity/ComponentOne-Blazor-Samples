using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DataFilterExplorer.Server
{
    public class CarStoreGroup
    {
        public Car Car { get; set; }
        public List<CountInStore> CountInStores { get; set; }

        public int TotalInStores
        {
            get
            {
                return CountInStores == null ? 0 : CountInStores.Sum(x => x.Count);
            }
        }

        public IEnumerable<Color> Colors
        {
            get
            {
                if(CountInStores == null)
                {
                    return new List<Color>();
                }

                var colors = CountInStores.Select(x => x.Color).Distinct();
                return colors;
            }
        }
    }
}
