using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class AsyncStreams
    {
        public async static Task Run()
        {
            await DemonstrateBasicUsage();
        }

        public static async Task DemonstrateBasicUsage()
        {
            Console.WriteLine("Basic async enumeration:");

            // Using await foreach to consume async stream
            await foreach (int number in GenerateNumbersAsync(5))
            {
                Console.WriteLine($"Received number: {number}");
            }
        }

        public static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // Simulate async work
                await Task.Delay(1000);
                yield return i;
            }
        }
    }

    /*
     //Async streams, introduced in C# 8.0, allow you to asynchronously iterate over collections 
    that are produced asynchronously. They combine the power of async/await 
    with the iterator pattern, enabling you to process data as it becomes available without blocking.
     */
}
