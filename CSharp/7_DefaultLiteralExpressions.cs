using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class DefaultLiteralExpressions
    {
        public static void Run()
        {
            DemonstrateNewSyntax();
            ValueTypeDefaults();
            ReferenceTypeDefaults();

            Console.ReadKey();
        }


        public static void DemonstrateNewSyntax()
        {
            // Type is inferred from variable declaration
            int number = default;
            string text = default;
            DateTime date = default;
            List<int> list = default;

            Console.WriteLine($"Number: {number}");           // 0
            Console.WriteLine($"Text: {text ?? "null"}");     // null
            Console.WriteLine($"Date: {date}");               // 1/1/0001 12:00:00 AM
            Console.WriteLine($"List: {(object)list ?? "null"}");     // null
        }

        public static void ValueTypeDefaults()
        {
            // Value types get their default values
            bool flag = default;          // false
            char character = default;     // '\0'
            byte byteValue = default;     // 0
            short shortValue = default;   // 0
            long longValue = default;     // 0
            float floatValue = default;   // 0.0f
            double doubleValue = default; // 0.0
            decimal decimalValue = default; // 0.0m

            Console.WriteLine($"Boolean: {flag}");
            Console.WriteLine($"Char: '{character}' (null char)");
            Console.WriteLine($"Byte: {byteValue}");
            Console.WriteLine($"Short: {shortValue}");
            Console.WriteLine($"Long: {longValue}");
            Console.WriteLine($"Float: {floatValue}");
            Console.WriteLine($"Double: {doubleValue}");
            Console.WriteLine($"Decimal: {decimalValue}");
        }

        public static void ReferenceTypeDefaults()
        {
            // Reference types default to null
            string text = default;
            object obj = default;
            int[] array = default;
            List<string> list = default;
            Dictionary<string, int> dict = default;

            Console.WriteLine($"String: {text ?? "null"}");
            Console.WriteLine($"Object: {obj ?? "null"}");
            Console.WriteLine($"Array: {(object)array ?? "null"}");
            Console.WriteLine($"List: {(object)list ?? "null"}");
            Console.WriteLine($"Dictionary: {(object)dict ?? "null"}");
        }
    }
}
