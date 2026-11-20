// =============================================================================
//  Feature     : Target-Typed new Expressions (new())
//  C# Version  : C# 9.0
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    'new()' without a type name creates an object of the TARGET type - the type
//    of the variable, field, property, parameter or return value it is assigned
//    to. 'var' (C# 3) only works for local variables, so fields and properties
//    still forced you to write long types twice:
//        private Dictionary<string, List<int>> _map = new Dictionary<string, List<int>>();
//    Target-typed new removes that repetition wherever the type is already known.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class TargetTypedNew
    {
        public static void Run()
        {
            DemonstrateBasics();
            DemonstrateFieldsAndProperties();
            DemonstrateArgumentsAndReturns();
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point() { }
            public Point(int x, int y) { X = x; Y = y; }
            public override string ToString() => $"({X}, {Y})";
        }

        // BASICS
        // - p1, p2 and p3 are equivalent: type written twice, type on the right (var), and
        //   type on the left only (target-typed new).
        // - Constructor arguments and object initializers work as usual: new(5, 6), new() { X = 7 }.
        // - Inside a collection initializer each new(...) is typed from the element type, so
        //   a List<Point> can be filled without repeating "Point" for every element.
        public static void DemonstrateBasics()
        {
            Point p1 = new Point(1, 2);
            var p2 = new Point(3, 4);
            Point p3 = new(5, 6);

            Point p4 = new() { X = 7, Y = 8 };

            Console.WriteLine($"{p1} {p2} {p3} {p4}");

            List<Point> path = new()
            {
                new(0, 0),
                new(1, 1),
                new() { X = 2, Y = 4 }
            };
            Console.WriteLine($"Path: {string.Join(" -> ", path)}");
        }

        // FIELDS AND PROPERTIES (where 'var' isn't allowed - the biggest win)
        // - Long generic field types are written once: Dictionary<string, List<decimal>> ... = new();
        // - Property initializers too: List<string> Symbols { get; } = new();
        // - Even an assignment into a dictionary slot: '_pricesBySymbol[symbol] = new();' creates
        //   a List<decimal> because that's the dictionary's value type.
        public class OrderBook
        {
            private readonly Dictionary<string, List<decimal>> _pricesBySymbol = new();
            private readonly object _sync = new();

            public List<string> Symbols { get; } = new();

            public void Add(string symbol, decimal price)
            {
                lock (_sync)
                {
                    if (!_pricesBySymbol.ContainsKey(symbol))
                    {
                        _pricesBySymbol[symbol] = new();
                        Symbols.Add(symbol);
                    }
                    _pricesBySymbol[symbol].Add(price);
                }
            }

            public int Count(string symbol) => _pricesBySymbol[symbol].Count;
        }

        public static void DemonstrateFieldsAndProperties()
        {
            OrderBook book = new();
            book.Add("MSFT", 410m);
            book.Add("MSFT", 412m);
            book.Add("AAPL", 225m);
            Console.WriteLine($"Symbols: {string.Join(", ", book.Symbols)}; MSFT prices: {book.Count("MSFT")}");
        }

        // METHOD ARGUMENTS AND RETURN VALUES
        // - DistanceFromOrigin(new(3, 4)): the PARAMETER type (Point) is the target.
        // - Origin() => new(): the RETURN type is the target.
        // - Works for any type with an accessible constructor, e.g. TimeSpan timeout = new(0, 0, 30).
        static double DistanceFromOrigin(Point p) => Math.Sqrt(p.X * p.X + p.Y * p.Y);

        static Point Origin() => new();

        public static void DemonstrateArgumentsAndReturns()
        {
            Console.WriteLine(DistanceFromOrigin(new(3, 4)));          // 5
            Console.WriteLine($"Origin: {Origin()}");                  // (0, 0)

            TimeSpan timeout = new(0, 0, 30);
            Console.WriteLine($"Timeout: {timeout.TotalSeconds}s");     // 30s
        }
    }

    /*

    Style guide most teams follow:
        var customer = new Customer();         // locals      -> var
        private Customer _customer = new();    // fields etc. -> target-typed new
    Pick one per situation - don't write  'var x = new();'  (compile error: no target type).

    //1. Readability: new() as a method argument can hide what's being created.
    //   Fine for obvious cases (new(3, 4) for a Point), less so for overloaded methods.

    //2. 'object o = new();' compiles and creates a plain 'object' - rarely what you want.

    Related C# 9 feature: target-typed conditional  ->  int? x = flag ? 1 : null;
    C# 12 collection expressions take this further for collections: List<int> nums = [1, 2, 3];

     */
}
