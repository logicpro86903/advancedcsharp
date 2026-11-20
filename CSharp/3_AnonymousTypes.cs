// =============================================================================
//  Feature     : Anonymous Types
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    'new { Name = "Alice", Age = 30 }' creates an object of a compiler-generated
//    class with READ-ONLY properties, without you declaring the class. The type
//    has no name you can write, so it is held in a 'var'. Anonymous types get
//    value-based Equals/GetHashCode and a readable ToString(). Their main purpose
//    is LINQ projections: selecting just the fields you need into a temporary shape.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp
{
    public class AnonymousTypes
    {
        public static void Run()
        {
            DemonstrateBasics();
            DemonstrateInLinq();
        }

        // ANONYMOUS TYPE BASICS
        // - The compiler generates a class with read-only properties Name and Age.
        //   ToString() prints "{ Name = Alice, Age = 30 }"; assigning a property won't compile.
        // - PROJECTION INITIALIZERS: property names can be inferred from a variable or member
        //   name: new { city, DateTime.Now.Year } creates properties 'city' and 'Year'.
        // - EQUALITY: Equals() compares property VALUES, but == still compares REFERENCES.
        // - Two anonymous objects with the same property names, types and order (in the same
        //   assembly) share ONE generated type.
        // - C# 10 added 'with' for anonymous types: copy the object and change some properties.
        public static void DemonstrateBasics()
        {
            var person = new { Name = "Alice", Age = 30 };
            Console.WriteLine($"{person.Name} is {person.Age}");    // Alice is 30
            Console.WriteLine(person);                              // { Name = Alice, Age = 30 }
            // person.Age = 31;                                     // compile error: read-only

            string city = "Berlin";
            var place = new { city, DateTime.Now.Year };
            Console.WriteLine($"{place.city} {place.Year}");

            var a = new { X = 1, Y = 2 };
            var b = new { X = 1, Y = 2 };
            Console.WriteLine($"a.Equals(b): {a.Equals(b)}");       // True
            Console.WriteLine($"a == b: {a == b}");                 // False

            Console.WriteLine($"Same type: {a.GetType() == b.GetType()}"); // True

            var moved = a with { X = 10 };
            Console.WriteLine(moved);                               // { X = 10, Y = 2 }
        }

        // ANONYMOUS TYPES IN LINQ
        // - An implicitly-typed array (new[] { ... }) of anonymous objects acts as sample data.
        // - Select(p => new { p.Name, PriceWithTax = ... }) PROJECTS each item into a new
        //   shape containing only what we need - no DTO class has to be declared.
        // - The result is IEnumerable<anonymous type>, consumed with 'var'.
        public static void DemonstrateInLinq()
        {
            var products = new[]
            {
                new { Name = "Laptop", Price = 1200m, Category = "Electronics" },
                new { Name = "Desk",   Price = 300m,  Category = "Furniture" },
                new { Name = "Phone",  Price = 800m,  Category = "Electronics" }
            };

            var summary = products
                .Where(p => p.Category == "Electronics")
                .Select(p => new { p.Name, PriceWithTax = p.Price * 1.2m });

            foreach (var item in summary)
                Console.WriteLine($"{item.Name}: {item.PriceWithTax}");
            // Laptop: 1440.0
            // Phone: 960.0
        }
    }

    /*

    //1. Anonymous types are local to a method - you can't return them with a proper type
    //   (only as object or dynamic). If it needs to leave the method, use a record or tuple.

    //2. Compared with tuples (C# 7, see 7_TuplesAndDeconstruction.cs):
    //   anonymous types = classes, immutable, nice ToString, work in expression trees (EF queries);
    //   tuples = structs, mutable, can be returned from methods.

     */
}
