// =============================================================================
//  Feature     : using static Directive
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    'using static TypeName;' imports the STATIC members (and nested types) of a
//    single type, so they can be used without the type name prefix:
//    WriteLine(...) instead of Console.WriteLine(...), Sqrt(25) instead of
//    Math.Sqrt(25). Great for math-heavy code or frequently used helpers; it also
//    brings enum members into scope (using static System.DayOfWeek -> Monday).
// =============================================================================

// USING STATIC DIRECTIVES: import the static members of Console, Math and String.
using static System.Console;
using static System.Math;
using static System.String;

namespace CSharp
{
    public class StaticDirectives
    {
        // WITHOUT vs WITH 'using static'
        // - The first block uses fully-qualified static calls (System.Console.WriteLine).
        // - The second block calls the imported members directly: WriteLine -> Console.WriteLine,
        //   Sqrt -> Math.Sqrt, IsNullOrEmpty -> String.IsNullOrEmpty.
        public static void Run()
        {
            System.Console.WriteLine("Hello");
            double result = System.Math.Sqrt(16);
            bool isEmpty = System.String.IsNullOrEmpty("test");

            WriteLine("Hello World!");
            double sqrtResult = Sqrt(25);
            bool isNull = IsNullOrEmpty("");

            WriteLine($"Square root of 25 is {sqrtResult}");
        }
    }

    /*

    //1. Overuse hurts readability - a bare 'Max(a, b)' may not obviously be Math.Max.
    //2. Extension methods are NOT imported as plain static calls by 'using static'.
    //3. C# 10 'global using static' applies the import to the whole project (see 10_GlobalUsings.cs).

     */
}
