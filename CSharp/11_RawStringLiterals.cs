using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 7 / C# 11.0 (2022)
    public class RawStringLiterals
    {
        public static void Run()
        {
            // Basic raw string literal using triple quotes
            var message = """
                This is a raw string literal.
                No need to escape "quotes" or \backslashes\.
                It spans multiple lines naturally.
                """;

            Console.WriteLine(message);

        }
    }

    /*
    Raw string literals provide a clean way to include text that contains quotes, 
    backslashes, and other special characters without requiring escape sequences. 
    They're particularly useful for JSON, XML, SQL, regular expressions, and multiline text.
     */
}
