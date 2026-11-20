// =============================================================================
//  Feature     : Switch Expressions
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    A switch EXPRESSION ('value switch { pattern => result, ... }') produces a
//    value, unlike the classic switch STATEMENT which executes blocks. It drops
//    the 'case', 'break' and temporary-variable ceremony, fits naturally into
//    expression-bodied members, and works with all patterns (constant, type,
//    property, tuple, relational...). The compiler warns when the arms don't
//    cover every possible input.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class SwitchExpressions
    {
        public static void Run()
        {
            GetDayTypeTraditional(DayOfWeek.Wednesday);
            GetDayType(DayOfWeek.Sunday);
        }

        // CLASSIC SWITCH STATEMENT (for comparison)
        // - Needs a result variable, a 'case' label per value, fall-through stacking, 'break'
        //   in every section, and a separate 'return'.
        public static string GetDayTypeTraditional(DayOfWeek day)
        {
            string result;
            switch (day)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    result = "Weekday";
                    break;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    result = "Weekend";
                    break;
                default:
                    result = "Unknown";
                    break;
            }
            return result;
        }

        // SWITCH EXPRESSION
        // - Syntax: 'input switch { pattern => value, pattern => value, _ => default }'.
        //   The input comes BEFORE the 'switch' keyword; arms are separated by commas.
        // - The whole thing is an expression, so it can be the body of an => member.
        // - '_' is the discard pattern: it matches anything (like 'default').
        // - 'or' to combine several values in one arm is a C# 9 pattern combinator.
        // - If no arm matches at runtime, SwitchExpressionException is thrown - so include '_'
        //   when the arms aren't exhaustive.
        public static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday
                or DayOfWeek.Thursday or DayOfWeek.Friday => "Weekday",
            DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
            _ => "Unknown"
        };
    }


    /*

    Other C# 8 pattern forms that shine in switch expressions:
        // property pattern
        decimal Discount(Order o) => o switch { { Total: > 1000 } => 0.1m, _ => 0m };

        // tuple pattern
        string Rps(string a, string b) => (a, b) switch
        {
            ("rock", "scissors") => "rock wins",
            ("paper", "rock")    => "paper wins",
            (var x, var y) when x == y => "tie",
            _ => "other"
        };

    See 7_PatternMatching.cs and 9_PatternMatchingEnhancements.cs.

     */
}
