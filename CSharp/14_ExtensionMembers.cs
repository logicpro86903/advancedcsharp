// =============================================================================
//  Feature     : Extension Members (extension blocks)
//  C# Version  : C# 14.0
//  .NET        : .NET 10
//  Released    : 2025 (Visual Studio 2026)
// -----------------------------------------------------------------------------
//  Summary:
//    'extension(Receiver r) { ... }' blocks are a new way to declare extensions.
//    Besides methods they support extension PROPERTIES, STATIC extension members
//    (called on the type itself, e.g. string.Random(8)) and operators. Classic
//    extension methods (C# 3, see 3_ExtensionMethods.cs) could only add
//    instance-style METHODS - you couldn't write 'list.IsEmpty' as a property or
//    add a static helper to someone else's type. Blocks also group all
//    extensions for the same receiver instead of repeating 'this string s'.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp
{
    public class ExtensionMembers
    {
        public static void Run()
        {
            DemonstrateExtensionProperties();
            DemonstrateExtensionMethodsInBlocks();
            DemonstrateStaticExtensionMembers();
            DemonstrateGenericExtensions();
        }

        // EXTENSION PROPERTIES
        // - WordTotal, IsBlank and FirstWord are declared in 'extension(string text)' below.
        // - They are used WITHOUT parentheses, exactly like real properties of string.
        public static void DemonstrateExtensionProperties()
        {
            string sentence = "Extension members are new in C# fourteen";

            Console.WriteLine($"Word count: {sentence.WordTotal}");       // 7
            Console.WriteLine($"Is blank: {sentence.IsBlank}");           // False
            Console.WriteLine($"'   ' is blank: {"   ".IsBlank}");       // True
            Console.WriteLine($"First word: {sentence.FirstWord}");       // Extension
        }

        // EXTENSION METHODS DECLARED IN A BLOCK
        // - Called just like classic extension methods: "hello world".Truncate(5).
        // - They still compile to static methods, so the static call form also works.
        public static void DemonstrateExtensionMethodsInBlocks()
        {
            Console.WriteLine("hello world".Truncate(5));                 // hello...
            Console.WriteLine("short".Truncate(10));                      // short

            Console.WriteLine(TextExtensions.Truncate("compiled as a static method", 8));
        }

        // STATIC EXTENSION MEMBERS
        // - Declared in a block WITHOUT a receiver name: 'extension(string) { public static ... }'.
        // - Called on the TYPE, as if they had always been part of it: string.Random(8),
        //   DateTime.NextMonday (a static extension PROPERTY).
        // - DateTime.Today.IsWeekend is an instance extension property on DateTime.
        public static void DemonstrateStaticExtensionMembers()
        {
            string id = string.Random(8);
            Console.WriteLine($"Random id: {id} (length {id.Length})");

            Console.WriteLine($"Today is weekend: {DateTime.Today.IsWeekend}");
            Console.WriteLine($"Next Monday: {DateTime.NextMonday:yyyy-MM-dd}");
        }

        // GENERIC EXTENSIONS
        // - 'extension<T>(IReadOnlyList<T> source)' adds members to EVERY IReadOnlyList<T>
        //   (List<int>, List<string>, arrays...): IsEmpty and SecondToLast properties.
        // - Members may declare their OWN extra type parameters: PairWith<TOther>.
        // - 'extension<T>(List<T>)' adds a static factory callable as List<int>.Filled(3, 7).
        public static void DemonstrateGenericExtensions()
        {
            var numbers = new List<int> { 4, 8, 15, 16, 23, 42 };
            var empty = new List<string>();

            Console.WriteLine($"numbers.IsEmpty: {numbers.IsEmpty}, empty.IsEmpty: {empty.IsEmpty}"); // False, True
            Console.WriteLine($"Second-to-last: {numbers.SecondToLast}");                          // 23

            var pairs = numbers.PairWith(new[] { "a", "b", "c" });
            Console.WriteLine(string.Join(", ", pairs));                                            // (4, a), (8, b), (15, c)

            var filled = List<int>.Filled(3, 7);
            Console.WriteLine($"Filled: [{string.Join(", ", filled)}]");                            // [7, 7, 7]
        }
    }

    // DECLARING EXTENSION MEMBERS
    // - Like classic extension methods, they live in a top-level, non-generic static class.
    // - 'extension(string text) { ... }' : the RECEIVER parameter 'text' is shared by every member
    //   in the block, so no 'this string text' is repeated on each method.
    // - 'extension(string) { ... }'      : no receiver name -> a block of STATIC extensions.
    // - Extension properties must be COMPUTED (expression-bodied or get accessors) - extensions
    //   can't add storage/state to a type.
    public static class TextExtensions
    {
        extension(string text)
        {
            public int WordTotal => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            public bool IsBlank => string.IsNullOrWhiteSpace(text);

            public string FirstWord
            {
                get
                {
                    int space = text.IndexOf(' ');
                    return space < 0 ? text : text[..space];
                }
            }

            public string Truncate(int maxLength) =>
                text.Length <= maxLength ? text : text[..maxLength] + "...";
        }

        extension(string)
        {
            public static string Random(int length)
            {
                const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
                var rng = System.Random.Shared;
                var result = new char[length];
                for (int i = 0; i < length; i++)
                    result[i] = chars[rng.Next(chars.Length)];
                return new string(result);
            }
        }

        extension(DateTime date)
        {
            public bool IsWeekend => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }

        extension(DateTime)
        {
            public static DateTime NextMonday
            {
                get
                {
                    var today = DateTime.Today;
                    int days = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                    return today.AddDays(days == 0 ? 7 : days);
                }
            }
        }
    }

    // GENERIC EXTENSION BLOCKS
    // - The type parameter goes on the block: extension<T>(IReadOnlyList<T> source).
    // - The receiver can be an interface, so the members apply to every implementation.
    // - 'extension<T>(List<T>)' (no receiver name) declares static members for List<T>.
    public static class CollectionExtensionMembers
    {
        extension<T>(IReadOnlyList<T> source)
        {
            public bool IsEmpty => source.Count == 0;

            public T SecondToLast => source.Count >= 2
                ? source[^2]
                : throw new InvalidOperationException("Need at least two items");

            public IEnumerable<(T, TOther)> PairWith<TOther>(IEnumerable<TOther> other) => source.Zip(other);
        }

        extension<T>(List<T>)
        {
            public static List<T> Filled(int count, T value) => Enumerable.Repeat(value, count).ToList();
        }
    }

    /*

    Classic vs new syntax - both are valid C# 14 and can live in the same static class:
        public static int WordCount(this string s) => ...;          // C# 3 style
        extension(string s) { public int WordCount() => ...; }      // C# 14 style (same call site!)
    Existing extension methods don't need to be rewritten - the new syntax is source and binary
    compatible. Use blocks when you want properties/static members or to group many extensions.

    What extension members can be:
        ✅ instance methods and properties, static methods and properties, operators
        ❌ indexers (not in C# 14)
        ❌ fields / auto-properties with storage - extensions can't add STATE to a type
        ❌ access to private members of the extended type

    //1. Name clashes: a real member on the type always wins over an extension member.
    //2. The containing static class must be in scope (using its namespace) - same as C# 3.

     */
}
