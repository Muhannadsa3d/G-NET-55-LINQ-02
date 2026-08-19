using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G_NET_55_LINQ_02
{


    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
    }

    public class Order
    {
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string Country { get; set; }
        public List<Order> Orders { get; set; }
    }


    internal class Program
    {
        static void Main(string[] args)
        {

            #region  Assignment02LINQ

            var catalog = new List<Product>
        {
            new Product { Name = "Salmon", Category = "Seafood", Price = 12.5m , UnitsInStock=0},
            new Product { Name = "Tuna", Category = "Seafood", Price = 9.8m , UnitsInStock=30 },
            new Product { Name = "Bread", Category = "Bakery", Price = 2.3m , UnitsInStock=1 }
        };

            //1.Get top 3 most expensive products
            var top3 = catalog.OrderByDescending(p => p.Price).Take(3);
            foreach (var p in top3) Console.WriteLine($"{p.Name} - ${p.Price}");

            //---------------------------------------------------

            //2.show page 2 of products, with page size = 5
            var page2 = catalog.Skip(5).Take(5);
            foreach (var p in page2) Console.WriteLine($"{p.Name}");

            //---------------------------------------------------

            //3.Take products from the list as long as Their UnitPrice is less than $25(list is ordered by price).
            var cheapProducts = catalog.OrderBy(p => p.Price)
                                       .TakeWhile(p => p.Price < 25);
            foreach (var p in cheapProducts) Console.WriteLine($"{p.Name} - ${p.Price}");

            //---------------------------------------------------

            //4.Check if ALL products in the "Seafood" category are in stock
            bool allSeafoodInStock = catalog.Where(p => p.Category == "Seafood")
                                .All(p => p.UnitsInStock > 0);
            Console.WriteLine($"All Seafood in stock? {allSeafoodInStock}");

            //---------------------------------------------------

            //5.Check if the ID list contains 9 int[] ids = { 3, 9, 13, 18 };
            int[] ids = { 3, 9, 13, 18 };
            Console.WriteLine($"IDs contain 9? {ids.Contains(9)}");

            //---------------------------------------------------


            //6.Group all products by Category and print each group with its product count.
            var groupedCounts = catalog.GroupBy(p => p.Category)
                           .Select(g => new { g.Key, Count = g.Count() });
            foreach (var g in groupedCounts) Console.WriteLine($"{g.Key}: {g.Count}");

            //---------------------------------------------------


            //7.Group products by Category and project only product names per group
            var groupedNames = catalog.GroupBy(p => p.Category)
                          .Select(g => new { g.Key, Names = g.Select(p => p.Name) });
            foreach (var g in groupedNames)
                Console.WriteLine($"{g.Key}: {string.Join(", ", g.Names)}");

            //---------------------------------------------------


            //8.Find all categories that have MORE THAN 3 products
            var bigCategories = catalog.GroupBy(p => p.Category)
                                       .Where(g => g.Count() > 3)
                                       .Select(g => g.Key);
            foreach (var c in bigCategories) Console.WriteLine(c);

            //---------------------------------------------------


            //10.Calculate the total number of units in stock across all products
            int totalUnits = catalog.Sum(p => p.UnitsInStock);
            Console.WriteLine($"Total Units: {totalUnits}");

            //---------------------------------------------------


            //11.Find the CHEAPEST and MOST EXPENSIVE product prices
            decimal minPrice = catalog.Min(p => p.Price);
            decimal maxPrice = catalog.Max(p => p.Price);
            Console.WriteLine($"Min: {minPrice}, Max: {maxPrice}");


            //---------------------------------------------------

            //12.Get a distinct list of all product categories
            var categories = catalog.Select(p => p.Category).Distinct();
            Console.WriteLine("--- Distinct Categories ---");
            foreach (var c in categories) Console.WriteLine(c);

            //---------------------------------------------------


            //13;find product IDs that are in setA but NOT in setB
            int[] setA = { 1, 3, 5, 7, 9, 11, 13 };
            int[] setB = { 3, 6, 9, 12, 15, 13 };
            var diff = setA.Except(setB);
            Console.WriteLine("--- SetA \\ SetB ---");
            foreach (var id in diff) Console.WriteLine(id);


            //---------------------------------------------------


            //14.Find countries that appear in list1 but NOT in list2
            string[] list1 = { "Germany", "France", "UK", "Spain" };
            string[] list2 = { "france", "SPAIN", "Italy" };
            var diffCountries = list1.Except(list2, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("--- Countries diff ---");
            foreach (var c in diffCountries) Console.WriteLine(c);

            //---------------------------------------------------


            //15.Build a Dictionary<int, Product> keyed by ProductID. Then retrieve and print the product with ID = 18.
            var dict = catalog.Select((p, i) => new { Key = i + 1, Value = p })
                  .ToDictionary(x => x.Key, x => x.Value);
            var product18 = dict.ContainsKey(18) ? dict[18] : null;
            Console.WriteLine($"Product with ID=18: {product18?.Name ?? "Not Found"}");

            //---------------------------------------------------


            //16.Get the first product whose price is greater than $50.
            var firstExpensive = catalog.FirstOrDefault(p => p.Price > 50);
            Console.WriteLine($"First > 50: {firstExpensive?.Name ?? "None"}");

            //---------------------------------------------------


            //17.Try to get the first product with a price > $500.it returns null instead of throwing.
            var maybeProduct = catalog.FirstOrDefault(p => p.Price > 500);
            Console.WriteLine($"First > 500: {maybeProduct?.Name ?? "null"}");

            //---------------------------------------------------


            //18.Generate a multiplication table row for 7
            var table7 = Enumerable.Range(1, 10).Select(i => $"{7} x {i} = {7 * i}");
            foreach (var row in table7) Console.WriteLine(row);

            //---------------------------------------------------


            //19.Generate even numbers between 1 and 30.
            var evens = Enumerable.Range(1, 30).Where(n => n % 2 == 0);
            Console.WriteLine("--- Evens ---");
            foreach (var e in evens) Console.WriteLine(e);

            //---------------------------------------------------


            //20.Concatenate the first 3 product names with the first 3 customer company names into a single sequence.
            var concat = catalog.Select(p => p.Name).Take(3)
                                .Concat(customer.Select(c => c.CustomerID).Take(3));

            Console.WriteLine("--- Concatenated ---");
            foreach (var item in concat)
            {
                Console.WriteLine(item);
            }

            //---------------------------------------------------

            //21.Pair each product with a customer(by position) and produce a string "ProductName sold to CompanyName".
            var paired = catalog.Zip(customer, (p, c) => $"{p.Name} sold to {c.CustomerID}");
            Console.WriteLine("--- Paired ---");
            foreach (var item in paired) Console.WriteLine(item);
            #endregion
        }
    }
}
