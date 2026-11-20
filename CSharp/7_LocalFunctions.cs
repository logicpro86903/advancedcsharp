using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class LocalFunctions
    {
        public static void Run()
        {
            DemonstrateLocalFunctions();
        }

        public static void DemonstrateLocalFunctions()
        {
            // Local function defined inside a method
            int Add(int a, int b)
            {
                return a + b;
            }

            // Local function with expression body
            int Multiply(int x, int y) => x * y;

            // Using local functions
            int sum = Add(5, 3);
            int product = Multiply(4, 6);

            Console.WriteLine($"Sum: {sum}, Product: {product}");

            // Local function can access outer method's variables
            int multiplier = 10;
            int MultiplyByTen(int value) => value * multiplier;

            Console.WriteLine($"15 * 10 = {MultiplyByTen(15)}");
        }

    }
}
