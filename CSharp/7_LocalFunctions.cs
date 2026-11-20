// =============================================================================
//  Feature     : Local Functions
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    A local function is a method declared INSIDE another method. It is visible
//    only within that method, keeping single-use helpers next to the code that
//    uses them instead of cluttering the class. Unlike lambdas, local functions
//    can be generic, recursive, use ref/out/params, be called before their
//    declaration, and usually avoid delegate allocations. They can also capture
//    the enclosing method's variables.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class LocalFunctions
    {
        public static void Run()
        {
            DemonstrateLocalFunctions();
        }

        // LOCAL FUNCTIONS
        // - 'Add' is declared with a normal block body; 'Multiply' with an expression body (=>).
        // - Both exist only inside DemonstrateLocalFunctions - nothing else in the class can call them.
        // - CAPTURING: MultiplyByTen reads 'multiplier', a local of the enclosing method, just like
        //   a lambda closure would.
        // - Mark a local function 'static' (C# 8) to forbid capturing and guarantee no hidden state.
        public static void DemonstrateLocalFunctions()
        {
            int Add(int a, int b)
            {
                return a + b;
            }

            int Multiply(int x, int y) => x * y;

            int sum = Add(5, 3);
            int product = Multiply(4, 6);

            Console.WriteLine($"Sum: {sum}, Product: {product}");

            int multiplier = 10;
            int MultiplyByTen(int value) => value * multiplier;

            Console.WriteLine($"15 * 10 = {MultiplyByTen(15)}");
        }
    }

    /*

    Classic use - eager argument validation for iterators/async methods:
        public static IEnumerable<int> Range(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count)); // runs immediately
            return Iterator();
            IEnumerable<int> Iterator() { for (int i = 0; i < count; i++) yield return i; }
        }

    Local function vs lambda:
    - Local functions can be recursive and generic without extra tricks.
    - A lambda always creates a delegate object; a local function only does if converted to one.

     */
}
