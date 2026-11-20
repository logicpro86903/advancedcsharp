// =============================================================================
//  Feature     : Top-Level Statements
//  C# Version  : C# 9.0
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    A program's entry point can be written as plain statements at the top of
//    ONE file, without declaring a namespace, a Program class or a static Main
//    method. The compiler generates the Main method for you. 'args' is available
//    implicitly, 'await' can be used directly (making Main async), and 'return n'
//    sets the process exit code. Since .NET 6 this is the default template for
//    console apps, web apps and minimal APIs.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    // This project already has a classic entry point in Program.cs, and only ONE file per
    // project may contain top-level statements - so the examples below are kept as comments.
    public class Program
    {
        // BEFORE C# 9: the classic entry point needs a class and a static Main method.
        //
        //     static void Main(string[] args)
        //     {
        //         Console.WriteLine("Hello, World!");
        //         var name = Console.ReadLine();
        //         Console.WriteLine($"Hello, {name}!");
        //     }

        // C# 9 TOP-LEVEL STATEMENTS: the ENTIRE contents of a Program.cs file.
        // - using directives come first, then statements - no namespace, class or Main.
        // - 'args' (the command-line arguments) is available without declaring it.
        // - 'return 1;' / 'return 0;' become the process EXIT CODE (generated Main returns int).
        // - Methods/types may be declared AFTER the statements in the same file.
        //
        //     using System;
        //
        //     if (args.Length == 0)
        //     {
        //         Console.WriteLine("Please provide a name as an argument");
        //         return 1;
        //     }
        //
        //     var name = args[0];
        //     var greeting = args.Length > 1 ? args[1] : "Hello";
        //
        //     Console.WriteLine($"{greeting}, {name}!");
        //     return 0;
    }
}

/*

    //1. Only one file per project can have top-level statements.
    //2. Local functions declared there can't be called from other files - they're local to the
    //   generated Main.
    //3. Combine with global usings (C# 10, see 10_GlobalUsings.cs) for very short entry files.

 */
