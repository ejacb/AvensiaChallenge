using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Avensia.Storefront.Developertest
{
    public class MinMaxRange : IMinMaxRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public List<IProductDto> Product { get; set; }

    }
}