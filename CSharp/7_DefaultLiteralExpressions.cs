// =============================================================================
//  Feature     : default Literal Expressions
//  C# Version  : C# 7.1
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017 v15.3)
// -----------------------------------------------------------------------------
//  Summary:
//    The 'default' operator returns the default value of a type: 0 for numbers,
//    false for bool, '\0' for char, all-zero fields for structs, and null for
//    reference types. Before C# 7.1 you had to repeat the type: default(int).
//    The 'default' LITERAL lets the compiler infer the type from context
//    (variable declaration, parameter, return type), so you just write 'default'.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class DefaultLiteralExpressions
    {
        public static void Run()
        {
            DemonstrateNewSyntax();
            ValueTypeDefaults();
            ReferenceTypeDefaults();

            Console.ReadKey();
        }

        // THE DEFAULT LITERAL
        // - 'int number = default;' is shorthand for 'int number = default(int);'.
        //   The compiler takes the type from the declaration on the left.
        // - It also works anywhere the type is known: optional parameters
        //   (CancellationToken token = default), return statements, comparisons, etc.
        // - DateTime is a struct, so its default is 01/01/0001 00:00:00 (all fields zero).
        public static void DemonstrateNewSyntax()
        {
            int number = default;
            string text = default;
            DateTime date = default;
            List<int> list = default;

            Console.WriteLine($"Number: {number}");           // 0
            Console.WriteLine($"Text: {text ?? "null"}");     // null
            Console.WriteLine($"Date: {date}");               // 1/1/0001 12:00:00 AM
            Console.WriteLine($"List: {(object)list ?? "null"}");     // null
        }

        // DEFAULTS OF VALUE TYPES: every built-in value type defaults to its "zero":
        // false, '\0' (the null character), 0, 0.0f, 0.0, 0.0m.
        public static void ValueTypeDefaults()
        {
            bool flag = default;
            char character = default;
            byte byteValue = default;
            short shortValue = default;
            long longValue = default;
            float floatValue = default;
            double doubleValue = default;
            decimal decimalValue = default;

            Console.WriteLine($"Boolean: {flag}");
            Console.WriteLine($"Char: '{character}' (null char)");
            Console.WriteLine($"Byte: {byteValue}");
            Console.WriteLine($"Short: {shortValue}");
            Console.WriteLine($"Long: {longValue}");
            Console.WriteLine($"Float: {floatValue}");
            Console.WriteLine($"Double: {doubleValue}");
            Console.WriteLine($"Decimal: {decimalValue}");
        }

        // DEFAULTS OF REFERENCE TYPES: always null - string, object, arrays, and all classes.
        // (The (object) casts let '??' combine a null collection with a string fallback.)
        public static void ReferenceTypeDefaults()
        {
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

    /*

    //1. 'var x = default;' does NOT compile - there is no type to infer from.
    //2. Most useful in generic code, where 'default' means "null or zero, whatever T is":
    //       T Find<T>(...) { ... return default; }

     */
}
