// =============================================================================
//  Feature     : Generalized Async Return Types (ValueTask<T>)
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    Async methods may return types other than Task/Task<T> - most importantly
//    ValueTask<T>, a STRUCT. Every 'async Task<T>' method allocates a Task object
//    on the heap, even when the result is already available (e.g. from a cache).
//    ValueTask<T> wraps EITHER a plain result (no allocation) OR a real Task (when
//    the work is truly asynchronous), cutting garbage on hot paths. It comes with
//    stricter usage rules than Task: await it exactly once.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class ValueTaskDemo
    {
        public static async Task Run()
        {
            await DemonstrateCachedLookup();
            await DemonstrateRules();
        }

        // TYPICAL SCENARIO: A CACHE IN FRONT OF A SLOW ASYNC SOURCE
        // - GetPriceTaskAsync (Task<T>): even a cache hit returns a NEW Task<decimal> object.
        // - GetPriceAsync (ValueTask<T>):
        //     * cache hit  -> new ValueTask<decimal>(cached): just a struct holding the value,
        //                     completes synchronously with ZERO heap allocations.
        //     * cache miss -> new ValueTask<decimal>(task): wraps the real Task doing the I/O.
        // - LoadFromDatabaseAsync simulates I/O and fills the cache.
        public class PriceService
        {
            private readonly Dictionary<string, decimal> _cache = new();
            public int DatabaseCalls { get; private set; }

            public async Task<decimal> GetPriceTaskAsync(string sku)
            {
                if (_cache.TryGetValue(sku, out var cached))
                    return cached;
                return await LoadFromDatabaseAsync(sku);
            }

            public ValueTask<decimal> GetPriceAsync(string sku)
            {
                if (_cache.TryGetValue(sku, out var cached))
                    return new ValueTask<decimal>(cached);

                return new ValueTask<decimal>(LoadFromDatabaseAsync(sku));
            }

            private async Task<decimal> LoadFromDatabaseAsync(string sku)
            {
                DatabaseCalls++;
                await Task.Delay(50);
                decimal price = sku.Length * 10m;
                _cache[sku] = price;
                return price;
            }
        }

        // OBSERVING THE FAST PATH
        // - The first call does real async work (IsCompletedSuccessfully = False).
        // - Later calls hit the cache and are already complete (True) - no allocation.
        // - Only ONE database call happens in total.
        // - 'async ValueTask<T>' methods (DoubleAsync) are also allowed; the compiler builds the
        //   appropriate state machine.
        public static async Task DemonstrateCachedLookup()
        {
            var service = new PriceService();

            for (int i = 0; i < 3; i++)
            {
                ValueTask<decimal> pending = service.GetPriceAsync("KEYBOARD");
                Console.WriteLine($"Call {i + 1}: completed synchronously = {pending.IsCompletedSuccessfully}");
                decimal price = await pending;
                Console.WriteLine($"  Price = {price}");
            }
            // Call 1: completed synchronously = False
            // Call 2: completed synchronously = True
            // Call 3: completed synchronously = True

            Console.WriteLine($"Database calls: {service.DatabaseCalls}");   // 1

            decimal viaTask = await service.GetPriceTaskAsync("KEYBOARD");
            Console.WriteLine($"Task<T> version also works: {viaTask}");

            Console.WriteLine($"Async ValueTask method: {await DoubleAsync(21)}");
        }

        static async ValueTask<int> DoubleAsync(int x)
        {
            await Task.Yield();
            return x * 2;
        }

        // THE RULES - ValueTask is NOT a drop-in replacement for Task
        // - CORRECT: await it exactly once, directly.
        // - Need to await more than once or use Task.WhenAll/WhenAny? Convert with AsTask() first;
        //   a Task CAN safely be awaited many times.
        // - WRONG with a ValueTask:
        //     await vt; await vt;    // awaiting twice - undefined behaviour
        //     vt.Result              // blocking on an incomplete ValueTask - not allowed
        //     Task.WhenAll(vt, vt2)  // doesn't accept ValueTask - use AsTask()
        public static async Task DemonstrateRules()
        {
            var service = new PriceService();

            decimal price = await service.GetPriceAsync("MOUSE");
            Console.WriteLine($"Awaited once: {price}");

            Task<decimal> asTask = service.GetPriceAsync("MONITOR").AsTask();
            await Task.WhenAll(asTask, Task.Delay(1));
            Console.WriteLine($"Via AsTask: {await asTask}");
        }
    }

    /*

    When to use which:
    - Default to Task / Task<T>. It's simpler and safe to await multiple times.
    - Use ValueTask<T> when a method OFTEN completes synchronously (caches, buffered streams)
      AND it's on a hot path where allocations matter. .NET itself uses it for Stream.ReadAsync,
      Channel reads, IAsyncEnumerator.MoveNextAsync (see 8_AsyncStreams.cs) and IAsyncDisposable.

    Other C# 7.x async improvements:
    - C# 7.1 'async Task Main' - your Program.cs uses it!
    - Any type with an [AsyncMethodBuilder] attribute can be an async return type.

     */
}
