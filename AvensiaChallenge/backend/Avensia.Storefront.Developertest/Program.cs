using System;
using System.Linq;
using System.Runtime.InteropServices;
using StructureMap;

namespace Avensia.Storefront.Developertest
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new DefaultRegistry());

            var productListVisualizer = container.GetInstance<ProductListVisualizer>();

            var shouldRun = true;
            int result;
            DisplayOptions();
            string[] currencyCode = { "USD", "GBP", "SEK", "DKK" };

            while (shouldRun)
            {
                Console.Write("Enter an option: ");
                var input = Console.ReadKey();
                Console.WriteLine("\n");
                switch (input.Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        Console.WriteLine("Printing all products");
                        productListVisualizer.OutputAllProduct();
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        Console.WriteLine("Printing paginated products");
                        Console.WriteLine("Please enter page number - 1,2,3, or 4");
                        bool pageNum = Int32.TryParse(Console.ReadLine(), out result);
                        int pageSize = 5;
                        if(result > 0 && result < 5 && !string.IsNullOrEmpty(pageNum.ToString()))
                        {
                            productListVisualizer.OutputPaginatedProducts(result, pageSize);
                        }
                        else
                        {
                            Console.WriteLine("Please input valid page Number...");
                        }
                        break;

                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                        Console.WriteLine("Printing products grouped by price");
                        productListVisualizer.OutputProductGroupedByPriceSegment();
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                        Console.WriteLine("Change Currency");
                        Console.WriteLine("Select Currency - USD, GBP, SEK, DKK");
                        string currency = Console.ReadLine();
                        if (currencyCode.Contains(currency))
                        {
                            foreach (var i in currencyCode)
                            {
                                if (i == currency)
                                {
                                    productListVisualizer.ChangeCurrency(currency);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter correct currency code");
                        }
                        break;
                    case ConsoleKey.Q:
                        shouldRun = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }

                Console.WriteLine();
                DisplayOptions();
            }

            Console.Write("\n\rPress any key to exit!");
            Console.ReadKey();
        }

        private static void DisplayOptions()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1 - Print all products");
            Console.WriteLine("2 - Print paginated products");
            Console.WriteLine("3 - Print products grouped by price");
            Console.WriteLine("4 - Exhchange Currency Rate");
            Console.WriteLine("q - Quit");
        }
    }
}
