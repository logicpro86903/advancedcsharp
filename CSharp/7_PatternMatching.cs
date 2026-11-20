// =============================================================================
//  Feature     : Pattern Matching (is-type patterns, switch with patterns)
//  C# Version  : C# 7.0 (later extended in C# 8, 9, 10, 11)
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    Pattern matching tests a value against a "shape" and, if it matches,
//    extracts data from it in one step. C# 7.0 introduced TYPE patterns with a
//    variable declaration ('value is string text'), constant patterns, and
//    patterns + 'when' guards in switch statements. Later versions built on this:
//    switch EXPRESSIONS and PROPERTY patterns (C# 8), RELATIONAL (<, >=) and
//    LOGICAL (and, or, not) patterns (C# 9). This file mixes the C# 7 basics
//    with those later forms to show where the feature ended up.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PatternMatching
    {
        public static void Run()
        {
            ProcessValue("hello");     // String: HELLO
            ProcessValue(42);          // Number: 84
            ProcessValue(DateTime.Now); // Date: 2024-12-09

            Console.WriteLine(AnalyzeNumber(5));  // Single digit positive
            Console.WriteLine(AnalyzeNumber(-5)); // Negative

            var p = new Person { Name = "John", Age = 30, City = "New York", IsEmployed = true };
            Console.WriteLine(DescribePerson(p)); // Working New Yorker
        }

        // PROPERTY PATTERNS in a switch expression (C# 8) + relational patterns (C# 9)
        // - '{ Age: < 18 }' matches when the Age property is less than 18.
        // - Several properties in one pattern must ALL match: { Age: >= 65, IsEmployed: false }.
        // - Arms are tested TOP to BOTTOM; the first match wins, so order matters
        //   (John, 30, employed in New York matches "Working New Yorker" before "Adult John").
        // - '_' is the discard pattern - the catch-all default arm.
        public static string DescribePerson(Person person) => person switch
        {
            { Age: < 18 } => "Minor",
            { Age: >= 65, IsEmployed: false } => "Retired",
            { City: "New York", IsEmployed: true } => "Working New Yorker",
            { Name: "John", Age: > 21 } => "Adult John",
            { IsEmployed: false } => "Unemployed",
            _ => "Regular person"
        };

        // CONSTANT, RELATIONAL AND LOGICAL PATTERNS
        // - Constant patterns (C# 7): 0, 1, -1 match exact values.
        // - Relational patterns (C# 9): < 0, > 0 compare against constants.
        // - Logical patterns (C# 9): '> 0 and < 10' combines patterns (also 'or', 'not').
        public static string AnalyzeNumber(int number) => number switch
        {
            0 => "Zero",
            1 => "One",
            -1 => "Negative one",
            > 0 and < 10 => "Single digit positive",
            < 0 => "Negative",
            _ => "Large number"
        };

        // TYPE PATTERN WITH DECLARATION (the core C# 7.0 feature)
        // - 'value is string text' checks the runtime type AND, if it matches, casts the value
        //   into a new strongly-typed variable 'text' - in a single step.
        // - Replaces the old two-step 'if (value is string) { var text = (string)value; ... }'
        //   or 'as' + null check.
        // - The variable is only definitely assigned where the pattern is known to be true.
        public static void ProcessValue(object value)
        {
            if (value is string text)
            {
                Console.WriteLine($"String: {text.ToUpper()}");
            }
            else if (value is int number)
            {
                Console.WriteLine($"Number: {number * 2}");
            }
            else if (value is DateTime date)
            {
                Console.WriteLine($"Date: {date:yyyy-MM-dd}");
            }
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public bool IsEmployed { get; set; }
        }
    }

    /*

    C# 7 switch STATEMENT with patterns and 'when' guards:
        switch (shape)
        {
            case Circle c when c.Radius > 10: ...; break;
            case Circle c:                    ...; break;
            case null:                        ...; break;
            default:                          ...; break;
        }

    See also: 8_SwitchExpressions.cs, 9_PatternMatchingEnhancements.cs,
              10_ExtendedPropertyPatterns.cs, 11_ListPatterns.cs.

     */
}
