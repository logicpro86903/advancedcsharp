// =============================================================================
//  Feature     : Inline Arrays ([InlineArray(N)])
//  C# Version  : C# 12.0
//  .NET        : .NET 8
//  Released    : 2023 (Visual Studio 2022 v17.8)
// -----------------------------------------------------------------------------
//  Summary:
//    A struct marked [InlineArray(N)] with exactly ONE field becomes a
//    fixed-size buffer of N elements stored INLINE - inside the struct itself,
//    with no separate heap-allocated array. It can be indexed like an array,
//    converted to Span<T>, and iterated with foreach. It is the safe, generic
//    replacement for 'unsafe fixed' buffers and is mainly used by runtime and
//    library authors for high-performance, allocation-free code.
// =============================================================================

using System.Runtime.CompilerServices;

namespace CSharp
{
    public class InlineArrays
    {
        // DECLARING INLINE ARRAYS
        // - [InlineArray(4)] tells the runtime to repeat the single field 4 times in memory.
        // - The struct must declare exactly ONE instance field; its type is the element type
        //   and its name doesn't matter (_element0 by convention).
        // - Any element type works (int, byte, float, even reference types) - unlike
        //   'fixed' buffers, which only allowed primitive types in unsafe code.
        [InlineArray(4)]
        public struct IntArray4
        {
            private int _element0;
        }

        [InlineArray(8)]
        public struct ByteArray8
        {
            private byte _element0;
        }

        [InlineArray(3)]
        public struct FloatArray3
        {
            private float _element0;
        }

        // USING INLINE ARRAYS
        // - 'new IntArray4()' allocates nothing on the heap: the 4 ints live inside the struct
        //   (on the stack here, or inline inside whatever object contains it).
        // - Indexing with [i] is bounds-checked, just like a normal array.
        // - Because they are structs, assigning one to another variable COPIES all elements.
        public static void Run()
        {
            var numbers = new IntArray4();
            numbers[0] = 10;
            numbers[1] = 20;
            numbers[2] = 30;
            numbers[3] = 40;

            Console.WriteLine($"Numbers: [{numbers[0]}, {numbers[1]}, {numbers[2]}, {numbers[3]}]");

            var bytes = new ByteArray8();
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = (byte)(i * 16);
            }

            Console.Write("Bytes: [");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(bytes[i]);
                if (i < 7) Console.Write(", ");
            }
            Console.WriteLine("]");

            var vector = new FloatArray3();
            vector[0] = 1.0f; // X
            vector[1] = 2.0f; // Y
            vector[2] = 3.0f; // Z

            Console.WriteLine($"Vector: ({vector[0]}, {vector[1]}, {vector[2]})");
        }
    }

    /*

    More operations:
        Span<int> span = numbers;           // implicit conversion to Span<T> - use all span APIs
        foreach (int n in numbers) { ... }  // foreach works directly
        // There is no .Length property - the size is the attribute's constant.

    //1. Large inline arrays make the struct large - every copy copies the whole buffer.
    //2. Most application code won't need them; they shine in parsers, networking and interop.

     */
}
