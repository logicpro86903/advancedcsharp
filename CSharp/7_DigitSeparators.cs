using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class DigitSeparators
    {
        public static void Run()
        {
            DemonstrateIntegerSeparators();
            PhoneNumbersAndIds();
        }

        public static void DemonstrateIntegerSeparators()
        {
            // Large numbers without separators (hard to read)
            int population = 8500000;
            long distance = 93000000;

            // Large numbers with separators (easy to read)
            int readablePopulation = 8_500_000;
            long readableDistance = 93_000_000;

            // Both values are identical
            Console.WriteLine($"Population equal: {population == readablePopulation}"); // True
            Console.WriteLine($"Distance equal: {distance == readableDistance}"); // True

            // Different grouping patterns
            int million = 1_000_000;
            int alsoMillion = 1000_000;
            int stillMillion = 10_00_000;

            Console.WriteLine($"All millions equal: {million == alsoMillion && alsoMillion == stillMillion}"); // True

            // Very large numbers
            long trillion = 1_000_000_000_000L;
            long quadrillion = 1_000_000_000_000_000L;

            Console.WriteLine($"Trillion: {trillion:N0}");
            Console.WriteLine($"Quadrillion: {quadrillion:N0}");
        }

        public static void PhoneNumbersAndIds()
        {
            // Phone numbers
            long phoneNumber = 1_555_123_4567;
            long internationalPhone = 44_20_7946_0958;

            // Credit card numbers
            long creditCard = 4532_1234_5678_9012;

            // Social security numbers
            int ssn = 123_45_6789;

            // Account numbers
            long accountNumber = 1234_5678_9012_3456;

            Console.WriteLine($"Phone: {phoneNumber}");
            Console.WriteLine($"International: +{internationalPhone}");
            Console.WriteLine($"Credit Card: {creditCard}");
            Console.WriteLine($"SSN: {ssn}");
            Console.WriteLine($"Account: {accountNumber}");
        }
    }
}
