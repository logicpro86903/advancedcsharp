// =============================================================================
//  Feature     : Indices and Ranges (^ and ..)
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    The "hat" operator ^n creates an index counted FROM THE END (^1 = last
//    element), replacing 'array[array.Length - 1]'. The range operator a..b
//    selects a SLICE from index a (inclusive) to b (exclusive); either end can be
//    omitted or use ^. They are backed by the System.Index and System.Range
//    types, so they can be stored in variables, and work on arrays, strings,
//    Span<T>, and (for ^ only) List<T>.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class IndicesAndRanges
    {
        public static void Run()
        {
            DemonstrateBasicIndexing();
            DemonstrateBasicRanges();
        }

        // INDEX FROM END (^)
        // - fruits[^1] is the LAST element, fruits[^2] the second to last, and so on.
        //   It replaces the clumsy fruits[fruits.Length - 1].
        // - ^0 means "one past the end" (== Length), so fruits[^0] throws
        //   IndexOutOfRangeException - it is mainly useful as a range END.
        // - Works with arrays, strings and List<T> (any type with Count/Length + an int indexer).
        // - The 'Index' type lets you store an index in a variable: Index lastIndex = ^1.
        public static void DemonstrateBasicIndexing()
        {
            string[] fruits = { "apple", "banana", "cherry", "date", "elderberry" };

            Console.WriteLine($"First: {fruits[0]}");           // apple
            Console.WriteLine($"Second: {fruits[1]}");          // banana
            Console.WriteLine($"Last: {fruits[fruits.Length - 1]}"); // elderberry

            Console.WriteLine($"Last: {fruits[^1]}");           // elderberry
            Console.WriteLine($"Second to last: {fruits[^2]}"); // date
            Console.WriteLine($"Third to last: {fruits[^3]}");  // cherry

            var numbers = new List<int> { 10, 20, 30, 40, 50 };
            Console.WriteLine($"Last number: {numbers[^1]}");    // 50
            Console.WriteLine($"Second to last: {numbers[^2]}"); // 40

            string text = "Hello World";
            Console.WriteLine($"Last character: {text[^1]}");    // 'd'
            Console.WriteLine($"Second to last: {text[^2]}");    // 'l'

            int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Index lastIndex = ^1;
            Index secondLastIndex = ^2;

            Console.WriteLine($"Last value: {values[lastIndex]}");       // 10
            Console.WriteLine($"Second last: {values[secondLastIndex]}"); // 9
        }

        // RANGES (..)
        // - start..end : start is INCLUSIVE, end is EXCLUSIVE -> animals[0..3] = elements 0, 1, 2.
        // - Omit an end:  ..3 (from start), 2.. (to the end), .. (everything).
        // - Mix with ^:   ^3.. (last three), 2..^1 (from index 2, excluding the last element).
        // - Slicing an ARRAY or STRING creates a COPY (a new array/string); slicing a Span<T>
        //   creates a view without copying.
        // - The 'Range' type stores a range in a variable; GetOffsetAndLength converts it to
        //   concrete numbers for a given collection length.
        // - Compare with the old way: allocate a new array + Array.Copy.
        public static void DemonstrateBasicRanges()
        {
            string[] animals = { "cat", "dog", "elephant", "fish", "giraffe", "horse", "iguana" };

            string[] traditionalSlice = new string[3];
            Array.Copy(animals, 1, traditionalSlice, 0, 3);

            string[] firstThree = animals[0..3];        // cat, dog, elephant
            string[] middleThree = animals[2..5];       // elephant, fish, giraffe
            string[] lastThree = animals[^3..^0];       // giraffe, horse, iguana

            Console.WriteLine($"First three: [{string.Join(", ", firstThree)}]");
            Console.WriteLine($"Middle three: [{string.Join(", ", middleThree)}]");
            Console.WriteLine($"Last three: [{string.Join(", ", lastThree)}]");

            string[] fromStart = animals[..3];          // cat, dog, elephant
            string[] fromIndex = animals[2..];          // elephant ... iguana
            string[] fromEnd = animals[^3..];           // giraffe, horse, iguana
            string[] toEnd = animals[2..^1];            // elephant ... horse
            string[] allElements = animals[..];         // full copy

            Console.WriteLine($"From start: [{string.Join(", ", fromStart)}]");
            Console.WriteLine($"From index 2: [{string.Join(", ", fromIndex)}]");
            Console.WriteLine($"From end: [{string.Join(", ", fromEnd)}]");
            Console.WriteLine($"To end minus 1: [{string.Join(", ", toEnd)}]");
            Console.WriteLine($"All elements: [{string.Join(", ", allElements)}]");

            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Range firstHalf = 0..5;
            Range secondHalf = 5..10;
            Range middleSection = 2..8;
            Range lastFew = ^3..;

            int[] part1 = numbers[firstHalf];    // 0, 1, 2, 3, 4
            int[] part2 = numbers[secondHalf];   // 5, 6, 7, 8, 9
            int[] middle = numbers[middleSection]; // 2, 3, 4, 5, 6, 7
            int[] ending = numbers[lastFew];     // 7, 8, 9

            Console.WriteLine($"First half: [{string.Join(", ", part1)}]");
            Console.WriteLine($"Second half: [{string.Join(", ", part2)}]");
            Console.WriteLine($"Middle: [{string.Join(", ", middle)}]");
            Console.WriteLine($"Last few: [{string.Join(", ", ending)}]");

            var (start, length) = firstHalf.GetOffsetAndLength(numbers.Length);
            Console.WriteLine($"First half - Start: {start}, Length: {length}");
        }
    }

    /*

    //1. List<T> supports ^ indexing but NOT range slicing (list[1..3] doesn't compile);
    //   use list.GetRange(...) or CollectionsMarshal.AsSpan(list)[1..3].

    //2. In hot paths prefer slicing a Span<T> (no copy) over slicing an array (allocates).

     */
}
