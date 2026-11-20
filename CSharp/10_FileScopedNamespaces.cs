// =============================================================================
//  Feature     : File-Scoped Namespaces
//  C# Version  : C# 10.0
//  .NET        : .NET 6
//  Released    : 2021 (Visual Studio 2022)
// -----------------------------------------------------------------------------
//  Summary:
//    'namespace X;' (ending with a semicolon) applies the namespace to the WHOLE
//    file - no braces needed. Almost every file contains exactly one namespace,
//    so the old braces just added one level of indentation to every line.
//
//      BEFORE (C# 1-9)                      AFTER (C# 10+)
//      namespace CSharp                     namespace CSharp;
//      {
//          public class Foo                 public class Foo
//          {                                {
//              ...                              ...
//          }                                }
//      }
//
//    NOTE: This file deliberately uses the syntax it demonstrates. All other files
//    in this project use the older block-scoped form: namespace CSharp { ... }
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

// FILE-SCOPED NAMESPACE DECLARATION: everything below belongs to 'CSharp'.
namespace CSharp;

public class FileScopedNamespaces
{
    // VERIFYING THE NAMESPACE
    // - typeof(...).Namespace shows the class is in 'CSharp', exactly as if braces were used.
    // - Every other type in this file (FileScopedHelper) is in the same namespace.
    // - It interoperates with block-scoped files: Linq (declared with braces elsewhere) lives
    //   in the very same CSharp namespace.
    public static void Run()
    {
        Console.WriteLine($"Namespace of this class: {typeof(FileScopedNamespaces).Namespace}"); // CSharp
        Console.WriteLine($"Full name: {typeof(FileScopedNamespaces).FullName}");               // CSharp.FileScopedNamespaces

        var helper = new FileScopedHelper();
        Console.WriteLine(helper.Describe());

        Console.WriteLine($"Same namespace as Linq: {typeof(Linq).Namespace == typeof(FileScopedNamespaces).Namespace}"); // True
    }
}

public class FileScopedHelper
{
    public string Describe() => $"{nameof(FileScopedHelper)} lives in {GetType().Namespace} with one less indent level";
}

/*

Rules:
//1. Only ONE file-scoped namespace per file, and you can't mix it with block-scoped namespaces in that file.
//2. It must come before any type declarations (using directives go above it, or inside if you prefer).
//3. Nested namespaces: use dots -> namespace Company.Product.Feature;

Tip: set it as the default style in .editorconfig:
    csharp_style_namespace_declarations = file_scoped:warning

Pairs well with other C# 10 "less ceremony" features:
- global using directives (10_GlobalUsings.cs, GlobalUsings.cs)
- implicit usings (<ImplicitUsings>enable</ImplicitUsings> in CSharp.csproj)
- top-level statements (C# 9, 9_TopLevelStatments.cs)

 */
