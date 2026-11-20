using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 8 / C# 12.0 (2023)
    public class PrimaryConstructorsForStructs
    {
        public static void Run()
        {
            // Simple usage
            var point1 = new Point(10, 20);
            var point2 = new Point(5, 5);

            Console.WriteLine($"Point 1: {point1}");
            Console.WriteLine($"Distance from origin: {point1.DistanceFromOrigin():F2}");
            Console.WriteLine($"Translated: {point1.Translate(5, -3)}");
        }
    }


    // Primary constructor for struct
    public struct Point(int x, int y)
    {
        // Parameters automatically become fields
        public int X { get; } = x;
        public int Y { get; } = y;

        // Additional methods
        public double DistanceFromOrigin() => Math.Sqrt(X * X + Y * Y);
        public Point Translate(int dx, int dy) => new(X + dx, Y + dy);

        public override string ToString() => $"({X}, {Y})";
    }


    // C# 12: Primary constructor
    public class Person(string name, int age)
    {
        public string Name { get; } = name;
        public int Age { get; } = age;

        public void Display() => Console.WriteLine($"{name} is {age}");
    }


    /*
      Primary constructors for structs provide a concise way to define constructor parameters 
        directly in the struct declaration, automatically creating properties 
        or fields and reducing boilerplate code.
     */
}
