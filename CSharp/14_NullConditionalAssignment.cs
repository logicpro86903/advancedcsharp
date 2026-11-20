// =============================================================================
//  Feature     : Null-Conditional Assignment (a?.B = value)
//  C# Version  : C# 14.0
//  .NET        : .NET 10
//  Released    : 2025 (Visual Studio 2026)
// -----------------------------------------------------------------------------
//  Summary:
//    The null-conditional operators ?. and ?[] can now appear on the LEFT side of
//    an assignment: 'customer?.Name = "Alice";'. The assignment happens only if
//    'customer' is not null; otherwise nothing happens and the right-hand side is
//    NOT evaluated. Since C# 6 you could READ safely with ?. (see
//    6_NullConditionalOperator.cs), but WRITING still needed an if-statement
//    around every assignment. This closes that gap, including compound
//    assignments (+=, -=, ??=) and event subscription.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class NullConditionalAssignment
    {
        public static void Run()
        {
            DemonstrateBasicAssignment();
            DemonstrateCompoundAssignment();
            DemonstrateIndexersAndChains();
            DemonstrateRightSideNotEvaluated();
        }

        public class Settings
        {
            public string Theme { get; set; } = "light";
            public int FontSize { get; set; } = 12;
            public List<string> RecentFiles { get; set; } = new();
        }

        public class UserProfile
        {
            public string Name { get; set; } = "";
            public Settings? Settings { get; set; }
            public int LoginCount { get; set; }
        }

        // BASIC NULL-CONDITIONAL ASSIGNMENT
        // - Before C# 14: 'if (loggedIn != null) loggedIn.Name = ...;'
        // - C# 14: 'loggedIn?.Name = ...;' - same behaviour in one line.
        // - guest is null, so 'guest?.Name = "Nobody"' is silently skipped (no exception).
        // - CHAINS: 'loggedIn?.Settings?.Theme = "dark"' assigns only if EVERY link is non-null;
        //   for Bob (Settings == null) the assignment is skipped.
        public static void DemonstrateBasicAssignment()
        {
            UserProfile? loggedIn = new() { Name = "Alice", Settings = new() };
            UserProfile? guest = null;

            if (loggedIn != null)
                loggedIn.Name = "Alice Smith";

            loggedIn?.Name = "Alice Smith";
            guest?.Name = "Nobody";

            Console.WriteLine($"loggedIn: {loggedIn?.Name}, guest is null: {guest is null}");

            loggedIn?.Settings?.Theme = "dark";
            Console.WriteLine($"Theme: {loggedIn?.Settings?.Theme}");      // dark

            var noSettings = new UserProfile { Name = "Bob" };
            noSettings.Settings?.Theme = "dark";
            Console.WriteLine($"Bob has settings: {noSettings.Settings is not null}"); // False
        }

        // COMPOUND ASSIGNMENT AND EVENTS
        // - Compound operators work too: user?.LoginCount += 1, user?.Settings?.FontSize += 2.
        // - Event subscription: 'notifier?.Changed += handler' subscribes only if notifier exists.
        // - Increment/decrement (user?.LoginCount++) is NOT allowed - use += 1.
        public static void DemonstrateCompoundAssignment()
        {
            UserProfile? user = new() { Name = "Carol", Settings = new() };

            user?.LoginCount += 1;
            user?.Settings?.FontSize += 2;
            Console.WriteLine($"Logins: {user?.LoginCount}, font size: {user?.Settings?.FontSize}"); // 1, 14

            Notifier? notifier = new();
            notifier?.Changed += () => Console.WriteLine("  Changed event fired");
            notifier?.Raise();
        }

        public class Notifier
        {
            public event Action? Changed;
            public void Raise() => Changed?.Invoke();
        }

        // NULL-CONDITIONAL INDEXER ASSIGNMENT (?[])
        // - 'scores?["Bob"] = 7' writes through the indexer only if the dictionary isn't null.
        // - Works with arrays (names?[0] = ...) and at the end of longer chains
        //   (user?.Settings?.RecentFiles?[0] = ...).
        public static void DemonstrateIndexersAndChains()
        {
            Dictionary<string, int>? scores = new() { ["Alice"] = 10 };
            Dictionary<string, int>? noScores = null;

            scores?["Bob"] = 7;
            noScores?["Bob"] = 7;
            Console.WriteLine($"scores: {string.Join(", ", scores!)}");   // [Alice, 10], [Bob, 7]

            string[]? names = new string[2];
            names?[0] = "first";
            Console.WriteLine($"names[0] = {names?[0]}");

            UserProfile? user = new() { Settings = new() { RecentFiles = new() { "a.txt" } } };
            user?.Settings?.RecentFiles?[0] = "renamed.txt";
            Console.WriteLine($"Recent: {user?.Settings?.RecentFiles[0]}"); // renamed.txt
        }

        static string LoadThemeFromDisk()
        {
            Console.WriteLine("  (LoadThemeFromDisk called)");
            return "solarized";
        }

        // THE RIGHT-HAND SIDE ONLY RUNS IF THE ASSIGNMENT HAPPENS
        // - With a null target, LoadThemeFromDisk() is NOT called at all (short-circuit).
        // - With a non-null target it runs and its result is assigned.
        // - Important when the right side is expensive or has side effects.
        public static void DemonstrateRightSideNotEvaluated()
        {
            Settings? missing = null;
            missing?.Theme = LoadThemeFromDisk();
            Console.WriteLine("After assigning to null target: no load happened");

            Settings? present = new();
            present?.Theme = LoadThemeFromDisk();
            Console.WriteLine($"Theme now: {present?.Theme}");
        }
    }

    /*

    The complete null-operator family (worth memorizing):
        a ?? b         C# 2    a, or b if a is null
        a?.B  a?[i]    C# 6    read B only if a isn't null (result is null otherwise)
        a ??= b        C# 8    assign b to a only if a IS null
        a?.B = c       C# 14   assign c to a.B only if a is NOT null

    //1. Use it as a statement. Mixing it into larger expressions makes it hard to tell whether
    //   the assignment actually happened.

    //2. Silently skipping a write can hide bugs. If a null target is actually an error,
    //   check and throw instead of using ?.

    Other C# 14 features worth knowing (see also 14_ExtensionMembers.cs and 13_BackingFields.cs for 'field'):
    - nameof(List<>)                            unbound generic types in nameof
    - (text, out result) => ...                 lambda parameter modifiers without explicit types
    - implicit Span<T>/ReadOnlySpan<T> conversions from arrays in more places
    - partial constructors and partial events   (for source generators)
    - user-defined compound assignment          public void operator +=(T value)

     */
}
