// =============================================================================
//  Feature     : Primary Constructors for Structs
//  C# Version  : C# 12.0
//  .NET        : .NET 8
//  Released    : 2023 (Visual Studio 2022 v17.8)
// -----------------------------------------------------------------------------
//  Summary:
//    Like classes, structs (and non-record types in general) can declare
//    constructor parameters directly after the type name: struct Point(int x, int y).
//    The parameters are in scope throughout the type body and are typically used
//    to initialize properties. Unlike 'record struct', NOTHING is generated
//    automatically - no public properties, equality or ToString - you decide what
//    to expose. (Class version with more detail: 12_PrimaryConstructorsForClasses.cs.)
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PrimaryConstructorsForStructs
    {
        // USING THE STRUCT
        // - 'new Point(10, 20)' calls the primary constructor.
        // - Translate returns a NEW Point (the struct's properties are get-only).
        public static void Run()
        {
            var point1 = new Point(10, 20);
            var point2 = new Point(5, 5);

            Console.WriteLine($"Point 1: {point1}");
            Console.WriteLine($"Distance from origin: {point1.DistanceFromOrigin():F2}");
            Console.WriteLine($"Translated: {point1.Translate(5, -3)}");
        }
    }


    // STRUCT WITH A PRIMARY CONSTRUCTOR
    // - 'x' and 'y' are parameters, NOT fields or properties. Here they are copied into the
    //   get-only properties X and Y via initializers, and members then use X/Y.
    // - Structs always also have the parameterless constructor 'new Point()', which skips the
    //   primary constructor and leaves X and Y at 0.
    public struct Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public double DistanceFromOrigin() => Math.Sqrt(X * X + Y * Y);
        public Point Translate(int dx, int dy) => new(X + dx, Y + dy);

        public override string ToString() => $"({X}, {Y})";
    }


    // CLASS WITH A PRIMARY CONSTRUCTOR (for comparison)
    // - Name/Age are initialized from the parameters, AND Display() uses the raw parameters
    //   'name'/'age' directly. That stores each value TWICE (property + captured parameter), so
    //   the compiler warns (CS9124). Prefer using Name/Age inside Display() instead.
    public class Person(string name, int age)
    {
        public string Name { get; } = name;
        public int Age { get; } = age;

        public void Display() => Console.WriteLine($"{name} is {age}");
    }


    /*

    Choosing the right form:
        struct Point(int x, int y)                  // you control members; nothing generated
        record struct Point(int X, int Y)           // properties, equality, ToString generated (mutable)
        readonly record struct Point(int X, int Y)  // same, but immutable - often the best choice

    //1. Inside a struct, a primary constructor parameter that's captured by members becomes
    //   part of the struct's state - and is copied with it on every assignment.

     */
}
