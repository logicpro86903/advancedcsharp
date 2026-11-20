// =============================================================================
//  Feature     : Extended Property Patterns
//  C# Version  : C# 10.0
//  .NET        : .NET 6
//  Released    : 2021 (Visual Studio 2022)
// -----------------------------------------------------------------------------
//  Summary:
//    Property patterns can reach into NESTED properties with a dotted path:
//        { Address.City: "Paris" }
//    instead of nesting another pattern:
//        { Address: { City: "Paris" } }
//    Real objects are nested (Order -> Customer -> Address) and the old syntax
//    quickly became deeply indented. Same meaning, far less noise. Like all
//    property patterns, it checks for null at every step automatically.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ExtendedPropertyPatterns
    {
        public static void Run()
        {
            DemonstrateBeforeAndAfter();
            DemonstrateInSwitchExpressions();
            DemonstrateCombiningPatterns();
        }

        // Sample domain: an Order has a Customer, who has an Address.
        public class Address
        {
            public string City { get; set; } = "";
            public string Country { get; set; } = "";
        }

        public class Customer
        {
            public string Name { get; set; } = "";
            public bool IsVip { get; set; }
            public Address Address { get; set; } = new();
        }

        public class Order
        {
            public int Id { get; set; }
            public decimal Total { get; set; }
            public Customer Customer { get; set; } = new();
            public List<string> Items { get; set; } = new();
        }

        static List<Order> GetOrders() => new()
        {
            new() { Id = 1, Total = 250m,  Customer = new() { Name = "Alice", IsVip = true,  Address = new() { City = "Paris",  Country = "France" } }, Items = new() { "Laptop" } },
            new() { Id = 2, Total = 40m,   Customer = new() { Name = "Bob",   IsVip = false, Address = new() { City = "Lyon",   Country = "France" } }, Items = new() { "Mouse", "Pad" } },
            new() { Id = 3, Total = 1200m, Customer = new() { Name = "Carol", IsVip = false, Address = new() { City = "Berlin", Country = "Germany" } }, Items = new() { "TV" } },
            new() { Id = 4, Total = 15m,   Customer = new() { Name = "Dave",  IsVip = true,  Address = new() { City = "Munich", Country = "Germany" } }, Items = new() }
        };

        // SAME CHECK, THREE GENERATIONS OF C#
        // - C# 1-7 : boolean logic with explicit null checks at every level.
        // - C# 8   : nested property patterns { Customer: { Address: { City: "Paris" } } }.
        // - C# 10  : extended property pattern { Customer.Address.City: "Paris" }.
        // - All three give the same result. Patterns also handle null safely: matching a null
        //   order simply returns false instead of throwing NullReferenceException.
        public static void DemonstrateBeforeAndAfter()
        {
            var order = GetOrders()[0];

            bool classic = order.Customer != null && order.Customer.Address != null && order.Customer.Address.City == "Paris";

            bool nested = order is { Customer: { Address: { City: "Paris" } } };

            bool extended = order is { Customer.Address.City: "Paris" };

            Console.WriteLine($"classic={classic}, nested={nested}, extended={extended}"); // all True

            Order? missing = null;
            Console.WriteLine($"null order matches? {missing is { Customer.Name: "Alice" }}"); // False
        }

        // BUSINESS RULES IN A SWITCH EXPRESSION
        // - Each arm reads like a rule: "VIP customers ship free", "France with total >= 100
        //   ships free", etc.
        // - Dotted paths can be mixed with other properties of the same object (Total: >= 100m)
        //   and with 'or' patterns ("Germany" or "Belgium").
        // - Arms are evaluated top to bottom, so the most specific rules come first.
        static decimal ShippingCost(Order order) => order switch
        {
            { Customer.IsVip: true }                                   => 0m,
            { Customer.Address.Country: "France", Total: >= 100m }     => 0m,
            { Customer.Address.Country: "France" }                     => 4.99m,
            { Customer.Address.Country: "Germany" or "Belgium" }       => 9.99m,
            _                                                          => 19.99m
        };

        public static void DemonstrateInSwitchExpressions()
        {
            foreach (var order in GetOrders())
                Console.WriteLine($"Order {order.Id} ({order.Customer.Name}, {order.Customer.Address.City}): shipping {ShippingCost(order)}");
            // Order 1 (Alice, Paris): shipping 0
            // Order 2 (Bob, Lyon): shipping 4.99
            // Order 3 (Carol, Berlin): shipping 9.99
            // Order 4 (Dave, Munich): shipping 0
        }

        // COMBINING WITH OTHER PATTERNS
        // - { Items.Count: 0 }                 : dotted path into a collection's property.
        // - { Items: [var only], Total: ... }  : LIST pattern (C# 11) - exactly one item, captured as 'only'.
        // - { Customer.Address.City: var city }: 'var' pattern CAPTURES the nested value into a variable.
        // - { Customer.Name.Length: > 3 }      : relational pattern at the end of a path.
        // - In an 'is' expression you can mix a dotted path with a nested pattern block:
        //   { Customer.Address: { City: ..., Country: ... }, Total: > 100m and < 500m }.
        public static void DemonstrateCombiningPatterns()
        {
            foreach (var order in GetOrders())
            {
                string label = order switch
                {
                    { Items.Count: 0 }                                     => "empty order",
                    { Items: [var only], Total: > 1000m }                  => $"single big-ticket item: {only}",
                    { Customer.Address.City: var city, Customer.IsVip: true } => $"VIP order from {city}",
                    { Customer.Name.Length: > 3 }                          => "long customer name",
                    _                                                      => "regular"
                };
                Console.WriteLine($"Order {order.Id}: {label}");
            }

            var first = GetOrders()[0];
            if (first is { Customer.Address: { City: "Paris", Country: "France" }, Total: > 100m and < 500m })
                Console.WriteLine("Mid-sized Parisian order");
        }
    }

    /*

    Pattern matching timeline in this project:
        C# 7   type patterns, 'is' with declaration       7_PatternMatching.cs
        C# 8   switch expressions, property patterns      8_SwitchExpressions.cs
        C# 9   relational (<, >=) and logical (and/or/not) 9_PatternMatchingEnhancements.cs
        C# 10  extended property patterns  { A.B.C: x }   this file
        C# 11  list patterns [first, .., last]            11_ListPatterns.cs

    //1. Order matters in switch expressions - the first matching arm wins, so put specific cases first.
    //   The compiler errors if an arm can never be reached.

    //2. A property pattern { } matches any NON-NULL value - 'x is { }' is an idiomatic null check.

     */
}
