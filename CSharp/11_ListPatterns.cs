using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 7 / C# 11.0 (2022)
    public class ListPatterns
    {
        public static void Run()
        {
            // Basic list pattern matching
            static string AnalyzeArray(int[] numbers) => numbers switch
            {
                [] => "Empty array",
                [var single] => $"Single element: {single}",
                [var first, var second] => $"Two elements: {first}, {second}",
                [var first, .., var last] => $"Multiple elements: first={first}, last={last}",
                _ => "Unknown pattern"
            };

            // Usage
            Console.WriteLine(AnalyzeArray(new int[] { })); // Empty array
            Console.WriteLine(AnalyzeArray(new int[] { 5 })); // Single element: 5
            Console.WriteLine(AnalyzeArray(new int[] { 1, 2 })); // Two elements: 1, 2
            Console.WriteLine(AnalyzeArray(new int[] { 1, 2, 3, 4, 5 })); // Multiple elements: first=1, last=5

        }
    }

    /*
     List patterns enable powerful pattern matching on arrays, lists, and other collection types, 
    allowing you to match against specific sequences, elements at positions, 
    and ranges within collections.
     */
}
