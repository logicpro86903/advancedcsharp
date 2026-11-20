// =============================================================================
//  Feature     : readonly Members (on structs)
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    Individual members of a (mutable) struct - methods, properties, indexers -
//    can be marked 'readonly' to promise they don't modify the struct's state.
//    The compiler enforces the promise, and can then skip hidden defensive copies
//    when the struct is accessed through a readonly field or an 'in' parameter.
//    It gives fine-grained immutability when the whole struct can't be a
//    'readonly struct' (C# 7.2) because some members do need to mutate it.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ReadOnlyMembers
    {
        // USING THE STRUCT: DistanceTo is readonly, so calling it never changes p2.
        public static void Run()
        {
            Point2 p1 = new Point2(3, 4);
            Point2 p2 = new Point2(6, 8);
            p2.DistanceTo(p1);

            Console.WriteLine($"Point p1: {p2}");
        }
    }

    // A MUTABLE STRUCT WITH SOME READONLY MEMBERS
    // - X and Y have setters, so the struct as a whole can't be a 'readonly struct'.
    // - Members marked 'readonly' may READ state but not WRITE it: an assignment like X = 10
    //   inside DistanceTo is a compile error.
    // - A readonly member may only call other readonly members (otherwise the compiler would
    //   have to make a defensive copy, and it warns about it).
    // - MoveTo is deliberately NOT readonly because it changes X and Y.
    // - Readonly applies to property getters too (IsAtOrigin) and to overrides of
    //   ToString/Equals/GetHashCode - these should never mutate state.
    public struct Point2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public readonly double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public readonly double DistanceTo(Point2 other)
        {
            // X = 10; // compile error: cannot modify a member inside a readonly method

            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public readonly double AngleTo(Point other)
        {
            double dx = other.X - X;
            double dy = other.Y - Y;
            return Math.Atan2(dy, dx) * 180.0 / Math.PI;
        }

        public void MoveTo(double newX, double newY)
        {
            X = newX;
            Y = newY;
        }

        public readonly bool IsAtOrigin => X == 0 && Y == 0;

        public readonly override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Point other && X == other.X && Y == other.Y;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    /*

    //1. Auto-property getters are implicitly readonly; you don't need to mark them.
    //2. If every member can be readonly, declare the whole type 'readonly struct' instead
    //   (see 7_ReadOnlyStructsAndInParams.cs).

     */
}
