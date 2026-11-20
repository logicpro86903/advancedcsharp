using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 6 / C# 10.0 (2021)
    public class LambdaImprovements
    {
        public static void Run()
        {

            // Option 1: Explicit delegate type declaration
            //Func<int, int, int> add = (int x, int y) => x + y;
            //Func<int, int, int> multiply = (int x, int y) => x * y;
            //Func<string, string> greet = (string name) => $"Hello, {name}!";
            //Func<int, bool> isEven = (int n) => n % 2 == 0;

            // Option 2: Cast to delegate type
            //var add = (Func<int, int, int>)((int x, int y) => x + y);

            // Option 3: Parameter types could sometimes be inferred from the delegate
            //Func<int, int, int> add = (x, y) => x + y; // Types inferred from Func<int, int, int>


            // C# 10.0+ - Lambdas can be assigned to var without explicit delegate types
            var add = (int x, int y) => x + y;
            var multiply = (int x, int y) => x * y;
            var greet = (string name) => $"Hello, {name}!";
            var isEven = (int n) => n % 2 == 0;

            // Type inference works automatically
            Console.WriteLine(add(5, 3)); // 8
            Console.WriteLine(multiply(4, 7)); // 28
            Console.WriteLine(greet("Alice")); // Hello, Alice!
            Console.WriteLine(isEven(10)); // True
        }
    }

    /*
     C# 10.0 introduced several enhancements to lambda expressions, including natural types for lambdas, 
    attributes on lambdas, and improved type inference that make functional programming more 
    powerful and expressive.
     */
}
