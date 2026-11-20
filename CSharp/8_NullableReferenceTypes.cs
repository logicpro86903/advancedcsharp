// =============================================================================
//  Feature     : Nullable Reference Types (string vs string?)
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    With nullable reference types enabled (<Nullable>enable</Nullable>, the
//    default for new projects since .NET 6), reference types are NON-NULLABLE by
//    default: 'string' means "never null" and 'string?' means "may be null". The
//    compiler performs flow analysis and WARNS when you might dereference null or
//    assign null to a non-nullable variable. It's purely a compile-time feature -
//    no runtime checks are added - but it catches most NullReferenceExceptions
//    before the code ever runs.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class NullableReferenceTypes
    {
        public static void Run()
        {
            DemonstrateBasicUsage();
            ProcessNames("John", null, "Doe");
        }

        // DECLARING NULLABILITY
        // - 'string Name' is NON-nullable: it must be initialized (here with string.Empty) and
        //   the compiler warns if you try to assign null to it.
        // - 'string? NickName' is NULLABLE: null is a valid value, so you must check before use.
        // - The same applies to any reference type, including collections.
        public static string Name { get; set; } = string.Empty;

        public static string? NickName { get; set; }

        public static List<string> Tags { get; set; } = new List<string>();

        public static List<string>? OptionalTags { get; set; }

        // WORKING WITH NULLABLE VALUES
        // - Name.Length: safe, no warning - Name is non-nullable.
        // - NickName.Length without a check: compiler WARNING CS8602 (possible null dereference).
        //   At runtime this line throws because NickName is null - the warning was right!
        // - Inside 'if (NickName != null)' flow analysis knows it's not null - no warning.
        // - '?.' plus '??' handles null gracefully.
        // - The NULL-FORGIVING operator '!' silences the warning ("trust me, it's not null").
        //   It does NOT add a check - use it only when you really know better than the compiler.
        public static void DemonstrateBasicUsage()
        {
            Console.WriteLine($"Name length: {Name.Length}");

            Console.WriteLine($"Nickname length: {NickName.Length}"); // warning CS8602

            if (NickName != null)
            {
                Console.WriteLine($"Nickname length: {NickName.Length}");
            }

            Console.WriteLine($"Nickname length: {NickName?.Length ?? 0}");

            Console.WriteLine($"Nickname length: {NickName!.Length}");
        }

        // NULLABLE PARAMETERS
        // - The signature documents intent: firstName/lastName are required, middleName is optional.
        // - Callers get a warning if they pass null for a non-nullable parameter.
        // - Inside the method, middleName must be checked before use.
        public static void ProcessNames(string firstName, string? middleName, string lastName)
        {
            string fullName = $"{firstName} {lastName}";

            if (middleName != null)
            {
                fullName = $"{firstName} {middleName} {lastName}";
            }

            string alternativeFullName = middleName != null
                ? $"{firstName} {middleName} {lastName}"
                : $"{firstName} {lastName}";

            Console.WriteLine($"Full name: {fullName}");
        }
    }

    /*

    //1. These are WARNINGS, not errors (unless you add <WarningsAsErrors>nullable</WarningsAsErrors>).
    //2. No runtime enforcement: callers with the feature disabled (or using '!') can still pass null,
    //   so public APIs should still validate (ArgumentNullException.ThrowIfNull).
    //3. Attributes refine the analysis: [NotNullWhen(true)], [MaybeNull], [NotNullIfNotNull]...
    //4. string? (annotation) is not the same as int? (Nullable<int>, a real different type).

     */
}
