using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class StringInterpolation
    {
        public static void Run()
        {
            // Basic string interpolation
            string name = "John";
            int age = 30;
            string message = $"Hello, my name is {name} and I am {age} years old.";
            Console.WriteLine(message);
            // Output: Hello, my name is John and I am 30 years old.

            // Without interpolation (old way)
            string oldMessage = string.Format("Hello, my name is {0} and I am {1} years old.", name, age);
            string concatMessage = "Hello, my name is " + name + " and I am " + age + " years old.";


            //Numeric Formatting
            double price = 19.95;
            decimal tax = 1.99m;
            int quantity = 3;

            string receipt = $@"
            Item Price:    ${price:F2}
            Tax:           ${tax:C}
            Quantity:      {quantity:D3}
            Total:         ${(price + (double)tax) * quantity:F2}
            Percentage:    {0.15:P}
            Scientific:    {12345.67:E2}
            ";

            Console.WriteLine(receipt);
            // Output:
            // Item Price:    $19.95
            // Tax:           $1.99
            // Quantity:      003
            // Total:         $65.82
            // Percentage:    15.00%
            // Scientific:    1.23E+004


            //DateTime Formatting
            DateTime currentDate = DateTime.Now;

            string dateFormats = $@"
            Default:           {currentDate}
            Short Date:        {currentDate:d}
            Long Date:         {currentDate:D}
            Short Time:        {currentDate:t}
            Long Time:         {currentDate:T}
            Custom:            {currentDate:yyyy-MM-dd HH:mm}
            Month/Year:        {currentDate:MMMM yyyy}
            Day of Week:       {currentDate:dddd}
            ISO 8601:          {currentDate:yyyy-MM-ddTHH:mm:ssZ}
            ";

            Console.WriteLine(dateFormats);

            //Custom Number Formats
            int number = 1234567;
            double percentage = 0.847;

            string customFormats = $@"
            Number with commas:     {number:N0}
            Currency:               {number:C}
            Hexadecimal:           {number:X}
            Zero padding:          {number:D10}
            Percentage:            {percentage:P1}
            Phone number format:   {5551234567:###-###-####}
            ";

            Console.WriteLine(customFormats);


            //Alignment and Padding
            string[] names = { "Alice", "Bob", "Charlie", "Diana" };
            int[] scores = { 95, 87, 92, 88 };

            Console.WriteLine($"{"Name",-10} {"Score",5}");
            Console.WriteLine($"{new string('-', 15)}");

            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{names[i],-10} {scores[i],5}");
            }

            // Output:
            // Name       Score
            // ---------------
            // Alice         95
            // Bob           87
            // Charlie       92
            // Diana         88

            //Complex Object Interpolation
            var product = new Product
            {
                Name = "Laptop",
                Price = 999.99m,
                Stock = 15,
                LastUpdated = DateTime.Now
            };

            string productInfo = $@"
            Product Details:
            - Name: {product.Name}
            - Price: {product.Price:C}
            - Stock: {product.Stock} units
            - Status: {(product.Stock > 0 ? "In Stock" : "Out of Stock")}
            - Last Updated: {product.LastUpdated:MMM dd, yyyy}
            - ToString(): {product}
            ";

            Console.WriteLine(productInfo);


            //LINQ with String Interpolation
            var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            string summary = $@"
                Numbers: {string.Join(", ", numbers)}
                Count: {numbers.Length}
                Sum: {numbers.Sum()}
                Average: {numbers.Average():F2}
                Even numbers: {string.Join(", ", numbers.Where(n => n % 2 == 0))}
                Odd numbers: {string.Join(", ", numbers.Where(n => n % 2 != 0))}
                Max: {numbers.Max()}
                Min: {numbers.Min()}
                ";

            Console.WriteLine(summary);

            //Verbatim String Interpolation
            string serverName = "MyServer";
            string databaseName = "ProductionDB";
            string userName = "admin";

            // Regular interpolated string (escaping required)
            string connectionString1 = $"Server={serverName}\\SQLEXPRESS;Database={databaseName};User Id={userName};";

            // Verbatim interpolated string (no escaping needed)
            string connectionString2 = $@"Server={serverName}\SQLEXPRESS;Database={databaseName};User Id={userName};";

            // Multi-line SQL with interpolation
            string sqlQuery = $@"
            SELECT 
                p.ProductId,
                p.ProductName,
                p.Price,
                c.CategoryName
            FROM Products p
            INNER JOIN Categories c ON p.CategoryId = c.CategoryId
            WHERE p.Price > {100}
                AND p.LastUpdated >= '{DateTime.Now.AddDays(-30):yyyy-MM-dd}'
            ORDER BY p.Price DESC";

            Console.WriteLine(sqlQuery);
                       
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime LastUpdated { get; set; }

        public override string ToString()
        {
            return $"Product[{Name}]";
        }
    }


    /*
     //String interpolation allows you to embed expressions directly into string literals 
       //using the $ prefix and {} placeholders. It was introduced in C# 6.0 and provides 
        //a more readable alternative to string.Format() and concatenation.
     */
}
