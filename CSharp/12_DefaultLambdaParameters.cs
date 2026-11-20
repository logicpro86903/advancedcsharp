using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 8 / C# 12.0 (2023)
    public class DefaultLambdaParameters
    {
        public static void Run()
        {
            // Lambda with default parameters
            var greet = (string name = "World", string greeting = "Hello") => $"{greeting}, {name}!";

            // Usage with different parameter combinations
            Console.WriteLine(greet()); // "Hello, World!"
            Console.WriteLine(greet("Alice")); // "Hello, Alice!"
            Console.WriteLine(greet("Bob", "Hi")); // "Hi, Bob!"

            // Lambda with mixed parameter types
            var calculatePrice = (decimal basePrice, decimal taxRate = 0.08m, decimal discount = 0.0m) =>
                basePrice * (1 + taxRate) * (1 - discount);

            Console.WriteLine($"Price: ${calculatePrice(100m):F2}"); // $108.00 (with 8% tax)
            Console.WriteLine($"Price: ${calculatePrice(100m, 0.1m):F2}"); // $110.00 (with 10% tax)
            Console.WriteLine($"Price: ${calculatePrice(100m, 0.08m, 0.15m):F2}"); // $91.80 (with tax and discount)

            // Lambda for string formatting with defaults
            var formatMessage = (string message, string prefix = "[INFO]", DateTime? timestamp = null) =>
                $"{prefix} {timestamp?.ToString("HH:mm:ss") ?? DateTime.Now.ToString("HH:mm:ss")}: {message}";

            Console.WriteLine(formatMessage("System started"));
            Console.WriteLine(formatMessage("User logged in", "[AUTH]"));
            Console.WriteLine(formatMessage("Error occurred", "[ERROR]", new DateTime(2024, 12, 10, 14, 30, 0)));
        }
    }

    /*
     Default lambda parameters allow you to specify default values for lambda expression parameters, 
    making lambdas more flexible and easier to use, similar to how default parameters work in regular 
    methods.
     */
}
