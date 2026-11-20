// =============================================================================
//  Feature     : Default Lambda Parameters
//  C# Version  : C# 12.0
//  .NET        : .NET 8
//  Released    : 2023 (Visual Studio 2022 v17.8)
// -----------------------------------------------------------------------------
//  Summary:
//    Lambda parameters can now declare DEFAULT VALUES, exactly like optional
//    method parameters: (string name = "World") => ... Callers may then omit
//    those arguments. C# 12 also allows 'params' arrays in lambdas. Together with
//    the lambda natural type (C# 10) this makes lambdas behave like small local
//    methods, and lets Minimal API handlers declare optional parameters.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class DefaultLambdaParameters
    {
        public static void Run()
        {
            // LAMBDA WITH DEFAULT PARAMETERS
            // - Both parameters are optional; omitted arguments take the default values.
            // - Assigned to 'var', the lambda gets a compiler-generated delegate type that
            //   REMEMBERS the defaults (a plain Func<string, string, string> could not).
            var greet = (string name = "World", string greeting = "Hello") => $"{greeting}, {name}!";

            Console.WriteLine(greet());            // Hello, World!
            Console.WriteLine(greet("Alice"));     // Hello, Alice!
            Console.WriteLine(greet("Bob", "Hi")); // Hi, Bob!

            // MIXING REQUIRED AND OPTIONAL PARAMETERS
            // - As with methods, required parameters (basePrice) must come BEFORE optional ones.
            // - Defaults must be compile-time constants (0.08m, 0.0m, null...).
            var calculatePrice = (decimal basePrice, decimal taxRate = 0.08m, decimal discount = 0.0m) =>
                basePrice * (1 + taxRate) * (1 - discount);

            Console.WriteLine($"Price: ${calculatePrice(100m):F2}");               // $108.00
            Console.WriteLine($"Price: ${calculatePrice(100m, 0.1m):F2}");         // $110.00
            Console.WriteLine($"Price: ${calculatePrice(100m, 0.08m, 0.15m):F2}"); // $91.80

            // NULL AS A DEFAULT FOR NON-CONSTANT VALUES
            // - DateTime.Now isn't a constant, so it can't be a default. The common workaround is a
            //   nullable parameter defaulting to null, then substituting the real value with ??.
            var formatMessage = (string message, string prefix = "[INFO]", DateTime? timestamp = null) =>
                $"{prefix} {timestamp?.ToString("HH:mm:ss") ?? DateTime.Now.ToString("HH:mm:ss")}: {message}";

            Console.WriteLine(formatMessage("System started"));
            Console.WriteLine(formatMessage("User logged in", "[AUTH]"));
            Console.WriteLine(formatMessage("Error occurred", "[ERROR]", new DateTime(2024, 12, 10, 14, 30, 0)));
        }
    }

    /*

    Also new in C# 12 for lambdas - params arrays:
        var sum = (params int[] values) => values.Sum();
        Console.WriteLine(sum(1, 2, 3));   // 6

    //1. If you assign the lambda to an existing delegate type (Func<...>), the defaults are ignored
    //   and the compiler warns - callers must pass every argument.

     */
}
