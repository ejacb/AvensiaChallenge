using System.Collections;
using System.Collections.Generic;

namespace Avensia.Storefront.Developertest
{
    public interface IProductRepository
    {
        IEnumerable<IProductDto> GetProducts(string CurrencyCode);
        IEnumerable<IMinMaxRange> GetProductsSorted(string CurrencyCode);
        IEnumerable<IProductDto> GetProducts(int start, int pageSize, string CurrencyCode);
        IDictionary<string,decimal> ChangeCurrency(string currency);


    }
}