// =============================================================================
//  Feature     : List Patterns
//  C# Version  : C# 11.0
//  .NET        : .NET 7
//  Released    : 2022 (Visual Studio 2022 v17.4)
// -----------------------------------------------------------------------------
//  Summary:
//    List patterns match a sequence by its SHAPE using square brackets:
//    [] (empty), [var x] (exactly one), [1, 2, _] (three, first two fixed),
//    [var first, .., var last] (at least two). The SLICE pattern '..' matches
//    zero or more elements and can capture them ([.., var tail] or [_, .. var rest]).
//    They work on arrays, List<T>, spans and any type with an indexer plus a
//    Length/Count property, and combine with all other patterns.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ListPatterns
    {
        public static void Run()
        {
            // LIST PATTERNS IN A SWITCH EXPRESSION
            // - []                        : matches an EMPTY array.
            // - [var single]              : exactly ONE element, captured into 'single'.
            // - [var first, var second]   : exactly TWO elements.
            // - [var first, .., var last] : TWO OR MORE elements; '..' (slice pattern) skips any
            //                               number of elements in the middle, so this catches the rest.
            // - Element patterns can be anything: constants ([1, ..]), relational ([> 0, ..]),
            //   discards ([_, _]) or type patterns.
            // - '_' covers inputs no list pattern matches (e.g. a null array).
            static string AnalyzeArray(int[] numbers) => numbers switch
            {
                [] => "Empty array",
                [var single] => $"Single element: {single}",
                [var first, var second] => $"Two elements: {first}, {second}",
                [var first, .., var last] => $"Multiple elements: first={first}, last={last}",
                _ => "Unknown pattern"
            };

            Console.WriteLine(AnalyzeArray(new int[] { })); // Empty array
            Console.WriteLine(AnalyzeArray(new int[] { 5 })); // Single element: 5
            Console.WriteLine(AnalyzeArray(new int[] { 1, 2 })); // Two elements: 1, 2
            Console.WriteLine(AnalyzeArray(new int[] { 1, 2, 3, 4, 5 })); // Multiple elements: first=1, last=5
        }
    }

    /*

    More examples:
        if (args is ["--help" or "-h", ..]) ShowHelp();          // first argument check
        if (line.Split(',') is [var name, var age, ..]) ...       // destructure CSV fields
        if (bytes is [0x89, 0x50, 0x4E, 0x47, ..]) ...            // PNG file signature
        if (numbers is [_, .. var middle, _]) ...                 // capture a slice (arrays/spans/strings)

    //1. Only ONE slice '..' is allowed per list pattern.
    //2. Capturing a slice ('.. var rest') requires the type to support ranges (arrays, spans, strings).

     */
}
