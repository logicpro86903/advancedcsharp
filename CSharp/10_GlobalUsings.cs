// =============================================================================
//  Feature     : Global Using Directives and Implicit Usings
//  C# Version  : C# 10.0
//  .NET        : .NET 6
//  Released    : 2021 (Visual Studio 2022)
// -----------------------------------------------------------------------------
//  Summary:
//    'global using System.Text;' placed in ANY file imports that namespace into
//    EVERY file of the project, removing the same using lines repeated at the top
//    of hundreds of files. This project declares its own in GlobalUsings.cs.
//    The SDK's IMPLICIT USINGS feature (<ImplicitUsings>enable</ImplicitUsings>
//    in CSharp.csproj) additionally generates global usings for common
//    namespaces such as System, System.IO, System.Linq and System.Threading.Tasks.
// =============================================================================

// Note: this file has NO using directives at all.
namespace CSharp
{
    public class GlobalUsings
    {
        // USING TYPES WITHOUT LOCAL using DIRECTIVES
        // - Console (System), string[] and LINQ's Where/ToList (System.Linq) all compile even
        //   though this file imports nothing - the namespaces come from global usings:
        //     * GlobalUsings.cs        -> global using System; System.Collections.Generic; System.Text;
        //     * implicit usings (SDK)  -> System.Linq, System.IO, System.Threading.Tasks, ...
        public static void Run()
        {
            string[] strings = { "apple", "banana", "cherry" };
            strings.Where(s => s.StartsWith("b"))
                   .ToList()
                   .ForEach(s => Console.WriteLine(s));
        }
    }
}

/*

    Forms of global usings:
        global using System.Text.Json;                 // namespace
        global using static System.Math;                // static members (see 6_StaticDirectives.cs)
        global using Json = System.Text.Json.JsonSerializer;   // alias

    They can also be declared in the .csproj:
        <ItemGroup> <Using Include="System.Text.Json" /> </ItemGroup>

    //1. Global usings must appear before any non-global using and before namespace declarations.
    //2. Keep them in one well-known file (e.g. GlobalUsings.cs) so they're easy to find.
    //3. The generated implicit usings live in obj/.../CSharp.GlobalUsings.g.cs.

 */
