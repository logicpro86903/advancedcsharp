// =============================================================================
//  Feature     : Discards (_)
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    A discard, written as a single underscore '_', is a write-only placeholder
//    for a value you intentionally don't need. It makes intent explicit and
//    avoids declaring dummy variables. Discards work with out arguments
//    (out _), standalone assignments (_ = Method()), tuple deconstruction
//    (var (x, _) = point), pattern matching (case int _ / '_' in switch
//    expressions) and unused lambda parameters (C# 9).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CSharp
{
    public class Discards
    {
        public static void Run()
        {
            ParseWithoutUsingAllOutputs();
            DictionaryOperationsWithDiscards();
        }

        // DISCARDS WITH OUT ARGUMENTS AND RETURN VALUES
        // - 'out _' : we only care whether TryParse succeeded, not the parsed value.
        //   No variable is declared and nothing is stored.
        // - '_ = Method()' : explicitly ignore a RETURN value. This documents that ignoring it
        //   is deliberate and silences analyzers that warn about unused results.
        // - Math.DivRem returns the quotient and outputs the remainder; 'out _' drops the remainder.
        // - You can use '_' several times in the same scope; each is independent.
        public static void ParseWithoutUsingAllOutputs()
        {
            string input = "123";

            if (int.TryParse(input, out _))
            {
                Console.WriteLine("Input is a valid integer");
            }

            _ = int.TryParse("456", out int value);
            Console.WriteLine($"Parsed value: {value}");

            if (Math.DivRem(17, 5, out _) > 3)
            {
                Console.WriteLine("Quotient is greater than 3");
            }
        }

        // DISCARDS WITH DICTIONARY LOOKUPS
        // - TryGetValue(key, out _) checks for a key when the value itself isn't needed
        //   (ContainsKey also works, but TryGetValue + discard is common in Try-pattern APIs).
        // - Multiple 'out _' discards in a row don't conflict, unlike a reused named variable.
        public static void DictionaryOperationsWithDiscards()
        {
            var scores = new Dictionary<string, int>
            {
                ["Alice"] = 95,
                ["Bob"] = 87
            };

            if (scores.TryGetValue("Alice", out _))
            {
                Console.WriteLine("Alice has a score");
            }

            var userData = new Dictionary<string, object>();
            bool hasName = userData.TryGetValue("name", out _);
            bool hasAge = userData.TryGetValue("age", out _);

            Console.WriteLine($"Has name: {hasName}, Has age: {hasAge}");
        }
    }

    /*

    Other places discards appear:
        var (name, _, _) = GetPerson();                 // tuple deconstruction
        _ = Task.Run(() => DoWork());                   // fire-and-forget (intentionally not awaited)
        string s = shape switch { Circle => "c", _ => "other" };   // catch-all pattern
        button.Click += (_, _) => Save();               // unused lambda parameters (C# 9)

    //1. If a real variable named '_' is in scope, '_' refers to THAT variable, not a discard.

     */
}
