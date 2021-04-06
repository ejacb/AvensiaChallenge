using System.Net.Http.Headers;

namespace Avensia.Storefront.Developertest
{
    public class DefaultProductDto : IProductDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        
    }
}