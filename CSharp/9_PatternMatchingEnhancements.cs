using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 5 / C# 9.0 (2020)
    public class PatternMatchingEnhancements_13
    {
        public static void Run()
        {
            // Usage
            Console.WriteLine(ClassifyTemperature(-5)); // Freezing
            Console.WriteLine(ClassifyTemperature(25)); // Mild
            CheckScore(85); // Good job!
        }

        // Basic relational patterns with comparison operators
        public static string ClassifyTemperature(int temperature) => temperature switch
        {
            < 0 => "Freezing",
            >= 0 and < 10 => "Very Cold",
            >= 10 and < 20 => "Cold",
            >= 20 and < 30 => "Mild",
            >= 30 and < 40 => "Warm",
            >= 40 => "Hot"
        };

        // Using in if statements
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
     C# 9.0 introduced significant improvements to pattern matching, 
    making it more powerful and expressive with relational patterns, 
    logical patterns, and type patterns.
     */
}
