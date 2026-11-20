using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class TuplesAndDeconstruction
    {
        public static void Run()
        {
            DemonstrateTuples();
            DemonstrateTypes();
            UseCalculateMethod();
        }

        // As field
        private (string Name, int Age) _personInfo;

        // As property
        public (double Latitude, double Longitude) Location { get; set; }

        // As array element type
        private (string Name, decimal Price)[] _products;

        public static void DemonstrateTypes()
        {
            // Local tuple variables
            (string firstName, string lastName) name = ("John", "Doe");
            (int x, int y, int z) position = (10, 20, 30);

            // Tuple arrays
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

            // Tuple collections
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

        public static void DemonstrateTuples()
        {
            // Creating tuples
            var point = (X: 10, Y: 20);
            var person = (Name: "John", Age: 30, IsActive: true);

            // Accessing tuple elements
            Console.WriteLine($"Point: ({point.X}, {point.Y})");
            Console.WriteLine($"Person: {person.Name}, Age: {person.Age}, Active: {person.IsActive}");

            // Without named elements (uses Item1, Item2, etc.)
            var coordinates = (10, 20);
            Console.WriteLine($"X: {coordinates.Item1}, Y: {coordinates.Item2}");

            // Mixed named and unnamed
            var data = (Count: 5, 42, Name: "Test");
            Console.WriteLine($"Count: {data.Count}, Value: {data.Item2}, Name: {data.Name}");
        }

        // Method returning a tuple
        private static (int Sum, int Product, int Diff) Calculate(int a, int b)
        {
            return (Sum: a + b, Product: a * b, a - b);
        }

        // Method with tuple parameter
        private static void DisplayPoint((double X, double Y) point)
        {
            Console.WriteLine($"Point coordinates: ({point.X}, {point.Y})");
        }

        public static void UseCalculateMethod()
        {
            var result = Calculate(5, 3);
            Console.WriteLine($"Sum: {result.Sum}, Product: {result.Product}, Difference: {result.Diff}");

            // Using the tuple directly
            DisplayPoint((15.5, 25.3));
        }
    }

    /*
     //Tuples allow you to group multiple values together without creating a custom class, 
     //while deconstruction lets you break down complex objects into their constituent parts. 
     //These features were significantly enhanced in C# 7.0 with value tuples and pattern matching.
    */
}
