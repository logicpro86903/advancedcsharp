// =============================================================================
//  Feature     : Pattern Matching in Practice (relational, logical, var and tuple patterns)
//  C# Version  : C# 9.0 patterns (C# 13.0 added no new pattern syntax)
//  .NET        : .NET 5+ (shown here on .NET 9)
//  Released    : 2020 (pattern features) / 2024 (C# 13)
// -----------------------------------------------------------------------------
//  Summary:
//    C# 13 itself introduced no new pattern-matching syntax; this file shows
//    how the patterns from C# 8-9 combine in real-world classification code:
//    RELATIONAL patterns (< 0, >= 18), LOGICAL combinators (and/or/not), VAR
//    patterns that capture the value, 'when' guards, and TUPLE patterns that
//    switch on several values at once. Together they replace long if/else
//    ladders with a declarative table of rules.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PatternMatchingEnhancements
    {
        public static void Run()
        {
            var ages = new[] { -1, 0, 3, 8, 15, 25, 70 };
            foreach (var age in ages)
            {
                Console.WriteLine($"Age {age}: {ClassifyAge(age)}");
            }

            var scores = new[] { -5, 45, 65, 75, 85, 95, 105 };
            foreach (var score in scores)
            {
                Console.WriteLine(AnalyzeScore(score));
            }

            Console.WriteLine(EvaluateTemperature(22.5, "C"));
            Console.WriteLine(EvaluateTemperature(75, "F"));
        }

        // RANGE CLASSIFICATION WITH RELATIONAL + 'and' PATTERNS
        // - Each arm describes a closed range: '>= 3 and <= 5'.
        // - The arms cover every int (< 0 ... >= 65), so no '_' default arm is required.
        static string ClassifyAge(int age) => age switch
        {
            < 0 => "Invalid age",
            >= 0 and <= 2 => "Infant",
            >= 3 and <= 5 => "Toddler",
            >= 6 and <= 12 => "Child",
            >= 13 and <= 17 => "Teenager",
            >= 18 and <= 64 => "Adult",
            >= 65 => "Senior"
        };

        // VAR PATTERNS + 'when' GUARDS
        // - 'var s' always matches and CAPTURES the value into 's' for use in the result.
        // - 'var s when s < 0' adds a boolean guard evaluated after the pattern matches.
        // - 'var s and >= 0 and < 60' combines capturing with relational checks in one pattern.
        // - The final 'var s' arm is the catch-all (everything above 100).
        static string AnalyzeScore(int score) => score switch
        {
            var s when s < 0 => $"Invalid score: {s}",
            var s and >= 0 and < 60 => $"Failing: {s}% - needs improvement",
            var s and >= 60 and < 70 => $"Passing: {s}% - satisfactory",
            var s and >= 70 and < 80 => $"Good: {s}% - above average",
            var s and >= 80 and < 90 => $"Very Good: {s}% - excellent work",
            var s and >= 90 and <= 100 => $"Outstanding: {s}% - exceptional!",
            var s => $"Invalid score range: {s}"
        };

        // TUPLE PATTERNS: SWITCHING ON TWO VALUES AT ONCE
        // - '(temp, unit.ToUpper()) switch' builds a tuple and matches both elements together.
        // - '(var t, "C")' : unit must be "C"; temperature captured as 't'.
        // - '(var t, "C") and ( < -40, _)' : a second tuple pattern adds a relational check on
        //   the first element; '_' ignores the second.
        // - 'when' handles conditions that patterns can't express as constants (absolute zero check).
        // - '(var t, var u)' catches any unknown unit.
        static string EvaluateTemperature(double temp, string unit) => (temp, unit.ToUpper()) switch
        {
            (var t, "C") when t < -273.15 => "Below absolute zero (impossible)",
            (var t, "C") and ( < -40, _) => $"Extremely cold: {t}°C",
            (var t, "C") and ( >= -40 and < 0, _) => $"Freezing: {t}°C",
            (var t, "C") and ( >= 0 and < 25, _) => $"Cool: {t}°C",
            (var t, "C") and ( >= 25 and < 35, _) => $"Warm: {t}°C",
            (var t, "C") and ( >= 35, _) => $"Hot: {t}°C",

            (var t, "F") when t < -459.67 => "Below absolute zero (impossible)",
            (var t, "F") and ( < 32, _) => $"Freezing: {t}°F",
            (var t, "F") and ( >= 32 and < 77, _) => $"Cool: {t}°F",
            (var t, "F") and ( >= 77 and < 95, _) => $"Warm: {t}°F",
            (var t, "F") and ( >= 95, _) => $"Hot: {t}°F",

            (var t, var u) => $"Unknown unit '{u}' with temperature {t}"
        };
    }

    /*

    See the pattern matching timeline: 7_PatternMatching.cs, 8_SwitchExpressions.cs,
    9_PatternMatchingEnhancements.cs, 10_ExtendedPropertyPatterns.cs, 11_ListPatterns.cs.

     */
}
