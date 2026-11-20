// =============================================================================
//  Feature     : Nullable Value Types (T?) and the ?? Operator
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    Value types (int, double, DateTime...) normally always hold a value.
//    Nullable<T>, written T?, adds a "no value" (null) state - perfect for
//    optional data such as nullable database columns. It exposes HasValue,
//    Value and GetValueOrDefault(). C# 2 also added the null-coalescing operator
//    '??' (use a fallback when null), and LIFTED operators so +, <, == work on T?.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class NullableValueTypes
    {
        public static void Run()
        {
            DemonstrateNullable();
            DemonstrateNullCoalescing();
            DemonstrateLiftedOperators();
        }

        // NULLABLE<T> BASICS
        // - 'int?' is shorthand for Nullable<int>; both forms are identical.
        // - HasValue tells you whether a value is present; Value returns it (throws if null).
        // - GetValueOrDefault() returns default(T) (0 for int) or a supplied fallback when null.
        // - Typical use: optional data, e.g. a 'ShippedOn' date that isn't known yet.
        public static void DemonstrateNullable()
        {
            int? age = null;
            Nullable<double> price = 9.99;

            Console.WriteLine($"age.HasValue: {age.HasValue}");     // False
            Console.WriteLine($"price.Value: {price.Value}");       // 9.99

            age = 30;
            if (age.HasValue)
                Console.WriteLine($"Age is {age.Value}");            // Age is 30

            int? missing = null;
            Console.WriteLine(missing.GetValueOrDefault());          // 0
            Console.WriteLine(missing.GetValueOrDefault(-1));        // -1

            DateTime? shippedOn = null;
            Console.WriteLine(shippedOn.HasValue ? shippedOn.Value.ToShortDateString() : "Not shipped yet");
        }

        // NULL-COALESCING OPERATOR ??
        // - 'a ?? b' returns a if it is not null, otherwise b.
        // - Works for nullable value types AND reference types.
        // - Can be chained: the first non-null value from left to right wins.
        public static void DemonstrateNullCoalescing()
        {
            int? userSetting = null;
            int pageSize = userSetting ?? 20;
            Console.WriteLine($"Page size: {pageSize}");             // Page size: 20

            string? nickname = null;
            string display = nickname ?? "Anonymous";
            Console.WriteLine(display);                              // Anonymous

            int? a = null, b = null, c = 3;
            Console.WriteLine(a ?? b ?? c ?? 0);                     // 3
        }

        // LIFTED OPERATORS
        // - Operators defined for int automatically work on int? ("lifted").
        // - Arithmetic: if either operand is null, the result is null (5 + null = null).
        // - Relational (<, >, <=, >=): any comparison involving null returns FALSE.
        // - Equality: '== null' tests for "no value".
        // - Boxing a null int? produces a real null reference, not a boxed "empty" struct.
        public static void DemonstrateLiftedOperators()
        {
            int? x = 5;
            int? y = null;
            int? sum = x + y;
            Console.WriteLine($"5 + null = {(sum.HasValue ? sum.ToString() : "null")}"); // 5 + null = null

            Console.WriteLine($"x > y: {x > y}");                    // False
            Console.WriteLine($"x < y: {x < y}");                    // False
            Console.WriteLine($"y == null: {y == null}");            // True

            object? boxed = y;
            Console.WriteLine($"boxed is null: {boxed == null}");    // True
        }
    }

    /*

    //1. Accessing .Value when HasValue is false throws InvalidOperationException.
    int? n = null;
    int bad = n.Value;    // throws

    //2. Converting int? to int requires an explicit cast (which also throws on null):
    int ok = (int)x;      // fine only if x has a value

    //3. Don't confuse with C# 8 nullable REFERENCE types (string?), which are only compiler
    //   annotations - see 8_NullableReferenceTypes.cs. int? is a real different type (Nullable<int>).

    C# 8 added ??= (assign if null) - see 8_NullCoalescingAssignment.cs.

     */
}
