using System;
using System.Threading.Tasks;

namespace CSharp
{
    //.NET FW 4.5 & C#5 2012
    public static class AsyncAwait
    {
        public static async Task RunAsync()
        {
            await SayHiAsync();
            await SayHelloAsync();

            Console.WriteLine("All tasks completed.");
        }

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
    //Async/await is a language feature in C# that simplifies writing asynchronous code, 
    // making it look and behave more like synchronous code while maintaining the benefits 
    // of non-blocking operations.
 */