// =============================================================================
//  Feature     : async / await
//  C# Version  : C# 5.0
//  .NET        : .NET Framework 4.5
//  Released    : 2012 (Visual Studio 2012)
// -----------------------------------------------------------------------------
//  Summary:
//    'async' and 'await' let you write asynchronous, non-blocking code that reads
//    like ordinary sequential code. An 'async' method returns a Task (or Task<T>);
//    'await' pauses the method until the awaited task completes WITHOUT blocking
//    the thread, then resumes where it left off. The compiler rewrites the method
//    into a state machine, replacing callback-based code. Essential for responsive
//    UIs and scalable servers doing I/O (HTTP, database, file access).
// =============================================================================

using System;
using System.Threading.Tasks;

namespace CSharp
{
    public static class AsyncAwait
    {
        // AWAITING TASKS SEQUENTIALLY
        // - 'async Task' marks an asynchronous method that returns no value (Task<T> returns a T).
        // - 'await SayHiAsync()' starts the operation and suspends RunAsync until it finishes;
        //   the thread is released to do other work in the meantime.
        // - The awaits run ONE AFTER ANOTHER, so total time is ~2 seconds. To run them
        //   concurrently, start both tasks first and then 'await Task.WhenAll(t1, t2)'.
        public static async Task RunAsync()
        {
            await SayHiAsync();
            await SayHelloAsync();

            Console.WriteLine("All tasks completed.");
        }

        // ASYNC METHODS
        // - Task.Delay(1000) is the async equivalent of Thread.Sleep(1000): it completes after
        //   one second without blocking a thread while waiting.
        // - Code before the first await runs synchronously on the caller's thread.
        private static async Task SayHiAsync()
        {
            Console.WriteLine("Hi");
            await Task.Delay(1000);
        }

        private static async Task SayHelloAsync()
        {
            Console.WriteLine("Hello");
            await Task.Delay(1000);
        }
    }
}

/*

    //1. Avoid 'async void' except for event handlers - exceptions can't be awaited/caught.
    //2. Don't block on async code with .Result or .Wait() - it can deadlock (especially in UI apps).
    //3. Async all the way: once a method is async, its callers should usually await it too.

    Related: ValueTask (7_ValueTask.cs), async Main (C# 7.1), async streams (8_AsyncStreams.cs).

 */
