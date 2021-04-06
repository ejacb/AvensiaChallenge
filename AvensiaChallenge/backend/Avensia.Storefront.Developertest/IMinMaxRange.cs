using System.Collections.Generic;

namespace Avensia.Storefront.Developertest
{
    public interface IMinMaxRange
    {

         int Min { get; set; }
         int Max { get; set; }
         List<IProductDto> Product { get; set; }
    }
}