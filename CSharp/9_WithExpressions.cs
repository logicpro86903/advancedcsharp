// =============================================================================
//  Feature     : with Expressions (non-destructive mutation)
//  C# Version  : C# 9.0 (extended to structs and anonymous types in C# 10)
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    'original with { Prop = newValue }' creates a COPY of a record with the
//    listed properties changed, leaving the original untouched. It is the
//    natural way to "modify" immutable data: instead of mutating an object you
//    produce a new version of it. The compiler clones the record (via a
//    generated copy constructor) and then sets the listed init-only properties.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class WithExpressions
    {
        public record Person(string FirstName, string LastName, int Age, string Email);

        // NON-DESTRUCTIVE MUTATION
        // - Each 'with' produces a NEW Person: olderPerson changes Age, marriedPerson changes
        //   LastName, updatedContact changes two properties at once.
        // - Properties not listed are copied from the original as-is.
        // - The original record is never modified - its Age is still 30 at the end.
        public static void Run()
        {
            var original = new Person("John", "Doe", 30, "john@example.com");

            var olderPerson = original with { Age = 31 };
            var marriedPerson = original with { LastName = "Smith" };
            var updatedContact = original with { Email = "john.smith@newcompany.com", LastName = "Smith" };

            Console.WriteLine(original);      // Person { FirstName = John, LastName = Doe, Age = 30, Email = john@example.com }
            Console.WriteLine(olderPerson);   // Person { FirstName = John, LastName = Doe, Age = 31, Email = john@example.com }
            Console.WriteLine(marriedPerson); // Person { FirstName = John, LastName = Smith, Age = 30, Email = john@example.com }
            Console.WriteLine(updatedContact);// Person { FirstName = John, LastName = Smith, Age = 30, Email = john.smith@newcompany.com }

            Console.WriteLine($"Original age still: {original.Age}"); // 30
        }
    }

    /*

    //1. The copy is SHALLOW: reference-type properties (lists, objects) are shared between the
    //   original and the copy.
    //2. 'original with { }' (empty braces) is a quick way to clone a record.
    //3. C# 10 allows 'with' on any struct and on anonymous types (see 3_AnonymousTypes.cs).

     */
}
