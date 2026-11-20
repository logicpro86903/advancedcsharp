// =============================================================================
//  Feature     : Lambda Improvements (natural type, explicit return type, attributes)
//  C# Version  : C# 10.0
//  .NET        : .NET 6
//  Released    : 2021 (Visual Studio 2022)
// -----------------------------------------------------------------------------
//  Summary:
//    Lambdas gained a NATURAL TYPE: when the parameter types are known, the
//    compiler infers a Func<>/Action<> delegate type, so a lambda can be assigned
//    to 'var' without spelling out the delegate. C# 10 also allows an explicit
//    RETURN TYPE before the parameters ('int (x) => x') and ATTRIBUTES on
//    lambdas and their parameters. These changes power ASP.NET Core Minimal APIs:
//    app.MapGet("/", () => "Hello").
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class LambdaImprovements
    {
        // BEFORE C# 10 (for comparison):
        //   Func<int, int, int> add = (x, y) => x + y;             // delegate type written out
        //   var add = (Func<int, int, int>)((int x, int y) => x + y); // or a cast
        //   var add = (int x, int y) => x + y;                     // compile error: no type to infer
        //
        // C# 10 NATURAL TYPE
        // - With explicitly typed parameters, the compiler infers the delegate type:
        //     (int x, int y) => x + y   -> Func<int, int, int>
        //     (string name) => $"..."   -> Func<string, string>
        //     (int n) => n % 2 == 0     -> Func<int, bool>
        // - The variables are still strongly typed delegates and are called normally.
        public static void Run()
        {
            var add = (int x, int y) => x + y;
            var multiply = (int x, int y) => x * y;
            var greet = (string name) => $"Hello, {name}!";
            var isEven = (int n) => n % 2 == 0;

            Console.WriteLine(add(5, 3)); // 8
            Console.WriteLine(multiply(4, 7)); // 28
            Console.WriteLine(greet("Alice")); // Hello, Alice!
            Console.WriteLine(isEven(10)); // True
        }
    }

    /*

    Other C# 10 lambda additions:
        var parse = object (bool b) => b ? 1 : "one";       // explicit RETURN type before the parameters
        var check = [Obsolete] (int x) => x > 0;            // attributes on the lambda
        var f = ([FromQuery] string q) => q;                // attributes on parameters (Minimal APIs)
        Func<string, int> p = int.Parse;  var p2 = int.Parse; // method groups get a natural type too
                                                               // (if there's exactly one overload)

    //1. Parameters without types can't produce a natural type:  var f = x => x;   // error
    //2. C# 12 adds default parameter values for lambdas (see 12_DefaultLambdaParameters.cs).

     */
}
