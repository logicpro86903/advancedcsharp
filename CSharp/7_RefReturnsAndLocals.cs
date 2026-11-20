using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class RefReturnsAndLocals
    {
        private static int[] _numbers = { 1, 2, 3, 4, 5 };

        public static void Run()
        {
            DemonstrateRefReturn();
            ModifyLargestElement();
        }
               

        // Method returning a reference to an array element
        public static ref int GetElement(int index)
        {
            if (index < 0 || index >= _numbers.Length)
                throw new IndexOutOfRangeException();

            return ref _numbers[index]; // Return reference, not copy
        }

        public static void DemonstrateRefReturn()
        {
            // Get reference to array element
            ref int element = ref GetElement(2);

            Console.WriteLine($"Original value: {element}"); // 3

            // Modify through reference
            element = 99;

            Console.WriteLine($"After modification: {_numbers[2]}"); // 99
            Console.WriteLine($"Array: [{string.Join(", ", _numbers)}]"); // [1, 2, 99, 4, 5]
        }

        // Ref return with conditional logic
        public static ref int GetLargestElement()
        {
            int maxIndex = 0;
            for (int i = 1; i < _numbers.Length; i++)
            {
                if (_numbers[i] > _numbers[maxIndex])
                    maxIndex = i;
            }
            return ref _numbers[maxIndex];
        }

        public static void ModifyLargestElement()
        {
            ref int largest = ref GetLargestElement();
            Console.WriteLine($"Largest element: {largest}");

            largest *= 2; // Modify the actual array element
            Console.WriteLine($"After doubling: [{string.Join(", ", _numbers)}]");
        }

    }
}


/*
 // Ref returns and locals, introduced in C# 7.0, allow methods to return references to variables 
// rather than copies of values, and enable local variables to store references. 
//This provides significant performance benefits by avoiding unnecessary copying, 
//especially for large value types.
 */
