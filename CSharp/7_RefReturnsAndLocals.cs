// =============================================================================
//  Feature     : ref Returns and ref Locals
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    A method can return a REFERENCE to a variable (e.g. an array element or a
//    field) instead of a copy of its value, and a 'ref' local can hold such a
//    reference. Writing through the ref local changes the original storage
//    directly. This avoids copying large structs and lets callers modify an
//    element "in place" once it has been located - a building block for
//    high-performance code such as Span<T>.
// =============================================================================

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

        // REF RETURN
        // - 'ref int' as the return type means the method returns a reference to an int variable.
        // - 'return ref _numbers[index];' returns a reference to the array slot itself, not a copy
        //   of its value.
        // - You may only return references to storage that outlives the method (array elements,
        //   fields, ref parameters) - never to a local variable.
        public static ref int GetElement(int index)
        {
            if (index < 0 || index >= _numbers.Length)
                throw new IndexOutOfRangeException();

            return ref _numbers[index];
        }

        // REF LOCAL
        // - 'ref int element = ref GetElement(2);' makes 'element' an ALIAS for _numbers[2].
        //   Both 'ref' keywords are required; without them you'd get an ordinary copy.
        // - Assigning element = 99 writes directly into the array.
        public static void DemonstrateRefReturn()
        {
            ref int element = ref GetElement(2);

            Console.WriteLine($"Original value: {element}"); // 3

            element = 99;

            Console.WriteLine($"After modification: {_numbers[2]}"); // 99
            Console.WriteLine($"Array: [{string.Join(", ", _numbers)}]"); // [1, 2, 99, 4, 5]
        }

        // FIND-THEN-MODIFY PATTERN
        // - The method does the search and returns a reference to the winning slot.
        // - The caller can then update that slot in place without knowing its index -
        //   'largest *= 2' modifies the array element itself.
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

            largest *= 2;
            Console.WriteLine($"After doubling: [{string.Join(", ", _numbers)}]");
        }
    }
}


/*

    //1. 'var x = GetElement(2);' (no 'ref') silently makes a COPY - changes won't reach the array.
    //2. Later: 'ref readonly' returns (C# 7.2), ref reassignment 'r = ref other;' (C# 7.3),
    //   and ref fields inside ref structs (C# 11).

 */
