// =============================================================================
//  Feature     : using Statement and IDisposable
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    The garbage collector frees memory, but not files, DB connections, sockets
//    or other limited resources. Types holding such resources implement
//    IDisposable (a single Dispose() method). The 'using' statement guarantees
//    Dispose() is called when the block ends - even if an exception is thrown -
//    by compiling into a try/finally for you.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class UsingStatement
    {
        public static void Run()
        {
            DemonstrateUsing();
            DemonstrateWhatUsingCompilesTo();
        }

        // IMPLEMENTING IDisposable
        // - Dispose() releases the resource ("Closing ...").
        // - Dispose should be IDEMPOTENT: safe to call more than once (the _disposed flag).
        // - Using the object after disposal should throw ObjectDisposedException.
        public class DatabaseConnection : IDisposable
        {
            private string _name;
            private bool _disposed;

            public DatabaseConnection(string name)
            {
                _name = name;
                Console.WriteLine($"Opening {_name}");
            }

            public void Query(string sql)
            {
                if (_disposed) throw new ObjectDisposedException(_name);
                Console.WriteLine($"  {_name}: {sql}");
            }

            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;
                Console.WriteLine($"Closing {_name}");
            }
        }

        // THE USING STATEMENT
        // - The resource is created in the parentheses and is in scope only inside the block.
        // - Dispose() is called automatically when the block exits, normally or via exception.
        // - Several using statements can be stacked to share one block; resources are
        //   disposed in REVERSE order of creation (Db-B first, then Db-A).
        public static void DemonstrateUsing()
        {
            using (DatabaseConnection conn = new DatabaseConnection("MainDb"))
            {
                conn.Query("SELECT * FROM Orders");
            }

            using (DatabaseConnection a = new DatabaseConnection("Db-A"))
            using (DatabaseConnection b = new DatabaseConnection("Db-B"))
            {
                a.Query("SELECT 1");
                b.Query("SELECT 2");
            }
        }

        // WHAT THE COMPILER GENERATES
        // - A 'using' block is just syntactic sugar for this try/finally pattern:
        //   the work goes in 'try', and 'finally' calls Dispose() if the object isn't null.
        public static void DemonstrateWhatUsingCompilesTo()
        {
            DatabaseConnection conn = new DatabaseConnection("ManualDb");
            try
            {
                conn.Query("SELECT 42");
            }
            finally
            {
                if (conn != null)
                    ((IDisposable)conn).Dispose();
            }
        }
    }

    /*

    Use 'using' for anything holding unmanaged/limited resources:
    files (FileStream, StreamReader), DB connections, sockets, locks, timers...

    //1. Don't confuse the using STATEMENT (resource cleanup) with the using DIRECTIVE (namespace import).

    C# 8 added 'using declarations' (using var x = ...;) that dispose at the end of the enclosing
    scope (see 8_UsingDeclarations.cs).

     */
}
