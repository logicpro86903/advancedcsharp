// =============================================================================
//  Feature     : params Collections
//  C# Version  : C# 13.0
//  .NET        : .NET 9
//  Released    : 2024 (Visual Studio 2022 v17.12)
// -----------------------------------------------------------------------------
//  Summary:
//    The 'params' modifier is no longer limited to arrays. It can now be applied
//    to any type usable with collection expressions (C# 12): Span<T>,
//    ReadOnlySpan<T>, IEnumerable<T>, IReadOnlyList<T>, List<T> and more. The
//    big win is 'params ReadOnlySpan<T>': callers still write Print("a", "b"),
//    but the arguments can be passed WITHOUT allocating an array on the heap.
//    .NET 9 added such overloads to many APIs (string.Join, Path.Combine, Task.WhenAll...).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ParamsCollectioncs
    {
        // CALLING A params COLLECTION METHOD
        // - The call site looks exactly like a classic params call: comma-separated arguments.
        // - The compiler builds the parameter's collection type from them (here an IEnumerable<string>).
        public void Run()
        {
            Print1("one", "two", "three");
        }

        // BEFORE C# 13: params had to be an ARRAY, allocated on the heap on every call:
        //     void Print(params string[] items) { }

        // C# 13 params COLLECTION TYPES
        // - params IEnumerable<string> / List<string> : any collection-expression-compatible type.
        // - params Span<string> / ReadOnlySpan<string> : arguments can be placed on the STACK -
        //   no array allocation. ReadOnlySpan<T> is the preferred choice for new APIs.
        // - When overloads exist for both an array and a span, the span overload is preferred.
        void Print1(params IEnumerable<string> items) { }
        void Print2(params List<string> items) { }
        void Print(params Span<string> items) { }
        void Print(params ReadOnlySpan<string> items) { }
    }

    /*

    //1. Just like before, params must be the LAST parameter and only one is allowed.
    //2. A params ReadOnlySpan<T> can't be captured beyond the call (it's a ref struct) - copy it
    //   (e.g. items.ToArray()) if you need to keep the values.

     */
}
