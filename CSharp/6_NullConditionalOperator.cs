// =============================================================================
//  Feature     : Null-Conditional Operators (?. and ?[])
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    'a?.B' accesses member B only if 'a' is not null; if 'a' IS null, the whole
//    expression evaluates to null instead of throwing NullReferenceException.
//    '?[]' does the same for indexers. Chains short-circuit at the first null
//    (order?.Customer?.Address?.City) and combine naturally with '??' to supply
//    a fallback. It also made raising events thread-safe in one line:
//    Changed?.Invoke(this, e).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class NullConditionalOperator
    {
        // NULL-CONDITIONAL MEMBER ACCESS
        // - Old way: an explicit 'if (person != null)' guard around every member access.
        // - New way: person?.Name returns null when person is null, otherwise person.Name.
        //   Here person is null, so nothing is dereferenced and WriteLine prints an empty line.
        // - When the member is a value type (e.g. int Age), the result becomes nullable (int?).
        public static void Run()
        {
            Person3 person = null;

            if (person != null)
            {
                Console.WriteLine(person.Name);
            }

            Console.WriteLine(person?.Name);
        }
    }

    public class Person3
    {
        public string Name { get; set; }
    }
}

/*

    More forms:
        int? length   = person?.Name?.Length;          // chain: null if ANY link is null
        string city   = order?.Customer?.City ?? "N/A"; // combine with ?? for a default
        var first     = list?[0];                       // null-conditional indexer
        PropertyChanged?.Invoke(this, args);            // safe event raising

    C# 14 allows ?. and ?[] on the LEFT side of an assignment (see 14_NullConditionalAssignment.cs).

 */
