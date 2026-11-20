// =============================================================================
//  Feature     : Null-Coalescing Assignment (??=)
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    'x ??= value' assigns value to x ONLY IF x is currently null. It is
//    shorthand for 'if (x == null) x = value;' (or 'x = x ?? value;'). The right
//    side is evaluated only when actually needed, which makes lazy
//    initialization and "default if missing" logic a single short line.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class NullCoalescingAssignment
    {
        public static void Run()
        {
            DemonstrateBasics();
            DemonstrateLazyInitialization();
            DemonstrateDictionaryOfLists();
        }

        // ??= BASICS
        // - First '??=' assigns "Guest" because name is null; the second does nothing because
        //   name already has a value.
        // - Works for nullable value types too (int? retries).
        // - SHORT-CIRCUIT: when the variable is not null, the right side is NOT evaluated -
        //   ExpensiveLookup() is never called.
        // - The whole expression evaluates to the variable's final value, so it can be used
        //   inline: (numbers ??= new List<int>()).Add(5).
        public static void DemonstrateBasics()
        {
            string? name = null;
            name ??= "Guest";
            Console.WriteLine(name);               // Guest

            name ??= "Someone else";
            Console.WriteLine(name);               // Guest

            int? retries = null;
            retries ??= 3;
            Console.WriteLine($"Retries: {retries}");  // Retries: 3

            string? cached = "already here";
            cached ??= ExpensiveLookup();
            Console.WriteLine(cached);

            List<int>? numbers = null;
            (numbers ??= new List<int>()).Add(5);
            Console.WriteLine($"numbers.Count = {numbers.Count}"); // 1
        }

        static string ExpensiveLookup()
        {
            Console.WriteLine("  (expensive lookup ran!)");
            return "looked up";
        }

        // LAZY INITIALIZATION (the most common real-world use)
        // - '_templates ??= LoadTemplates()' loads the data on FIRST access and caches it in the
        //   field; every later access returns the cached list without reloading.
        public class ReportService
        {
            private List<string>? _templates;

            public List<string> Templates => _templates ??= LoadTemplates();

            private static List<string> LoadTemplates()
            {
                Console.WriteLine("  Loading templates (happens once)...");
                return new List<string> { "Monthly", "Quarterly" };
            }
        }

        public static void DemonstrateLazyInitialization()
        {
            var service = new ReportService();
            Console.WriteLine($"First access: {service.Templates.Count} templates");   // loads
            Console.WriteLine($"Second access: {service.Templates.Count} templates");  // cached
        }

        // GROUPING INTO A DICTIONARY OF LISTS
        // - TryGetValue leaves 'list' null when the key is missing.
        // - 'list ??= byLetter[key] = new List<string>()' creates and stores the list only on
        //   first use for that key; afterwards the existing list is reused.
        public static void DemonstrateDictionaryOfLists()
        {
            var words = new[] { "apple", "avocado", "banana", "blueberry", "cherry" };
            var byLetter = new Dictionary<char, List<string>>();

            foreach (var word in words)
            {
                byLetter.TryGetValue(word[0], out var list);
                list ??= byLetter[word[0]] = new List<string>();
                list.Add(word);
            }

            foreach (var pair in byLetter)
                Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}");
            // a: apple, avocado
            // b: banana, blueberry
            // c: cherry
        }
    }

    /*

    The null operator family:
        a ?? b       C# 2   value of a, or b if a is null           (2_NullableValueTypes.cs)
        a?.B         C# 6   member access only if a isn't null      (6_NullConditionalOperator.cs)
        a ??= b      C# 8   assign b to a only if a is null          (this file)
        a?.B = c     C# 14  assign to a.B only if a isn't null        (14_NullConditionalAssignment.cs)

    //1. ??= is NOT thread-safe. For lazy init shared across threads use Lazy<T> or
    //   LazyInitializer.EnsureInitialized.

    //2. The left side must be a variable, property or indexer - something assignable.

     */
}
