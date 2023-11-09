using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SQLServerRealTimeUpdates.Data
{
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductComparer : EqualityComparer<Product>
    {
        public override bool Equals([AllowNull] Product x, [AllowNull] Product y)
        {
            if (null == x && null == y)
                return true;
            if (null == x || null == y)
                return false;
            return x.Code == y.Code;
        }

        public override int GetHashCode([DisallowNull] Product obj)
        {
            return obj.Code.GetHashCode();
        }
    }
}
