using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class ReadOnlyMembers
    {
        public static void Run()
        {
            Point2 p1 = new Point2(3, 4);
            Point2 p2 = new Point2(6, 8);
            p2.DistanceTo(p1);

            Console.WriteLine($"Point p1: {p2}");
        }
    }

    public struct Point2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Readonly method - guarantees it won't modify the struct
        public readonly double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        // Readonly method that takes parameters
        public readonly double DistanceTo(Point2 other)
        {
            // X = 10; // This line would cause a compile-time error

            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Readonly method with complex calculations
        public readonly double AngleTo(Point other)
        {
            double dx = other.X - X;
            double dy = other.Y - Y;
            return Math.Atan2(dy, dx) * 180.0 / Math.PI;
        }

        // Non-readonly method that modifies the struct
        public void MoveTo(double newX, double newY)
        {
            X = newX;
            Y = newY;
        }

        // Readonly property getter
        public readonly bool IsAtOrigin => X == 0 && Y == 0;

        // Readonly ToString override
        public readonly override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }

        // Readonly Equals override
        public readonly override bool Equals(object? obj)
        {
            return obj is Point other && X == other.X && Y == other.Y;
        }

        // Readonly GetHashCode override
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    /*
     //Readonly members in C# allow you to declare individual members of a struct as readonly, 
    providing fine-grained control over immutability. This feature was introduced in C# 8.0 
    and helps optimize performance while maintaining immutability guarantees.
     */
}
