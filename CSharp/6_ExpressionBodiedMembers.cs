using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class ExpressionBodiedMembers
    {
        public static void Run()
        {
            // Usage
            var calc = new Calculator();
            Console.WriteLine(calc.Add(5, 3)); // Output: 8
            Console.WriteLine(calc.GetGrade(85)); // Output: B
        }
    }

    public class Calculator
    {
        // Traditional method syntax
        public int AddTraditional(int x, int y)
        {
            return x + y;
        }

        // Expression-bodied method
        public int Add(int x, int y) => x + y;

        // More complex expression
        public double CalculateArea(double radius) => Math.PI * radius * radius;

        // String manipulation
        public string FormatName(string firstName, string lastName) => $"{firstName} {lastName}".Trim();

        // Conditional expression
        public string GetGrade(int score) => score >= 90 ? "A" : score >= 80 ? "B" : "C";
    }
}


/*
 // Expression-bodied members allow you to define class members using a more concise lambda-like syntax 
(=>). They were introduced in C# 6.0 for methods and properties, and expanded in C# 7.0 to include 
constructors, destructors, and accessors.
 */
