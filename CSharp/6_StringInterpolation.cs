// =============================================================================
//  Feature     : String Interpolation ($"...")
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    Prefixing a string literal with '$' lets you embed C# expressions directly
//    inside {braces}: $"Hello {name}". It replaces error-prone string.Format
//    index placeholders ({0}, {1}) and '+' concatenation. Each hole supports an
//    optional ALIGNMENT and FORMAT: {value,alignment:format}. Combine with '@'
//    ($@"...") for verbatim, multi-line strings without escaping backslashes.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace CSharp
{
    public class StringInterpolation
    {
        public static void Run()
        {
            // BASIC INTERPOLATION vs THE OLD WAYS
            // - $"... {name} ... {age} ..." evaluates each expression and inserts its ToString().
            // - string.Format uses numbered placeholders that can get out of sync with arguments.
            // - Concatenation with '+' is noisy and easy to get spacing wrong.
            string name = "John";
            int age = 30;
            string message = $"Hello, my name is {name} and I am {age} years old.";
            Console.WriteLine(message);
            // Hello, my name is John and I am 30 years old.

            string oldMessage = string.Format("Hello, my name is {0} and I am {1} years old.", name, age);
            string concatMessage = "Hello, my name is " + name + " and I am " + age + " years old.";


            // NUMERIC FORMAT STRINGS: {value:format}
            // - F2 = fixed-point, 2 decimals    C = currency    D3 = integer padded to 3 digits
            // - P  = percentage (x100 + %)      E2 = scientific notation, 2 decimals
            // - Any expression can go in the hole, including arithmetic in parentheses.
            // - '$@' combines interpolation with a verbatim (multi-line) string.
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
            // Item Price:    $19.95
            // Tax:           $1.99
            // Quantity:      003
            // Total:         $65.82
            // Percentage:    15.00%
            // Scientific:    1.23E+004


            // DATE/TIME FORMAT STRINGS
            // - Standard specifiers: d (short date), D (long date), t (short time), T (long time).
            // - Custom patterns: yyyy, MM, dd, HH, mm, MMMM (month name), dddd (day name).
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

            // MORE NUMBER FORMATS
            // - N0 = thousands separators, no decimals   X = hexadecimal   D10 = zero-pad to 10
            // - P1 = percentage with 1 decimal
            // - Custom digit patterns using '#' placeholders (e.g. phone number layout).
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


            // ALIGNMENT: {value,width}
            // - Positive width right-aligns, negative width left-aligns, padding with spaces.
            // - Perfect for printing simple text tables in the console.
            string[] names = { "Alice", "Bob", "Charlie", "Diana" };
            int[] scores = { 95, 87, 92, 88 };

            Console.WriteLine($"{"Name",-10} {"Score",5}");
            Console.WriteLine($"{new string('-', 15)}");

            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{names[i],-10} {scores[i],5}");
            }
            // Name       Score
            // ---------------
            // Alice         95
            // Bob           87
            // Charlie       92
            // Diana         88

            // INTERPOLATING OBJECTS AND CONDITIONAL EXPRESSIONS
            // - Property access and format specifiers combine: {product.Price:C}.
            // - A conditional (?:) must be wrapped in PARENTHESES, because ':' would otherwise be
            //   read as the start of a format string.
            // - {product} calls the object's ToString() override.
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


            // METHOD CALLS AND LINQ INSIDE HOLES
            // - Any expression is allowed, including method calls, lambdas and LINQ queries.
            // - Keep holes simple for readability; complex logic is better computed beforehand.
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

            // VERBATIM INTERPOLATED STRINGS ($@)
            // - In a regular $"..." string a backslash must be escaped as '\\'.
            // - In $@"..." backslashes are literal and the string may span multiple lines,
            //   which suits file paths, connection strings and SQL text.
            string serverName = "MyServer";
            string databaseName = "ProductionDB";
            string userName = "admin";

            string connectionString1 = $"Server={serverName}\\SQLEXPRESS;Database={databaseName};User Id={userName};";

            string connectionString2 = $@"Server={serverName}\SQLEXPRESS;Database={databaseName};User Id={userName};";

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

    //1. Never build SQL from user input with interpolation - that's SQL injection.
    //   Use parameterized queries (EF Core's FromSqlInterpolated turns holes into parameters safely).

    //2. Formatting uses the CURRENT culture (decimal separator, currency symbol).
    //   Use FormattableString.Invariant($"...") or string.Create(CultureInfo.InvariantCulture, $"...").

    //3. To output a literal brace, double it: $"{{not a hole}}".

    Later: C# 10 interpolated string handlers (faster, less allocation),
           C# 11 newlines inside holes and raw interpolated strings $"""...""" (see 11_RawStringLiterals.cs).

     */
}
