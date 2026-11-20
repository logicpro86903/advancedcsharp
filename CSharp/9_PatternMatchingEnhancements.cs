// =============================================================================
//  Feature     : Pattern Matching Enhancements (relational and logical patterns)
//  C# Version  : C# 9.0
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    C# 9 added RELATIONAL patterns (<, <=, >, >= against a constant) and
//    LOGICAL pattern combinators ('and', 'or', 'not'), plus parenthesized
//    patterns and simple type patterns (just a type name). Ranges and
//    categories can now be expressed declaratively - 'x is >= 80 and < 90' -
//    both in switch expressions and in 'is' checks, replacing chains of
//    '&&'/'||' comparisons. 'is not null' became the idiomatic null check.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PatternMatchingEnhancements_13
    {
        public static void Run()
        {
            Console.WriteLine(ClassifyTemperature(-5)); // Freezing
            Console.WriteLine(ClassifyTemperature(25)); // Mild
            CheckScore(85); // Good job!
        }

        // RELATIONAL + 'and' PATTERNS IN A SWITCH EXPRESSION
        // - '< 0' is a relational pattern: matches when the value is less than 0.
        // - '>= 10 and < 20' combines two relational patterns: the value must satisfy BOTH.
        // - The arms together cover every int (from < 0 up to >= 40), so the compiler knows the
        //   switch is exhaustive and no '_' default arm is needed.
        public static string ClassifyTemperature(int temperature) => temperature switch
        {
            < 0 => "Freezing",
            >= 0 and < 10 => "Very Cold",
            >= 10 and < 20 => "Cold",
            >= 20 and < 30 => "Mild",
            >= 30 and < 40 => "Warm",
            >= 40 => "Hot"
        };

        // RELATIONAL PATTERNS WITH 'is'
        // - 'score is >= 90 and <= 100' tests a range without repeating the variable name
        //   (compare with 'score >= 90 && score <= 100').
        // - Patterns compare against CONSTANTS only; 'score is > max' with a variable won't compile.
        public static void CheckScore(int score)
        {
            if (score is >= 90 and <= 100)
                Console.WriteLine("Excellent!");
            else if (score is >= 80 and < 90)
                Console.WriteLine("Good job!");
            else if (score is >= 70 and < 80)
                Console.WriteLine("Average");
            else if (score is < 70 and > 0)
                Console.WriteLine("Needs improvement");
            else
                Console.WriteLine("Invalid score");
        }
    }

    /*

    The other C# 9 pattern additions:
        if (input is not null) ...                         // 'not' pattern - idiomatic null check
        bool isVowel = c is 'a' or 'e' or 'i' or 'o' or 'u'; // 'or' pattern
        bool isLetter = c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'); // parentheses
        string kind = shape switch { Circle => "round", _ => "other" };  // type pattern without a variable

    //1. 'and' binds tighter than 'or'; use parentheses when mixing them.

     */
}
