using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class OutVariablesAnywhere
    {
        public static void Run()
        {
            TraditionalOutVariables();
            InlineOutVariables();
        }

        public static void TraditionalOutVariables()
        {
            string input = "123";

            // Had to declare variable first
            int result;
            if (int.TryParse(input, out result))
            {
                Console.WriteLine($"Parsed: {result}");
            }

            // Variable exists in outer scope even if parsing failed
            Console.WriteLine($"Result variable: {result}"); // Could be 0 if parsing failed
        }


        public static void InlineOutVariables()
        {
            string input = "123";

            // Declare variable inline
            if (int.TryParse(input, out int result))
            {
                Console.WriteLine($"Parsed: {result}");
            }

            // Variable is still accessible in the containing scope
            Console.WriteLine($"Result variable: {result}");
        }
    }
}
