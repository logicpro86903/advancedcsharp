// =============================================================================
//  Feature     : Object and Collection Initializers
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    OBJECT INITIALIZERS set properties as part of the 'new' expression:
//    new Customer { Name = "Bob", Age = 25 }. COLLECTION INITIALIZERS fill a
//    collection inline: new List<int> { 1, 2, 3 }. Both turn several statements
//    into a single expression, which is what makes LINQ projections like
//    'select new Customer { ... }' possible, and makes object graphs easy to read.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ObjectAndCollectionInitializers
    {
        public static void Run()
        {
            DemonstrateObjectInitializers();
            DemonstrateCollectionInitializers();
        }

        public class Address
        {
            public string City { get; set; } = "";
            public string Country { get; set; } = "";
        }

        public class Customer
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public Address HomeAddress { get; set; } = new Address();
            public List<string> Tags { get; } = new List<string>();

            public Customer() { }
            public Customer(string name) { Name = name; }
        }

        // OBJECT INITIALIZERS
        // - c1: the pre-C# 3 way - construct, then assign each property in separate statements.
        // - c2: '{ Name = ..., Age = ... }' sets properties right after the (parameterless)
        //   constructor runs, all in one expression.
        // - c3: combine with a constructor that takes arguments: new Customer("Carol") { Age = 41 }.
        // - c4: NESTED initializers - HomeAddress gets its own initializer, and
        //   'Tags = { ... }' (no 'new') ADDS items to the existing list returned by the
        //   get-only Tags property instead of replacing it.
        public static void DemonstrateObjectInitializers()
        {
            Customer c1 = new Customer();
            c1.Name = "Alice";
            c1.Age = 30;

            var c2 = new Customer { Name = "Bob", Age = 25 };

            var c3 = new Customer("Carol") { Age = 41 };

            var c4 = new Customer
            {
                Name = "Dave",
                Age = 35,
                HomeAddress = new Address { City = "Paris", Country = "France" },
                Tags = { "vip", "early-adopter" }
            };

            Console.WriteLine($"{c1.Name} {c1.Age} | {c2.Name} {c2.Age} | {c3.Name} {c3.Age}");
            Console.WriteLine($"{c4.Name} lives in {c4.HomeAddress.City}, tags: {string.Join(", ", c4.Tags)}");
        }

        // COLLECTION INITIALIZERS
        // - Work on any type that implements IEnumerable and has an Add method; the compiler
        //   calls Add once per element.
        // - Elements can themselves use object initializers (list of Customers).
        // - Dictionary: each '{ key, value }' pair calls Add(key, value).
        // - INDEX INITIALIZER (C# 6): '["key"] = value' calls the indexer setter instead of Add.
        public static void DemonstrateCollectionInitializers()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            var customers = new List<Customer>
            {
                new Customer { Name = "Eve", Age = 28 },
                new Customer { Name = "Frank", Age = 52 }
            };

            var capitals = new Dictionary<string, string>
            {
                { "France", "Paris" },
                { "Japan", "Tokyo" }
            };

            var populations = new Dictionary<string, int>
            {
                ["Paris"] = 2_100_000,
                ["Tokyo"] = 14_000_000
            };

            Console.WriteLine($"numbers: {numbers.Count}, customers: {customers.Count}");
            Console.WriteLine($"Capital of Japan: {capitals["Japan"]}, Tokyo pop: {populations["Tokyo"]}");
        }
    }

    /*

    //1. Object initializers run AFTER the constructor, so the ctor can't see those values.

    //2. Dictionary { {"a",1}, {"a",2} } throws (duplicate key, uses Add),
    //   but ["a"]=1, ["a"]=2 silently overwrites (uses the indexer).

    C# 12 collection expressions ( List<int> x = [1, 2, 3]; ) are the modern form - see 12_CollectionExpressions.cs.

     */
}
