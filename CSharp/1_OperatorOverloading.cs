// =============================================================================
//  Feature     : Operator Overloading and User-Defined Conversions
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    Types can define what built-in operators (+, -, *, ==, !=, <, > ...) mean
//    for them, so domain types like Money, Vector or Matrix read like math:
//    'price + tax' instead of 'price.Add(tax)'. Operators are public static
//    methods named 'operator X'. Types can also define IMPLICIT conversions
//    (safe, no cast needed) and EXPLICIT conversions (may lose data, cast required).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class OperatorOverloading
    {
        public static void Run()
        {
            DemonstrateArithmeticOperators();
            DemonstrateEqualityAndConversions();
        }

        public struct Money
        {
            public readonly decimal Amount;
            public readonly string Currency;

            public Money(decimal amount, string currency)
            {
                Amount = amount;
                Currency = currency;
            }

            // BINARY OPERATORS
            // - Declared as 'public static ReturnType operator X(left, right)'.
            // - At least one parameter must be the containing type.
            // - Operand types don't have to match: Money * decimal is allowed.
            public static Money operator +(Money a, Money b)
            {
                if (a.Currency != b.Currency)
                    throw new InvalidOperationException("Currency mismatch");
                return new Money(a.Amount + b.Amount, a.Currency);
            }

            public static Money operator *(Money a, decimal factor)
            {
                return new Money(a.Amount * factor, a.Currency);
            }

            // UNARY OPERATOR: takes a single operand (here negation: -price).
            public static Money operator -(Money a)
            {
                return new Money(-a.Amount, a.Currency);
            }

            // EQUALITY OPERATORS
            // - == and != MUST be overloaded as a pair (compiler error otherwise).
            // - Also override Equals and GetHashCode so equality is consistent everywhere
            //   (collections, dictionaries, LINQ Distinct, etc.).
            public static bool operator ==(Money a, Money b)
            {
                return a.Amount == b.Amount && a.Currency == b.Currency;
            }

            public static bool operator !=(Money a, Money b)
            {
                return !(a == b);
            }

            public override bool Equals(object? obj)
            {
                return obj is Money && this == (Money)obj;
            }

            public override int GetHashCode()
            {
                return Amount.GetHashCode() ^ Currency.GetHashCode();
            }

            // IMPLICIT CONVERSION: decimal -> Money happens automatically (Money m = 5m;).
            // Only use implicit when the conversion can never fail or lose information.
            public static implicit operator Money(decimal amount)
            {
                return new Money(amount, "USD");
            }

            // EXPLICIT CONVERSION: Money -> decimal requires a cast ((decimal)money) because
            // the currency information is lost.
            public static explicit operator decimal(Money money)
            {
                return money.Amount;
            }

            public override string ToString()
            {
                return Amount.ToString("F2") + " " + Currency;
            }
        }

        // USING ARITHMETIC OPERATORS
        // - The overloaded operators make Money arithmetic read naturally.
        // - Compound assignment (+=) is derived automatically from + : total = total + tax.
        public static void DemonstrateArithmeticOperators()
        {
            Money price = new Money(19.99m, "USD");
            Money tax = new Money(1.60m, "USD");

            Console.WriteLine(price + tax);      // 21.59 USD
            Console.WriteLine(price * 3);        // 59.97 USD
            Console.WriteLine(-price);           // -19.99 USD

            Money total = price;
            total += tax;
            Console.WriteLine(total);            // 21.59 USD
        }

        // USING EQUALITY AND CONVERSIONS
        // - == compares by value thanks to our overload (structs don't get == by default).
        // - Money fromDecimal = 5m;  uses the implicit conversion (no cast).
        // - (decimal)fromDecimal     uses the explicit conversion (cast required).
        public static void DemonstrateEqualityAndConversions()
        {
            Money a = new Money(10m, "EUR");
            Money b = new Money(10m, "EUR");
            Console.WriteLine($"a == b: {a == b}");          // a == b: True
            Console.WriteLine($"a.Equals(b): {a.Equals(b)}"); // a.Equals(b): True

            Money fromDecimal = 5m;
            decimal raw = (decimal)fromDecimal;
            Console.WriteLine($"{fromDecimal} -> {raw}");    // 5.00 USD -> 5
        }
    }

    /*

    //1. Only overload operators when the meaning is obvious (math types, money, vectors).
    //2. Overloading == without Equals/GetHashCode gives inconsistent behaviour in collections.
    //3. Make conversions implicit only if they can never fail or lose data.
    //4. Some operators come in required pairs: == / !=, < / >, <= / >=, true / false.

    C# 11 later allowed operators in interfaces (static abstract members / generic math),
    and C# 14 allows user-defined compound assignment (operator +=).

     */
}
