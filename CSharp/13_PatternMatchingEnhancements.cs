using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 9 / C# 13.0 (2024)
    public class PatternMatchingEnhancements
    {
        public static void Run()
        {
            // Usage examples
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

        // Enhanced range patterns with more flexible syntax
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

        // Range patterns with variables
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

        // Complex range conditions
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



}
