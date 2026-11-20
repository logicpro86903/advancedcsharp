// =============================================================================
//  Feature     : Extension Methods
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    Extension methods let you "add" methods to existing types - including types
//    you don't own (string, int) and interfaces (IEnumerable<T>) - without
//    inheritance or modifying the original. They are static methods in a static
//    class whose first parameter is marked with 'this'; callers invoke them with
//    instance-method syntax. All of LINQ (Where, Select, OrderBy...) is built as
//    extension methods on IEnumerable<T>.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ExtensionMethods
    {
        public static void Run()
        {
            DemonstrateExtensionMethods();
            DemonstrateChaining();
        }

        // CALLING EXTENSION METHODS
        // - sentence.WordCount() looks like an instance method of string, but string has no
        //   such method - the compiler rewrites it to StringExtensions.WordCount(sentence).
        // - Because it's really a static call, it can be invoked on a NULL reference without
        //   an immediate NullReferenceException (the method decides how to handle null).
        // - Value types (int) can be extended too: 7.IsEven().
        public static void DemonstrateExtensionMethods()
        {
            string sentence = "the quick brown fox";

            Console.WriteLine(sentence.WordCount());         // 4
            Console.WriteLine(sentence.ToTitleCase());       // The Quick Brown Fox
            Console.WriteLine("level".IsPalindrome());       // True

            Console.WriteLine(StringExtensions.WordCount(sentence)); // 4 - the equivalent static call

            string? nothing = null;
            Console.WriteLine(nothing.IsNullOrEmpty());      // True

            Console.WriteLine(7.IsEven());                   // False
        }

        // EXTENDING INTERFACES + CHAINING (how LINQ works)
        // - EveryOther and Doubled extend IEnumerable<T>, so they apply to EVERY collection
        //   (List, array, HashSet, query results...).
        // - Each returns IEnumerable<T>, so calls chain fluently: source.EveryOther().Doubled().
        public static void DemonstrateChaining()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            var result = numbers.EveryOther().Doubled();
            Console.WriteLine(string.Join(", ", result));    // 2, 6, 10
        }
    }

    // DECLARING EXTENSION METHODS
    // - Must live in a NON-generic, NON-nested, STATIC class.
    // - The method itself must be static.
    // - 'this' before the FIRST parameter marks the type being extended; that parameter
    //   receives the object the method was "called on".
    public static class StringExtensions
    {
        public static int WordCount(this string text)
        {
            return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string ToTitleCase(this string text)
        {
            var words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return string.Join(" ", words);
        }

        public static bool IsPalindrome(this string text)
        {
            for (int i = 0, j = text.Length - 1; i < j; i++, j--)
                if (text[i] != text[j]) return false;
            return true;
        }

        // Extending a nullable type: the method safely handles a null receiver.
        public static bool IsNullOrEmpty(this string? text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }
    }

    // CUSTOM LINQ-STYLE OPERATORS
    // - A GENERIC extension method on IEnumerable<T> combined with an iterator (yield return)
    //   is exactly how LINQ operators are implemented: lazy, chainable, and type-agnostic.
    public static class SequenceExtensions
    {
        public static IEnumerable<T> EveryOther<T>(this IEnumerable<T> source)
        {
            bool take = true;
            foreach (var item in source)
            {
                if (take) yield return item;
                take = !take;
            }
        }

        public static IEnumerable<int> Doubled(this IEnumerable<int> source)
        {
            foreach (var n in source)
                yield return n * 2;
        }
    }

    /*

    //1. Instance methods always win. If the type later adds a method with the same signature,
    //   your extension silently stops being called.

    //2. They must be imported: the containing namespace has to be in a using directive.

    //3. They can't access private members - they're just static methods.

    C# 14 added extension members (extension blocks) with extension properties,
    static extension members and operators (see 14_ExtensionMembers.cs).

     */
}
