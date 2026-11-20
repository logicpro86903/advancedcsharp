// =============================================================================
//  Feature     : Named and Optional Parameters
//  C# Version  : C# 4.0
//  .NET        : .NET Framework 4.0
//  Released    : 2010 (Visual Studio 2010)
// -----------------------------------------------------------------------------
//  Summary:
//    OPTIONAL parameters declare a default value (string name = "World"), so
//    callers may omit them - replacing piles of overloads. NAMED arguments let
//    callers specify which parameter a value is for (name: "Alice"), in any
//    order, which makes calls with many parameters or boolean flags far more
//    readable. Both also made COM interop (Office APIs) much less painful.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public static class NamedAndOptionalParams
    {
        // OPTIONAL AND NAMED PARAMETERS
        // - 'string name = "World"' makes the parameter OPTIONAL; the default must be a
        //   compile-time constant (literal, const, default(T) or new ValueType()).
        // - Optional parameters must come AFTER all required parameters.
        // - Greet() omits the argument, so the compiler inserts "World" at the call site.
        // - Greet(name: "Alice") uses a NAMED argument: 'parameterName: value'. Named
        //   arguments can be given in any order and let you skip earlier optional parameters.
        public static void Run()
        {
            void Greet(string name = "World") => Console.WriteLine($"Hello {name}");

            Greet();                // Hello World
            Greet(name: "Alice");   // Hello Alice
        }
    }
}


/*

    //1. Changing a default value is a breaking change: defaults are COPIED into the
    //   caller's compiled code, so callers keep the old value until they recompile.
    public void Method(int x = 1) { }   // Version 1
    public void Method(int x = 2) { }   // Version 2 - old callers still pass 1!

    //2. Renaming a parameter breaks callers that use it as a named argument.

    //3. Named arguments can hurt readability if overused; they shine for booleans and
    //   long parameter lists:  Save(overwrite: true, createBackup: false)

 */
