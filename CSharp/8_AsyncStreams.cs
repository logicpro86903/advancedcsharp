// =============================================================================
//  Feature     : Async Streams (IAsyncEnumerable<T> and await foreach)
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    Async streams combine iterators (yield return) with async/await. A method
//    returning IAsyncEnumerable<T> can 'await' between items, and consumers use
//    'await foreach' to process each item AS SOON AS it is produced - without
//    blocking a thread and without waiting for the whole collection. Ideal for
//    paged API results, database rows, message queues, file lines and
//    real-time feeds.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class AsyncStreams
    {
        public async static Task Run()
        {
            await DemonstrateBasicUsage();
        }

        // CONSUMING AN ASYNC STREAM WITH 'await foreach'
        // - Each iteration asynchronously awaits the next item (MoveNextAsync under the hood).
        // - Items are printed one per second as they arrive - the consumer doesn't wait for
        //   all 5 to be generated first.
        public static async Task DemonstrateBasicUsage()
        {
            Console.WriteLine("Basic async enumeration:");

            await foreach (int number in GenerateNumbersAsync(5))
            {
                Console.WriteLine($"Received number: {number}");
            }
        }

        // PRODUCING AN ASYNC STREAM (ASYNC ITERATOR)
        // - Return type IAsyncEnumerable<T> + the 'async' modifier + 'yield return'.
        // - The method can 'await' real asynchronous work (here Task.Delay simulates I/O)
        //   between yields.
        // - Like normal iterators it is LAZY: nothing runs until someone enumerates it.
        public static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(1000);
                yield return i;
            }
        }
    }

    /*

    //1. Support cancellation by adding a parameter marked [EnumeratorCancellation]:
    //       async IAsyncEnumerable<int> Gen([EnumeratorCancellation] CancellationToken ct = default)
    //   and pass the token with:  await foreach (var x in Gen().WithCancellation(token))

    //2. LINQ operators for IAsyncEnumerable are built into .NET 10 (System.Linq.AsyncEnumerable);
    //   earlier versions used the System.Linq.Async NuGet package.

    EF Core exposes queries as async streams: await foreach (var row in db.Orders.AsAsyncEnumerable())

     */
}
