// =============================================================================
//  Feature     : Collection Expressions ([ ... ] and the spread element ..)
//  C# Version  : C# 12.0
//  .NET        : .NET 8
//  Released    : 2023 (Visual Studio 2022 v17.8)
// -----------------------------------------------------------------------------
//  Summary:
//    A single, uniform syntax creates almost any collection: [1, 2, 3]. The
//    TARGET type decides what gets built - array, List<T>, HashSet<T>, Span<T>,
//    ImmutableArray<T>, interfaces like IEnumerable<T>... - so you never repeat
//    the type. [] is an empty collection, and the SPREAD element '..source'
//    inlines all items of another collection or sequence. It replaces
//    'new int[] { ... }', 'new List<int> { ... }', Array.Empty<T>() and
//    manual Concat/AddRange calls, and the compiler picks an efficient way to build it.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class CollectionExpressions
    {
        public static void Run()
        {
            // BEFORE C# 12 (for comparison):
            //   int[] numbers = new int[] { 1, 2, 3, 4, 5 };   or   int[] numbers = { 1, 2, 3, 4, 5 };
            //   List<string> names = new List<string> { "Alice", "Bob" };

            // CREATING COLLECTIONS
            // - The same [ ... ] syntax builds arrays, List<T>, HashSet<T> and more; the declared
            //   (target) type decides which one.
            // - '[]' creates an EMPTY collection of the target type (for arrays it reuses a cached
            //   empty instance, like Array.Empty<T>()).
            // - Queue<T> and Stack<T> can't be targets: they have no Add method or
            //   [CollectionBuilder] attribute (Stack would also reverse the order).
            int[] numbers = [1, 2, 3, 4, 5];
            string[] names = ["Alice", "Bob", "Charlie"];
            double[] values = [3.14, 2.71, 1.41];

            List<int> numbersList = [10, 20, 30, 40];
            List<string> cities = ["New York", "London", "Tokyo"];

            HashSet<string> uniqueNames = ["John", "Jane", "Bob", "Alice"];

            int[] emptyArray = [];
            List<string> emptyList = [];
            HashSet<int> emptySet = [];

            Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Names: [{string.Join(", ", names)}]");
            Console.WriteLine($"Cities count: {cities.Count}");
            Console.WriteLine($"Unique names: {uniqueNames.Count}");

            // THE SPREAD ELEMENT (..)
            // - '..first' inserts every element of 'first' at that position.
            // - Spreads and single values can be mixed freely, in any order:
            //   [0, .. first, 10, .. second, 20, .. third, 30].
            // - Source and target types don't have to match: arrays, lists and sets can all be
            //   spread into a new array.
            int[] first = [1, 2, 3];
            int[] second = [4, 5, 6];
            List<int> third = [7, 8, 9];

            int[] combined = [.. first, .. second, .. third];
            int[] withExtra = [0, .. first, 10, .. second, 20, .. third, 30];

            Console.WriteLine($"Combined: [{string.Join(", ", combined)}]");
            Console.WriteLine($"With extra: [{string.Join(", ", withExtra)}]");

            string[] fruits = ["apple", "banana"];
            List<string> vegetables = ["carrot", "broccoli"];
            HashSet<string> grains = ["rice", "wheat"];

            string[] allFoods = [.. fruits, .. vegetables, .. grains, "milk", "cheese"];
            Console.WriteLine($"All foods: [{string.Join(", ", allFoods)}]");

            // SPREADING LINQ RESULTS
            // - Any IEnumerable<T> can be spread, including lazy LINQ queries; the query is
            //   enumerated while the new collection is built.
            // - '[.. query]' is a concise alternative to query.ToArray() / ToList().
            var evenNumbers = Enumerable.Range(1, 10).Where(x => x % 2 == 0);
            var oddNumbers = Enumerable.Range(1, 10).Where(x => x % 2 == 1);

            int[] allNumbers = [.. evenNumbers, .. oddNumbers];
            int[] sortedNumbers = [.. allNumbers.OrderBy(x => x)];

            Console.WriteLine($"All numbers: [{string.Join(", ", allNumbers)}]");
            Console.WriteLine($"Sorted: [{string.Join(", ", sortedNumbers)}]");
        }
    }

    /*

    Other targets:
        Span<int> span = [1, 2, 3];                   // may be stack-allocated - no heap at all
        IEnumerable<int> seq = [1, 2, 3];             // compiler picks a suitable read-only type
        ImmutableArray<int> imm = [1, 2, 3];          // types with [CollectionBuilder] support it

    //1. 'var x = [1, 2, 3];' doesn't compile - there is no target type to decide what to build.
    //2. Pairs nicely with params collections (C# 13, see 13_ParamsCollectioncs.cs).

     */
}
