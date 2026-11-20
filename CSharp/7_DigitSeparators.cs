// =============================================================================
//  Feature     : Digit Separators and Binary Literals
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    The underscore '_' can be placed between digits of any numeric literal to
//    make long numbers readable: 1_000_000 instead of 1000000. Separators are
//    purely visual - the compiler ignores them, so the value is unchanged. They
//    work in decimal, hexadecimal (0xFF_FF) and binary literals (0b1010_0101,
//    also new in C# 7.0), and in int, long, float, double and decimal values.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class DigitSeparators
    {
        public static void Run()
        {
            DemonstrateIntegerSeparators();
            PhoneNumbersAndIds();
        }

        // DIGIT SEPARATORS IN NUMERIC LITERALS
        // - 8_500_000 is exactly the same value as 8500000 - the comparisons print True.
        // - Grouping is free-form: 1_000_000, 1000_000 and 10_00_000 (Indian lakh style) are
        //   all one million. Group in whatever way matches the domain.
        // - Rules: '_' can't be the first or last character, or sit next to the decimal
        //   point or a type suffix (L, m, f). Since C# 7.2 it may follow a 0x/0b prefix: 0x_FF.
        public static void DemonstrateIntegerSeparators()
        {
            int population = 8500000;
            long distance = 93000000;

            int readablePopulation = 8_500_000;
            long readableDistance = 93_000_000;

            Console.WriteLine($"Population equal: {population == readablePopulation}"); // True
            Console.WriteLine($"Distance equal: {distance == readableDistance}"); // True

            int million = 1_000_000;
            int alsoMillion = 1000_000;
            int stillMillion = 10_00_000;

            Console.WriteLine($"All millions equal: {million == alsoMillion && alsoMillion == stillMillion}"); // True

            long trillion = 1_000_000_000_000L;
            long quadrillion = 1_000_000_000_000_000L;

            Console.WriteLine($"Trillion: {trillion:N0}");
            Console.WriteLine($"Quadrillion: {quadrillion:N0}");
        }

        // GROUPING TO MATCH REAL-WORLD FORMATS
        // - Separators can mirror how humans read identifiers: phone numbers, card numbers, etc.
        // - Note the output prints WITHOUT underscores - they exist only in source code.
        public static void PhoneNumbersAndIds()
        {
            long phoneNumber = 1_555_123_4567;
            long internationalPhone = 44_20_7946_0958;
            long creditCard = 4532_1234_5678_9012;
            int ssn = 123_45_6789;
            long accountNumber = 1234_5678_9012_3456;

            Console.WriteLine($"Phone: {phoneNumber}");
            Console.WriteLine($"International: +{internationalPhone}");
            Console.WriteLine($"Credit Card: {creditCard}");
            Console.WriteLine($"SSN: {ssn}");
            Console.WriteLine($"Account: {accountNumber}");
        }
    }

    /*

    Binary literals + separators are great for bit masks:
        byte flags = 0b_0000_0101;    // bits 0 and 2 set
        int mask   = 0xFF_FF_00_00;

    //1. Real identifiers like phone or card numbers should normally be STRINGS (leading zeros,
    //   '+' signs, no arithmetic). They are numbers here only to show the separator syntax.

     */
}
