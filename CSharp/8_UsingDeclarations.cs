// =============================================================================
//  Feature     : using Declarations and await using
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    'using var x = ...;' is a using statement WITHOUT braces: the object is
//    disposed automatically at the end of the ENCLOSING SCOPE (usually the
//    method). Classic using blocks (see 1_UsingStatement.cs) cause deep nesting
//    when several resources are needed; using declarations keep code flat.
//    C# 8 also added 'await using' for IAsyncDisposable (asynchronous cleanup)
//    and pattern-based Dispose for ref structs.
// =============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class UsingDeclarations
    {
        public static async Task Run()
        {
            DemonstrateBeforeAndAfter();
            DemonstrateDisposeOrder();
            await DemonstrateAwaitUsing();
        }

        // A disposable resource that logs when it is opened, used and closed.
        public class Resource : IDisposable
        {
            private readonly string _name;
            public Resource(string name) { _name = name; Console.WriteLine($"  open  {_name}"); }
            public void Use() => Console.WriteLine($"  use   {_name}");
            public void Dispose() => Console.WriteLine($"  close {_name}");
        }

        // BEFORE vs AFTER
        // - Classic: each resource needs its own using block, so three resources means three
        //   levels of nesting (the "pyramid of doom").
        // - Using declarations (ReadWithDeclarations): one flat line per resource.
        // - In both cases resources are disposed in REVERSE order of creation:
        //   reader, then command, then connection.
        public static void DemonstrateBeforeAndAfter()
        {
            Console.WriteLine("Classic using blocks (nested):");
            using (var connection = new Resource("connection"))
            {
                using (var command = new Resource("command"))
                {
                    using (var reader = new Resource("reader"))
                    {
                        reader.Use();
                    }
                }
            }

            Console.WriteLine("Using declarations (flat):");
            ReadWithDeclarations();
        }

        // All three are disposed at the END OF THIS METHOD, in reverse order.
        static void ReadWithDeclarations()
        {
            using var connection = new Resource("connection");
            using var command = new Resource("command");
            using var reader = new Resource("reader");
            reader.Use();
        }

        // SCOPE CONTROLS WHEN DISPOSE RUNS
        // - A using declaration is disposed when its enclosing BLOCK ends - an inner { } block,
        //   an if-block, a loop body, or the method.
        // - Use an inner block to release a resource early ('temp' closes before "...after inner block").
        // - Real-world example: WriteFile's StreamWriter is disposed (flushed and closed) when
        //   WriteFile returns, so the file can be read immediately afterwards.
        public static void DemonstrateDisposeOrder()
        {
            Console.WriteLine("Scope matters:");
            {
                using var temp = new Resource("temp");
                temp.Use();
            }
            Console.WriteLine("  ...after inner block");

            if (DateTime.Now.Year > 2000)
            {
                using var scoped = new Resource("if-scoped");
            }

            string path = Path.Combine(Path.GetTempPath(), "using-demo.txt");
            WriteFile(path);
            Console.WriteLine($"  File says: {File.ReadAllText(path)}");
            File.Delete(path);
        }

        static void WriteFile(string path)
        {
            using var writer = new StreamWriter(path);
            writer.Write("hello from a using declaration");
        }

        // IAsyncDisposable: cleanup that itself needs to await (e.g. flushing to network or disk).
        public class AsyncResource : IAsyncDisposable
        {
            public async ValueTask DisposeAsync()
            {
                await Task.Delay(10);
                Console.WriteLine("  async resource disposed");
            }
        }

        // AWAIT USING
        // - 'await using' calls and AWAITS DisposeAsync() instead of calling Dispose().
        // - Both forms exist: declaration ('await using var x = ...;') and block
        //   ('await using (var x = ...) { }').
        // - 'block' is disposed at the end of its braces; 'resource' at the end of the method,
        //   after "end of method" prints.
        public static async Task DemonstrateAwaitUsing()
        {
            Console.WriteLine("await using:");
            await using var resource = new AsyncResource();
            await using (var block = new AsyncResource())
            {
                Console.WriteLine("  inside block");
            }
            Console.WriteLine("  end of method");
        }
    }

    /*

    //1. The resource lives until the END OF THE SCOPE, not until its last use. If you need it released
    //   early (e.g. a file lock, a DB connection in a long method), use a block or an inner { } scope.

    //2. You can't reassign a using variable:  connection = new Resource("x");  // compile error

    //3. Must be initialized in the declaration - no 'using var x;'

    Also new in C# 8: a ref struct (which can't implement interfaces) can be used in 'using' if it simply
    has a public void Dispose() method - "pattern-based" disposal.

     */
}
