// =============================================================================
//  Feature     : Span<T> and ref struct
//  C# Version  : C# 7.2
//  .NET        : .NET Core 2.1 (Span<T> shipped in 2018)
//  Released    : 2017 (Visual Studio 2017 v15.5)
// -----------------------------------------------------------------------------
//  Summary:
//    Span<T> and ReadOnlySpan<T> are type-safe "windows" over contiguous memory -
//    an array, part of an array, a string, stack memory or native memory -
//    WITHOUT copying it. Substring(), Split() and array copies allocate new
//    objects; spans let you slice and parse data with ZERO allocations. Spans are
//    'ref struct' types: special structs that can only live on the stack, which
//    the compiler enforces. This is the foundation of modern high-performance
//    .NET (Kestrel, System.Text.Json, span-based Parse/TryFormat APIs).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class SpanAndRefStructs
    {
        public static void Run()
        {
            DemonstrateSlicingArrays();
            DemonstrateStringParsingWithoutAllocations();
            DemonstrateStackalloc();
            DemonstrateRefStructRules();
        }

        // SPAN<T> OVER AN ARRAY: SLICING WITHOUT COPYING
        // - An array converts implicitly to Span<T>.
        // - Slice(start, length) or range syntax (all[^3..], C# 8) creates a VIEW, not a copy.
        // - Writing through a span writes straight into the ORIGINAL array (numbers[3] becomes 300).
        // - Spans offer helpers like Fill, IndexOf, CopyTo, SequenceEqual...
        // - ReadOnlySpan<T> is the same view but forbids writes.
        public static void DemonstrateSlicingArrays()
        {
            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Span<int> all = numbers;
            Span<int> middle = all.Slice(3, 4);
            Span<int> lastThree = all[^3..];

            middle[0] = 300;
            Console.WriteLine($"numbers[3] after writing through the span: {numbers[3]}"); // 300

            Console.WriteLine($"middle:    {string.Join(", ", middle.ToArray())}");    // 300, 4, 5, 6
            Console.WriteLine($"lastThree: {string.Join(", ", lastThree.ToArray())}"); // 7, 8, 9

            middle.Fill(0);
            Console.WriteLine($"After Fill: {string.Join(", ", numbers)}");
            Console.WriteLine($"Index of 8: {all.IndexOf(8)}");                        // 8

            ReadOnlySpan<int> readOnly = numbers;
            // readOnly[0] = 1;                      // compile error
            Console.WriteLine($"Sum of first 3 via ReadOnlySpan: {Sum(readOnly[..3])}");
        }

        // Accepting ReadOnlySpan<T> makes ONE method work with arrays, slices, stack memory, etc.
        static int Sum(ReadOnlySpan<int> values)
        {
            int total = 0;
            foreach (int v in values) total += v;
            return total;
        }

        // READONLYSPAN<CHAR>: PARSING STRINGS WITH ZERO ALLOCATIONS
        // - line.Split(',') allocates an array plus a new string per part.
        // - AsSpan() + IndexOf + slicing just indexes into the ORIGINAL string - no new strings.
        // - Most parse methods (DateTime.Parse, decimal.Parse, int.Parse...) accept
        //   ReadOnlySpan<char> directly.
        // - Only call .ToString() on a span when you truly need a string - that's where it allocates.
        public static void DemonstrateStringParsingWithoutAllocations()
        {
            string line = "2024-11-19,ORDER-1042,149.99";

            string[] parts = line.Split(',');
            Console.WriteLine($"Split approach: {parts[1]}");

            ReadOnlySpan<char> span = line.AsSpan();

            int firstComma = span.IndexOf(',');
            ReadOnlySpan<char> datePart = span[..firstComma];
            ReadOnlySpan<char> rest = span[(firstComma + 1)..];

            int secondComma = rest.IndexOf(',');
            ReadOnlySpan<char> orderId = rest[..secondComma];
            ReadOnlySpan<char> amountPart = rest[(secondComma + 1)..];

            DateTime date = DateTime.Parse(datePart);
            decimal amount = decimal.Parse(amountPart, System.Globalization.CultureInfo.InvariantCulture);

            Console.WriteLine($"Span approach: date={date:yyyy-MM-dd}, id={orderId.ToString()}, amount={amount}");
            Console.WriteLine($"Starts with ORDER: {orderId.StartsWith("ORDER")}");    // True
        }

        // STACKALLOC: TEMPORARY BUFFERS ON THE STACK
        // - Before Span, stackalloc required 'unsafe' code and pointers. Assigning it to a
        //   Span<T> makes it SAFE (bounds-checked) code.
        // - Stack memory is freed automatically when the method returns - no GC involved.
        // - Common pattern: small sizes on the stack, larger sizes fall back to a heap array.
        // - TryFormat writes a number into a char buffer without creating a string.
        public static void DemonstrateStackalloc()
        {
            Span<int> squares = stackalloc int[5];
            for (int i = 0; i < squares.Length; i++)
                squares[i] = i * i;
            Console.WriteLine($"Squares: {string.Join(", ", squares.ToArray())}");     // 0, 1, 4, 9, 16

            int size = 64;
            Span<byte> buffer = size <= 256 ? stackalloc byte[size] : new byte[size];
            buffer[0] = 0xFF;
            Console.WriteLine($"Buffer length: {buffer.Length}, first byte: {buffer[0]}");

            Span<char> chars = stackalloc char[16];
            if (12345.TryFormat(chars, out int written))
                Console.WriteLine($"Formatted {written} chars: {chars[..written].ToString()}");
        }

        // WRITING YOUR OWN REF STRUCT
        // - 'ref struct' declares a struct that can ONLY live on the stack - just like Span<T>.
        // - Only a ref struct is allowed to contain a Span field (_remaining).
        // - WordReader walks through text word by word, handing out slices of the original
        //   text instead of allocating substrings.
        public ref struct WordReader
        {
            private ReadOnlySpan<char> _remaining;

            public WordReader(ReadOnlySpan<char> text) { _remaining = text; }

            public bool TryReadNext(out ReadOnlySpan<char> word)
            {
                _remaining = _remaining.TrimStart();
                if (_remaining.IsEmpty)
                {
                    word = default;
                    return false;
                }
                int space = _remaining.IndexOf(' ');
                word = space < 0 ? _remaining : _remaining[..space];
                _remaining = space < 0 ? ReadOnlySpan<char>.Empty : _remaining[space..];
                return true;
            }
        }

        // REF STRUCT RULES (enforced by the compiler, because they may point at stack memory)
        // - can't be a field of a class or a normal struct      class C { Span<int> s; }
        // - can't be boxed / assigned to object or an interface  object o = span;
        // - can't be captured by lambdas                         () => span.Length
        // - can't be used across 'await' in async methods        (C# 13 relaxed this a bit)
        // - can't be a generic type argument                     List<Span<int>>
        //   (C# 13 added 'allows ref struct' to opt in for generics)
        // Need to store it on the heap or across an await? Use Memory<T> / ReadOnlyMemory<T>,
        // and call .Span when you're ready to work on the data.
        public static void DemonstrateRefStructRules()
        {
            var reader = new WordReader("  the quick   brown fox ");
            int count = 0;
            while (reader.TryReadNext(out var word))
            {
                Console.Write($"[{word.ToString()}] ");
                count++;
            }
            Console.WriteLine($"-> {count} words, no substrings allocated");

            Memory<int> memory = new int[] { 1, 2, 3 };
            Span<int> fromMemory = memory.Span;
            Console.WriteLine($"Memory<T> -> Span<T>: {fromMemory.Length} items");
        }
    }

    /*

    Cheat sheet:
        Span<T>              read/write view, stack only (ref struct)
        ReadOnlySpan<T>      read-only view, stack only - use for method PARAMETERS (most flexible)
        Memory<T>            like Span but a normal struct - can be stored in fields, used across await
        ReadOnlyMemory<T>    read-only version of Memory<T>
        stackalloc           allocate a small temporary buffer on the stack

    //1. Don't stackalloc large or user-controlled sizes - the stack is small (~1 MB) -> StackOverflow.
    //2. A span over a List<T> (CollectionsMarshal.AsSpan) becomes invalid if the list grows.
    //3. You don't need spans everywhere - use them in hot paths (parsing, serialization, buffers).

    Related: 7_RefReturnsAndLocals.cs, 7_ReadOnlyStructsAndInParams.cs, 12_InlineArrays.cs,
    C# 13 'allows ref struct', C# 14 implicit span conversions (arrays convert to spans automatically
    in more places, e.g. extension methods on ReadOnlySpan<T> can be called on arrays).

     */
}
