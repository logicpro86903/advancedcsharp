// =============================================================================
//  Feature     : Implicitly Typed Local Variables (var)
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    'var' tells the compiler to infer a local variable's type from its
//    initializer. The variable is still STATICALLY and STRONGLY typed - the type
//    is fixed at compile time, exactly as if you had written it out. 'var'
//    removes repetition for long generic types and is REQUIRED for anonymous
//    types, whose type has no name you could write.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ImplicitlyTypedLocals
    {
        // TYPE INFERENCE WITH var
        // - The type comes from the right-hand side: 10 -> int, "Alice" -> string,
        //   9.99m -> decimal, DateTime.Now -> DateTime. GetType() confirms the real types.
        // - Biggest readability win: avoid repeating long generic types on both sides
        //   (compare 'verbose' and 'concise' below - they are the same type).
        // - Common in foreach loops, where the element type can be long (KeyValuePair<...>).
        // - Required for anonymous types: there is no type name to write instead of var.
        // - Once inferred, the type can't change: assigning a string to 'count' won't compile
        //   (var is NOT dynamic).
        public static void Run()
        {
            var count = 10;
            var name = "Alice";
            var price = 9.99m;
            var created = DateTime.Now;

            Console.WriteLine($"{count.GetType().Name}, {name.GetType().Name}, {price.GetType().Name}, {created.GetType().Name}");
            // Int32, String, Decimal, DateTime

            Dictionary<string, List<int>> verbose = new Dictionary<string, List<int>>();
            var concise = new Dictionary<string, List<int>>();
            concise["scores"] = new List<int> { 90, 85 };
            Console.WriteLine($"verbose: {verbose.Count}, concise: {concise.Count}");

            foreach (var pair in concise)
                Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}"); // scores: 90, 85

            var point = new { X = 1, Y = 2 };
            Console.WriteLine(point);             // { X = 1, Y = 2 }

            // count = "ten";                     // compile error: count is an int, not dynamic
        }
    }

    /*

    Rules:
    //1. Must be initialized:            var x;          // error
    //2. Can't be initialized to null:   var y = null;   // error (no type to infer)
    //3. Local variables only - not fields, parameters, or return types.

    var vs dynamic (C# 4, see 4_DynamicTyping.cs):
    - var:     type fixed at compile time, full IntelliSense and compile errors.
    - dynamic: type resolved at runtime, errors only at runtime.

    Style: use var when the type is obvious from the right side; spell it out when it isn't
    (var result = Process();  // what type is this?).
    C# 9 added the reverse: target-typed new  ->  Dictionary<string, int> d = new();

     */
}
