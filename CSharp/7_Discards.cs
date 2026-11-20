using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class Discards
    {
        public static void Run()
        {
            ParseWithoutUsingAllOutputs();
            DictionaryOperationsWithDiscards();
        }


        public static void ParseWithoutUsingAllOutputs()
        {
            string input = "123";

            // Only care about whether parsing succeeded, not the actual value
            if (int.TryParse(input, out _))
            {
                Console.WriteLine("Input is a valid integer");
            }

            // Only care about the parsed value, not success status
            _ = int.TryParse("456", out int value);
            Console.WriteLine($"Parsed value: {value}");

            // Don't need remainder in division
            if (Math.DivRem(17, 5, out _) > 3)
            {
                Console.WriteLine("Quotient is greater than 3");
            }
        }

        public static void DictionaryOperationsWithDiscards()
        {
            var scores = new Dictionary<string, int>
            {
                ["Alice"] = 95,
                ["Bob"] = 87
            };

            // Only check if key exists, don't care about the value
            if (scores.TryGetValue("Alice", out _))
            {
                Console.WriteLine("Alice has a score");
            }

            // Multiple discards
            var userData = new Dictionary<string, object>();
            bool hasName = userData.TryGetValue("name", out _);
            bool hasAge = userData.TryGetValue("age", out _);

            Console.WriteLine($"Has name: {hasName}, Has age: {hasAge}");
        }

    }


 /*   Discards are placeholder variables that are intentionally unused in application code.
  //They're represented by the underscore (_) and were introduced in C# 7.0. 
 //Discards help make your code more readable by explicitly indicating which values you don't need.
 */
}
