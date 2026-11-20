// =============================================================================
//  Feature     : Lambda Expressions
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    Lambdas are a compact syntax for anonymous functions: 'x => x * x'.
//    Parameter types are usually inferred, single-expression bodies need no
//    braces or 'return', and they can capture surrounding variables (closures).
//    .NET 3.5 added the general-purpose Func<...> (returns a value) and
//    Action<...> (returns void) delegate types. Lambdas are what make LINQ and
//    most modern .NET APIs readable.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp
{
    public class LambdaExpressions
    {
        public static void Run()
        {
            DemonstrateEvolution();
            DemonstrateFuncAndAction();
            DemonstrateHigherOrderFunctions();
        }

        // EVOLUTION OF INLINE FUNCTIONS
        // - C# 1: write a named method, then wrap it in an explicit delegate.
        // - C# 2: anonymous method with the 'delegate' keyword and explicit parameter types.
        // - C# 3: lambda 'n => n % 2 == 0' - parameter type inferred from Predicate<int>,
        //         expression body returns its value automatically.
        // All three produce equivalent delegates.
        public static void DemonstrateEvolution()
        {
            Predicate<int> isEven1 = new Predicate<int>(IsEven);
            Predicate<int> isEven2 = delegate (int n) { return n % 2 == 0; };
            Predicate<int> isEven3 = n => n % 2 == 0;

            Console.WriteLine($"{isEven1(4)} {isEven2(4)} {isEven3(4)}"); // True True True
        }

        static bool IsEven(int n) { return n % 2 == 0; }

        // FUNC AND ACTION + LAMBDA SYNTAX FORMS
        // - Func<T1, ..., TResult>: the LAST type argument is the return type.
        // - Action<T1, ...>: returns void.
        // - Syntax forms:
        //     ()     => 42                 no parameters: empty parentheses
        //     x      => x * x              one parameter: parentheses optional
        //     (a, b) => a + b              several parameters: parentheses required
        //     (a, b) => { ...; return; }   STATEMENT body: braces + explicit return
        public static void DemonstrateFuncAndAction()
        {
            Func<int> getAnswer = () => 42;
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (a, b) => a + b;
            Func<double, double, double> hypotenuse = (a, b) =>
            {
                double sumOfSquares = a * a + b * b;
                return Math.Sqrt(sumOfSquares);
            };

            Action sayHi = () => Console.WriteLine("Hi!");
            Action<string, int> repeat = (text, times) =>
            {
                for (int i = 0; i < times; i++) Console.Write(text);
                Console.WriteLine();
            };

            Console.WriteLine($"{getAnswer()}, {square(5)}, {add(2, 3)}, {hypotenuse(3, 4)}"); // 42, 25, 5, 5
            sayHi();
            repeat("ab", 3);                                         // ababab
        }

        // RETURNING A LAMBDA (closure): the returned function remembers 'factor'.
        static Func<int, int> MakeMultiplier(int factor)
        {
            return x => x * factor;
        }

        // ACCEPTING A LAMBDA: the caller supplies the filtering logic.
        static List<T> Filter<T>(List<T> items, Func<T, bool> predicate)
        {
            var result = new List<T>();
            foreach (var item in items)
                if (predicate(item)) result.Add(item);
            return result;
        }

        // HIGHER-ORDER FUNCTIONS
        // - Functions that take or return other functions.
        // - MakeMultiplier(3) returns a new function "multiply by 3".
        // - Filter(words, w => w.Length <= 4) passes the condition as a lambda.
        // - LINQ is the same idea: OrderBy/Select receive lambdas describing what to do.
        public static void DemonstrateHigherOrderFunctions()
        {
            var triple = MakeMultiplier(3);
            Console.WriteLine(triple(7));                                   // 21

            var words = new List<string> { "apple", "kiwi", "banana", "fig" };
            var shortWords = Filter(words, w => w.Length <= 4);
            Console.WriteLine(string.Join(", ", shortWords));               // kiwi, fig

            var lengths = words.OrderBy(w => w.Length).Select(w => $"{w}({w.Length})");
            Console.WriteLine(string.Join(", ", lengths));                  // fig(3), kiwi(4), apple(5), banana(6)
        }
    }

    /*

    A lambda can become either:
    - a delegate (compiled code you can call)   Func<int, bool> f = x => x > 5;
    - an expression tree (data describing code)  Expression<Func<int, bool>> e = x => x > 5;
    See 3_ExpressionTrees.cs - that's how Entity Framework turns lambdas into SQL.

    //1. Same closure pitfalls as anonymous methods (captured variables, not values).
    //2. Lambdas capturing variables allocate - use 'static' lambdas (C# 9) to prevent capture.

    Later improvements: C# 10 natural type & attributes (10_LambdaImprovements.cs),
    C# 12 default parameters (12_DefaultLambdaParameters.cs).

     */
}
