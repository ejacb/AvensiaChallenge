using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Avensia.Storefront.Developertest
{
    public class ProductListVisualizer
    {
        private readonly IProductRepository _productRepository;
        private IDictionary<string, decimal> ConversionRate;
        private string CurrencyCode;

        public ProductListVisualizer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Outputs all the product
        public void OutputAllProduct()
        {
            CheckConversion();
            var products = _productRepository.GetProducts(CurrencyCode);
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}\t{product.ProductName}\t{product.Price }");
            }
        }

        //Outputs all the product in with pagination
        public void OutputPaginatedProducts(int pageNum, int pageSize)
        {
            CheckConversion();
            var products = _productRepository.GetProducts(pageNum, pageSize, CurrencyCode);
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}\t{product.ProductName}\t{product.Price }");
            }
        }

        //Outputs all the product grouped by price (100) segment
        public void OutputProductGroupedByPriceSegment()
        {
            CheckConversion();
            var products = _productRepository.GetProductsSorted(CurrencyCode);
            
            foreach (var range in products)
            {
                Console.WriteLine($"\n{range.Min}-{range.Max}\t{ConversionRate.Keys.FirstOrDefault()}");
                foreach(var product in range.Product)
                {
                    Console.WriteLine($"{product.Id}\t{product.ProductName}\t{product.Price}");
                }
            }
        }

        //Changes the currency (e.g US -> DKK)
        public void ChangeCurrency(string currency)
        {
            var products = _productRepository.ChangeCurrency(currency);
            ConversionRate = products;
            CurrencyCode = products.Keys.FirstOrDefault();
            Console.WriteLine("It has been converted to:" + ConversionRate.Keys.FirstOrDefault());
           
        }

        //Checks the string CurrencyCode if empty. If empty, it will be set to its default key (USD). 
        //Checks the Dictionary ConversionRate if empty. If empty, it will be set to its default key and value (USD, 1.0). 
        public void CheckConversion()
        {
            if(ConversionRate == null)
            {
                ConversionRate = new Dictionary<string, decimal>();
                ConversionRate.Add("USD", 1.0M);
            }
            if(CurrencyCode == null)
            {
                CurrencyCode = "USD";
            }
        }
    }
}