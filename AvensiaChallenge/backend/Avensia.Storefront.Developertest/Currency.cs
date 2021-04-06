using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace Avensia.Storefront.Developertest
{
    public class Currency
    {

        public Dictionary<string, decimal> currency = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> Conversion 
        {
            get { return currency; }
            set { Conversion = currency; } 
        }

        public Currency()
        {
            currency.Add("USD", 1.0M);
            currency.Add("GBP", 0.71M);
            currency.Add("SEK", 8.38M);
            currency.Add("DKK", 6.06M);
        }
    }
}