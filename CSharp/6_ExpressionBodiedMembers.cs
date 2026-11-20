// =============================================================================
//  Feature     : Expression-Bodied Members
//  C# Version  : C# 6.0 (extended in C# 7.0)
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    Members whose body is a single expression can be written with the lambda
//    arrow '=>' instead of braces and 'return':
//        public int Add(int x, int y) => x + y;
//    C# 6 allowed this for methods, operators and read-only properties/indexers.
//    C# 7.0 extended it to constructors, finalizers, and get/set accessors.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ExpressionBodiedMembers
    {
        public static void Run()
        {
            var calc = new Calculator();
            Console.WriteLine(calc.Add(5, 3));      // 8
            Console.WriteLine(calc.GetGrade(85));   // B
        }
    }

    public class Calculator
    {
        // TRADITIONAL BLOCK BODY: braces + explicit 'return' statement.
        public int AddTraditional(int x, int y)
        {
            return x + y;
        }

        // EXPRESSION-BODIED METHODS
        // - '=> expression' replaces '{ return expression; }'; the expression's value is returned.
        // - For void methods the expression is simply executed: void Log() => Console.WriteLine("x");
        // - Any single expression works: arithmetic, string interpolation, method calls, or a
        //   (nested) conditional ?: expression as in GetGrade.
        // - If you need several statements, use a normal block body instead.
        public int Add(int x, int y) => x + y;

        public double CalculateArea(double radius) => Math.PI * radius * radius;

        public string FormatName(string firstName, string lastName) => $"{firstName} {lastName}".Trim();

        public string GetGrade(int score) => score >= 90 ? "A" : score >= 80 ? "B" : "C";
    }
}


/*

    Other members that can be expression-bodied:
        public string FullName => $"{First} {Last}";            // read-only property (C# 6)
        public Point(int x) => X = x;                            // constructor        (C# 7)
        public string Name { get => _name; set => _name = value; } // accessors       (C# 7)

    //1. 'public int X => 5;' (computed each read) is NOT the same as 'public int X { get; } = 5;'
    //   (initialized once and stored).

 */
