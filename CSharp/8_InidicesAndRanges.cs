using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class IndicesAndRanges
    {
        public static void Run()
        {
            DemonstrateBasicIndexing();
            DemonstrateBasicRanges();
        }

        public static void DemonstrateBasicIndexing()
        {
            string[] fruits = { "apple", "banana", "cherry", "date", "elderberry" };

            // Traditional indexing (from start)
            Console.WriteLine($"First: {fruits[0]}");           // apple
            Console.WriteLine($"Second: {fruits[1]}");          // banana
            Console.WriteLine($"Last: {fruits[fruits.Length - 1]}"); // elderberry

            // Index from end using ^ operator
            Console.WriteLine($"Last: {fruits[^1]}");           // elderberry
            Console.WriteLine($"Second to last: {fruits[^2]}"); // date
            Console.WriteLine($"Third to last: {fruits[^3]}");  // cherry

            // ^0 is equivalent to Length (one past the end)
            // fruits[^0] would throw an IndexOutOfRangeException

            // Working with different collection types
            var numbers = new List<int> { 10, 20, 30, 40, 50 };
            Console.WriteLine($"Last number: {numbers[^1]}");    // 50
            Console.WriteLine($"Second to last: {numbers[^2]}"); // 40

            string text = "Hello World";
            Console.WriteLine($"Last character: {text[^1]}");    // 'd'
            Console.WriteLine($"Second to last: {text[^2]}");    // 'l'


            int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Index can be stored in a variable
            Index lastIndex = ^1;
            Index secondLastIndex = ^2;

            Console.WriteLine($"Last value: {values[lastIndex]}");       // 10
            Console.WriteLine($"Second last: {values[secondLastIndex]}"); // 9
        }


        public static void DemonstrateBasicRanges()
        {
            string[] animals = { "cat", "dog", "elephant", "fish", "giraffe", "horse", "iguana" };

            // Traditional slicing (manually creating new arrays)
            string[] traditionalSlice = new string[3];
            Array.Copy(animals, 1, traditionalSlice, 0, 3);

            // Range slicing - much simpler!
            string[] firstThree = animals[0..3];        // cat, dog, elephant
            string[] middleThree = animals[2..5];       // elephant, fish, giraffe
            string[] lastThree = animals[^3..^0];       // giraffe, horse, iguana

            Console.WriteLine($"First three: [{string.Join(", ", firstThree)}]");
            Console.WriteLine($"Middle three: [{string.Join(", ", middleThree)}]");
            Console.WriteLine($"Last three: [{string.Join(", ", lastThree)}]");

            // Various range patterns
            string[] fromStart = animals[..3];          // First 3: cat, dog, elephant
            string[] fromIndex = animals[2..];          // From index 2 to end
            string[] fromEnd = animals[^3..];           // Last 3 elements
            string[] toEnd = animals[2..^1];            // From index 2, excluding last
            string[] allElements = animals[..];         // All elements (copy)

            Console.WriteLine($"From start: [{string.Join(", ", fromStart)}]");
            Console.WriteLine($"From index 2: [{string.Join(", ", fromIndex)}]");
            Console.WriteLine($"From end: [{string.Join(", ", fromEnd)}]");
            Console.WriteLine($"To end minus 1: [{string.Join(", ", toEnd)}]");
            Console.WriteLine($"All elements: [{string.Join(", ", allElements)}]");



            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Range can be stored in a variable
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

            // Range information
            var (start, length) = firstHalf.GetOffsetAndLength(numbers.Length);
            Console.WriteLine($"First half - Start: {start}, Length: {length}");


        }


    }

    /*
     //Indices and ranges, introduced in C# 8.0, provide a more intuitive and concise way to access elements 
    //and slices of arrays, lists, and other indexable collections. The hat operator (^) 
    //creates indices from the end, while the range operator (..) creates ranges for slicing.
     */
}
