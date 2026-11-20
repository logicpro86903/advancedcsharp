// =============================================================================
//  Feature     : Tuples (ValueTuple) and Deconstruction
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    C# 7 tuples group several values into one lightweight value without
//    declaring a class: (string Name, int Age). They are backed by the
//    System.ValueTuple STRUCT and support NAMED elements, which makes returning
//    multiple values from a method clean (no out parameters, no Tuple<T1,T2>
//    with Item1/Item2). DECONSTRUCTION unpacks a tuple (or any type with a
//    Deconstruct method) into separate variables: var (sum, product) = Calc().
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class TuplesAndDeconstruction
    {
        public static void Run()
        {
            DemonstrateTuples();
            DemonstrateTypes();
            UseCalculateMethod();
        }

        // TUPLES AS MEMBER TYPES: a tuple type can be used anywhere a type is expected -
        // fields, properties and array element types.
        private (string Name, int Age) _personInfo;

        public (double Latitude, double Longitude) Location { get; set; }

        private (string Name, decimal Price)[] _products;

        // TUPLES IN LOCALS AND COLLECTIONS
        // - Declare a local with an explicit tuple type: (string firstName, string lastName) name.
        // - Arrays and generic collections of tuples are a quick way to hold small records
        //   (List<(string Name, int Age, string City)>) without defining a class.
        // - Elements are accessed by their names: point.X, person.City.
        public static void DemonstrateTypes()
        {
            (string firstName, string lastName) name = ("John", "Doe");
            (int x, int y, int z) position = (10, 20, 30);

            var points = new (int X, int Y)[]
            {
                (0, 0),
                (10, 15),
                (20, 25)
            };

            foreach (var point in points)
            {
                Console.WriteLine($"Point: ({point.X}, {point.Y})");
            }

            var people = new List<(string Name, int Age, string City)>
            {
                ("Alice", 25, "New York"),
                ("Bob", 30, "Los Angeles"),
                ("Charlie", 35, "Chicago")
            };

            foreach (var person in people)
            {
                Console.WriteLine($"{person.Name} is {person.Age} years old and lives in {person.City}");
            }
        }

        // CREATING TUPLE LITERALS
        // - '(X: 10, Y: 20)' creates a tuple with NAMED elements.
        // - Without names, elements are accessed as Item1, Item2, ... (still always available).
        // - Names can be mixed: unnamed positions fall back to ItemN (data.Item2).
        // - Tuples are mutable value types and have value equality (== compares elements, C# 7.3).
        public static void DemonstrateTuples()
        {
            var point = (X: 10, Y: 20);
            var person = (Name: "John", Age: 30, IsActive: true);

            Console.WriteLine($"Point: ({point.X}, {point.Y})");
            Console.WriteLine($"Person: {person.Name}, Age: {person.Age}, Active: {person.IsActive}");

            var coordinates = (10, 20);
            Console.WriteLine($"X: {coordinates.Item1}, Y: {coordinates.Item2}");

            var data = (Count: 5, 42, Name: "Test");
            Console.WriteLine($"Count: {data.Count}, Value: {data.Item2}, Name: {data.Name}");
        }

        // RETURNING MULTIPLE VALUES
        // - The return type '(int Sum, int Product, int Diff)' names each element, so callers get
        //   readable result.Sum / result.Product instead of out parameters.
        // - Tuples can also be method PARAMETERS: DisplayPoint((15.5, 25.3)).
        private static (int Sum, int Product, int Diff) Calculate(int a, int b)
        {
            return (Sum: a + b, Product: a * b, a - b);
        }

        private static void DisplayPoint((double X, double Y) point)
        {
            Console.WriteLine($"Point coordinates: ({point.X}, {point.Y})");
        }

        // CONSUMING TUPLE RESULTS
        // - Access by name: result.Sum.
        // - Alternatively the result can be DECONSTRUCTED into separate variables
        //   (see the deconstruction forms in the notes at the bottom of this file).
        public static void UseCalculateMethod()
        {
            var result = Calculate(5, 3);
            Console.WriteLine($"Sum: {result.Sum}, Product: {result.Product}, Difference: {result.Diff}");

            DisplayPoint((15.5, 25.3));
        }
    }

    /*

    Deconstruction forms:
        var (sum, product, diff) = Calculate(5, 3);   // new variables, types inferred
        (int s, var p, _)        = Calculate(5, 3);   // mix explicit types and discards
        (a, b) = (b, a);                              // swap two variables without a temp

    Any type can be deconstructed by adding a Deconstruct method:
        public void Deconstruct(out string name, out int age) { name = Name; age = Age; }
        var (n, a) = person;

    //1. Tuple element NAMES are compile-time only; at runtime they're Item1, Item2...
    //   (so names don't survive reflection or 'dynamic').
    //2. For data that travels through your app, a record (C# 9) is usually clearer than a tuple.

     */
}
