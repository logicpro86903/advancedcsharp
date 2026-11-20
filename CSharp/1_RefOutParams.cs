// =============================================================================
//  Feature     : ref, out and params Parameters
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    By default C# passes arguments BY VALUE (a copy of the value, or a copy of
//    the reference). Three modifiers change that:
//      ref    -> pass the caller's variable itself; the method can read AND replace it
//      out    -> the method MUST assign the variable; used to return extra values
//      params -> accept a variable number of arguments, received as an array
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class RefOutParams
    {
        public static void Run()
        {
            DemonstratePassByValue();
            DemonstrateRef();
            DemonstrateOut();
            DemonstrateParams();
        }

        // DEFAULT: PASS BY VALUE
        // - 'number' is a copy of the int, so assigning it does not affect the caller.
        // - 'list' is a copy of the REFERENCE: it still points to the caller's list object,
        //   so list.Add(...) is visible to the caller.
        // - Re-pointing the local copy (list = new ...) does NOT change the caller's variable.
        static void TryToChange(int number, List<string> list)
        {
            number = 999;
            list.Add("added");
            list = new List<string>();
        }

        public static void DemonstratePassByValue()
        {
            int n = 1;
            var items = new List<string>();
            TryToChange(n, items);
            Console.WriteLine($"n = {n}, items.Count = {items.Count}"); // n = 1, items.Count = 1
        }

        // REF PARAMETERS
        // - The method receives an alias to the CALLER'S variable, so any assignment inside
        //   the method changes the caller's variable directly.
        // - With a reference type, 'ref' lets the method REPLACE the caller's object entirely.
        // - The argument must be definitely assigned before the call, and 'ref' must be written
        //   at the call site too, so the side effect is obvious when reading the caller.
        static void Double(ref int number)
        {
            number *= 2;
        }

        static void ResetList(ref List<string> list)
        {
            list = new List<string> { "fresh" };
        }

        public static void DemonstrateRef()
        {
            int value = 21;
            Double(ref value);
            Console.WriteLine($"Doubled: {value}");                 // Doubled: 42

            var list = new List<string> { "old" };
            ResetList(ref list);
            Console.WriteLine($"List now: {string.Join(", ", list)}"); // List now: fresh
        }

        // OUT PARAMETERS
        // - The method MUST assign every out parameter on every code path before returning
        //   (the compiler enforces it), so the caller doesn't need to initialize the variable.
        // - Used to return more than one value from a method.
        // - The common "Try" pattern returns success as bool and the value via out:
        //   int.TryParse, Dictionary.TryGetValue, DateTime.TryParse, ...
        static bool TryDivide(int a, int b, out int result)
        {
            if (b == 0)
            {
                result = 0;
                return false;
            }
            result = a / b;
            return true;
        }

        static void GetMinMax(int[] numbers, out int min, out int max)
        {
            min = int.MaxValue;
            max = int.MinValue;
            foreach (int n in numbers)
            {
                if (n < min) min = n;
                if (n > max) max = n;
            }
        }

        // USING OUT
        // - 'int result;' is declared but not initialized - the callee guarantees assignment.
        // - 'out int parsed' declares the variable inline (C# 7 syntax, see 7_OutVariablesAnywhere.cs).
        // - GetMinMax returns two values through two out parameters.
        public static void DemonstrateOut()
        {
            int result;
            if (TryDivide(10, 3, out result))
                Console.WriteLine($"10 / 3 = {result}");             // 10 / 3 = 3

            if (!TryDivide(1, 0, out result))
                Console.WriteLine("Cannot divide by zero");

            if (int.TryParse("123", out int parsed))
                Console.WriteLine($"Parsed: {parsed}");              // Parsed: 123

            int min, max;
            GetMinMax(new[] { 4, 9, 1, 7 }, out min, out max);
            Console.WriteLine($"Min: {min}, Max: {max}");            // Min: 1, Max: 9
        }

        // PARAMS ARRAYS
        // - 'params T[]' lets callers pass zero or more arguments separated by commas; the
        //   compiler packs them into a new array for you.
        // - It must be the LAST parameter, and a method can have only one.
        // - With no arguments the method receives an EMPTY array (never null).
        static int Sum(params int[] numbers)
        {
            int total = 0;
            foreach (int n in numbers) total += n;
            return total;
        }

        static void Log(string level, params object[] parts)
        {
            Console.WriteLine($"[{level}] {string.Join(" ", parts)}");
        }

        // USING PARAMS
        // - Sum(1, 2, 3, 4) becomes Sum(new int[] { 1, 2, 3, 4 }).
        // - An existing array can still be passed directly.
        // - params object[] accepts mixed types (string.Format / Console.WriteLine use this).
        public static void DemonstrateParams()
        {
            Console.WriteLine(Sum());                       // 0
            Console.WriteLine(Sum(5));                      // 5
            Console.WriteLine(Sum(1, 2, 3, 4));             // 10
            Console.WriteLine(Sum(new[] { 10, 20 }));       // 30

            Log("INFO", "User", 42, "logged in at", DateTime.Today.ToShortDateString());

            Console.WriteLine("{0} + {1} = {2}", 1, 2, 3);
        }
    }

    /*

    Quick comparison:
                  must init before call?   must assign in method?   direction
        (none)    yes                      no                        in (copy)
        ref       yes                      no                        in & out
        out       no                       YES                       out
        in (C#7.2) yes                     can't assign              in, read-only reference
                                                                     (see 7_ReadOnlyStructsAndInParams.cs)

    //1. Prefer returning a value (or a tuple, C# 7) over out parameters for new code.
    //   'out' is still idiomatic for the TryXxx pattern.

    //2. 'ref' on a reference type (ref List<T>) is rarely needed - only when the method must
    //   REPLACE the caller's object, not just modify it.

    //3. params allocates an array on every call. C# 13 'params collections' allow
    //   params ReadOnlySpan<T> to avoid that (see 13_ParamsCollectioncs.cs).

    //4. async methods and iterators can't have ref/out parameters.

     */
}
