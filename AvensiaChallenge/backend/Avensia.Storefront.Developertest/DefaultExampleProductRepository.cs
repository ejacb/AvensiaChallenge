using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;

namespace Avensia.Storefront.Developertest
{
    public class DefaultExampleProductRepository : IProductRepository
    {
        public IEnumerable<IProductDto> GetProducts(string CurrencyCode)
        {
            //Reading JSON file
            string strResultJson = File.ReadAllText(@"products.json");
            List<DefaultProductDto> products = new List<DefaultProductDto>();

            //Initializing Cache
            var cache = MemoryCache.Default;
            products = (List<DefaultProductDto>)cache.Get("products");

            if (products == null)
            {
                products = new List<DefaultProductDto>();
                products = JsonConvert.DeserializeObject<List<DefaultProductDto>>(strResultJson);
                System.Diagnostics.Debug.WriteLine("products were not in cache");

                //put in cache and set policy
                var policy = new CacheItemPolicy().AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                cache.Set("products", products, policy);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("products were in cache");
            }



            return products;
        }

        public IEnumerable<IProductDto> GetProducts(int start, int pageSize, string CurrencyCode)
        {
            IEnumerable<IProductDto> products = GetProducts(CurrencyCode);
            //usign skip and take operators for pagination
            var result = products.Skip((start - 1) * pageSize).Take(pageSize);

            return result;
        }

        public IEnumerable<IMinMaxRange> GetProductsSorted(string CurrencyCode)
        {
           
            IEnumerable<IProductDto> products = GetProducts(CurrencyCode);
            var getMaxPrice = products.OrderByDescending(c => c.Price).Select(c => c.Price).FirstOrDefault();
            var range = (Math.Ceiling(Decimal.Parse(getMaxPrice.ToString()) / 100));
            List<int> rangeArray = new List<int>();

            for (var x = 0; x <= range; x++)
            {
                rangeArray.Add(x * 100);
            }

            var dto = new List<MinMaxRange>();

            foreach (var r in rangeArray)
            {
                var SplitInvoice = products
                    .Where(x => x.Price < r && x.Price > r - 100)
                    .Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 100)
                    .Select(x => x.Select(v => v.Value).ToList())
                    .OrderBy(x => range)
                    .ToList();

                foreach (var splitinvice in SplitInvoice)
                {
                    var map = new MinMaxRange()
                    {
                        Product = (splitinvice),
                        Min = r - 100,
                        Max = r
                    };

                    dto.Add(map);
                }
            }

            return dto;
        }

        //Converts Currency
        public IDictionary<string, decimal> ChangeCurrency(string CurrencyCode)
        {
            var products = GetProducts(CurrencyCode);
            var defaultProducts = GetDefaultValues();
            decimal exchangeRate = 1.0M;
            string currencyCode = "USD";
            Currency currencyList = new Currency();
            Dictionary<string, decimal> currencyDictionary = new Dictionary<string, decimal>();

            foreach(var x in currencyList.Conversion)
            {
                if(CurrencyCode == x.Key)
                {
                    exchangeRate = x.Value;
                    currencyCode = x.Key;
                    if (currencyCode == x.Key)
                    {
                        foreach (var p in products.Zip(defaultProducts, Tuple.Create))
                        {
                            p.Item1.Price =  p.Item2.Price * x.Value;
                        }
                        break;
                    }
                }
            }

            currencyDictionary.Add(currencyCode, exchangeRate);
            return currencyDictionary;
        }

        //Get Default Values (USD)
        public IEnumerable<IProductDto> GetDefaultValues()
        {
            string strResultJson = File.ReadAllText(@"products.json");
            var products = JsonConvert.DeserializeObject<List<DefaultProductDto>>(strResultJson);

            return products;
        }
    }

}