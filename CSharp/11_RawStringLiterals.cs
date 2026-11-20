// =============================================================================
//  Feature     : Raw String Literals (""" ... """)
//  C# Version  : C# 11.0
//  .NET        : .NET 7
//  Released    : 2022 (Visual Studio 2022 v17.4)
// -----------------------------------------------------------------------------
//  Summary:
//    A raw string literal starts and ends with at least THREE double quotes.
//    Everything in between is taken literally: quotes, backslashes and newlines
//    need no escaping. In the multi-line form, the indentation of the closing """
//    is stripped from every line, so the text can be indented to match the
//    surrounding code. Perfect for JSON, XML, SQL, regular expressions and any
//    multi-line text. Combine with $ for interpolation ($"""...{x}...""").
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class RawStringLiterals
    {
        // MULTI-LINE RAW STRING
        // - Opening """ must be followed by a newline; closing """ must be on its own line.
        // - "quotes" and \backslashes\ appear exactly as written - no \" or \\ escapes.
        // - The closing """ is indented 16 spaces, so 16 spaces are removed from the start of
        //   every content line: the printed text starts at column 0.
        public static void Run()
        {
            var message = """
                This is a raw string literal.
                No need to escape "quotes" or \backslashes\.
                It spans multiple lines naturally.
                """;

            Console.WriteLine(message);
        }
    }

    /*

    More forms:
        var single = """He said "hi" """;                       // single-line raw string

        var json = $$"""
            { "name": "{{name}}", "age": {{age}} }
            """;                                                 // $$ -> holes use {{ }}, single { } stay literal

        var text = """"
            A raw string can contain """ if you open with four quotes.
            """";

    //1. Content lines must start with at least the same whitespace as the closing """ (else compile error).
    //2. The number of $ signs decides how many braces start an interpolation hole.

     */
}
