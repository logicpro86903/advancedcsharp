// =============================================================================
//  Feature     : Out Variables (inline out declarations)
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    You can declare an out variable right in the argument list of the call:
//    int.TryParse(input, out int result). Previously the variable had to be
//    declared on a separate line beforehand. The type can also be 'var'
//    (out var result). The variable's scope "leaks" to the enclosing block, so
//    it remains usable after the if-statement that declared it.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class OutVariablesAnywhere
    {
        public static void Run()
        {
            TraditionalOutVariables();
            InlineOutVariables();
        }

        // BEFORE C# 7: DECLARE FIRST, THEN PASS
        // - 'int result;' had to be declared on its own line before it could be passed as out.
        // - It stays in scope afterwards; if parsing failed, TryParse sets it to 0.
        public static void TraditionalOutVariables()
        {
            string input = "123";

            int result;
            if (int.TryParse(input, out result))
            {
                Console.WriteLine($"Parsed: {result}");
            }

            Console.WriteLine($"Result variable: {result}");
        }

        // C# 7: DECLARE INLINE
        // - 'out int result' declares AND passes the variable in one step.
        // - 'out var result' also works - the type is inferred from the method's signature.
        // - SCOPE: a variable declared in an if-condition belongs to the ENCLOSING block, so it
        //   is still accessible after the if-statement (useful for "guard and continue" code).
        public static void InlineOutVariables()
        {
            string input = "123";

            if (int.TryParse(input, out int result))
            {
                Console.WriteLine($"Parsed: {result}");
            }

            Console.WriteLine($"Result variable: {result}");
        }
    }

    /*

    Guard-clause style made possible by the scope rule:
        if (!int.TryParse(input, out var id)) return;
        Console.WriteLine(id);    // id is definitely assigned here

    Combine with discards when you don't need the value:  int.TryParse(input, out _)

     */
}
