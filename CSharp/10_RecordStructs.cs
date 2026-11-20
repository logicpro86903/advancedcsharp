// =============================================================================
//  Feature     : Record Structs
//  C# Version  : C# 10.0
//  .NET        : .NET 6
//  Released    : 2021 (Visual Studio 2022)
// -----------------------------------------------------------------------------
//  Summary:
//    'record struct' brings the conveniences of records (C# 9) to VALUE types:
//    a positional declaration generates the constructor, properties, value-based
//    == / != / Equals / GetHashCode (faster than the default reflection-based
//    struct equality), a readable ToString(), Deconstruct, and 'with' support -
//    all without heap allocation. 'readonly record struct' makes it immutable.
//    (C# 10 also lets you write 'record class' to be explicit about the C# 9 kind.)
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class RecordStructs
    {
        // POSITIONAL RECORD STRUCT
        // - Generates properties X and Y, a constructor, equality members, ToString, Deconstruct.
        // - Unlike 'record class', the positional properties of a plain 'record struct' are
        //   MUTABLE ({ get; set; }). Use 'readonly record struct Point(int X, int Y);' to get
        //   init-only properties and full immutability.
        public record struct Point(int X, int Y);

        // VALUE EQUALITY AND ToString
        // - point1 == point2 is True because all fields match (== is generated - plain structs
        //   don't even have an == operator by default).
        // - ToString prints "Point { X = 10, Y = 20 }" instead of the type name.
        public static void Run()
        {
            var point1 = new Point(10, 20);
            var point2 = new Point(10, 20);

            Console.WriteLine(point1 == point2); // True
            Console.WriteLine(point1); // Point { X = 10, Y = 20 }
        }
    }
}


/*

    record class vs record struct:
        record (class)       reference type, heap allocated, immutable positional properties, supports inheritance
        record struct        value type, copied on assignment, MUTABLE positional properties, no inheritance
        readonly record struct  value type, immutable - usually the best choice for small value-like data

    //1. Large record structs get copied on every assignment/call - keep them small (a few fields).

 */
