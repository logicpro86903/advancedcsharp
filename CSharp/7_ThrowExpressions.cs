// =============================================================================
//  Feature     : throw Expressions
//  C# Version  : C# 7.0
//  .NET        : .NET Framework 4.7 / .NET Core 2.0
//  Released    : 2017 (Visual Studio 2017)
// -----------------------------------------------------------------------------
//  Summary:
//    'throw' can be used as an EXPRESSION, not only as a statement. Before C# 7
//    you couldn't write 'x ?? throw ...' or 'ok ? a : throw ...' and needed a
//    separate if-statement. Throw expressions make guard clauses one-liners.
//    They are allowed in exactly three places:
//      1. the right side of ??                     value ?? throw new ...
//      2. either branch of the ?: operator          ok ? value : throw new ...
//      3. the body of an expression-bodied member   => throw new ...
//    (and, from C# 8, in switch expression arms).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ThrowExpressions
    {
        public static void Run()
        {
            DemonstrateNullCoalescing();
            DemonstrateConditional();
            DemonstrateExpressionBodied();
        }

        // THROW WITH ?? - THE CLASSIC CONSTRUCTOR GUARD
        // - 'smtpHost ?? throw new ArgumentNullException(...)' assigns the value if it isn't null,
        //   otherwise throws - validation and assignment in ONE line.
        // - Before C# 7 this took two statements:
        //       if (smtpHost == null) throw new ArgumentNullException(nameof(smtpHost));
        //       _smtpHost = smtpHost;
        public class EmailService
        {
            private readonly string _smtpHost;
            private readonly string _sender;

            public EmailService(string? smtpHost, string? sender)
            {
                _smtpHost = smtpHost ?? throw new ArgumentNullException(nameof(smtpHost));
                _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            }

            public string Describe() => $"{_sender} via {_smtpHost}";
        }

        // USING THE ?? GUARD
        // - A valid construction succeeds; passing null throws, and ParamName reports which argument.
        // - The same idea works for "must exist" lookups: GetValueOrDefault(...) ?? throw ...
        public static void DemonstrateNullCoalescing()
        {
            var ok = new EmailService("smtp.example.com", "noreply@example.com");
            Console.WriteLine(ok.Describe());

            try
            {
                var bad = new EmailService(null, "noreply@example.com");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Guard caught missing: {ex.ParamName}");    // smtpHost
            }

            var config = new Dictionary<string, string> { ["Env"] = "Prod" };
            string env = config.GetValueOrDefault("Env") ?? throw new InvalidOperationException("Env not configured");
            Console.WriteLine($"Env = {env}");
        }

        // THROW IN A CONDITIONAL ?: BRANCH
        // - Return the value if it is valid, otherwise throw - all inside one return expression.
        static int ParsePercentage(string text)
        {
            int value = int.Parse(text);
            return value >= 0 && value <= 100
                ? value
                : throw new ArgumentOutOfRangeException(nameof(text), $"{value} is not a valid percentage");
        }

        public static void DemonstrateConditional()
        {
            Console.WriteLine($"Parsed: {ParsePercentage("75")}%");
            try
            {
                ParsePercentage("150");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Rejected: {ex.Message.Split('(')[0].Trim()}"); // 150 is not a valid percentage
            }
        }

        // THROW IN EXPRESSION-BODIED MEMBERS
        // - Explicitly unsupported operations (Delete on a read-only repository) become one-liners.
        // - Handy placeholder while a member isn't written yet (NotImplementedException).
        public class ReadOnlyRepository
        {
            public string Get(int id) => $"Item {id}";

            public void Delete(int id) => throw new NotSupportedException("This repository is read-only");

            public string Export() => throw new NotImplementedException();
        }

        // USING THROWING MEMBERS + THROW IN LAMBDAS / SWITCH EXPRESSIONS
        // - Calling Delete throws NotSupportedException, which we catch.
        // - Throw expressions also work inside lambdas and switch expression arms (C# 8):
        //   the '_' arm rejects zero.
        public static void DemonstrateExpressionBodied()
        {
            var repo = new ReadOnlyRepository();
            Console.WriteLine(repo.Get(7));
            try
            {
                repo.Delete(7);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
            }

            Func<int, string> describe = n => n switch
            {
                > 0 => "positive",
                < 0 => "negative",
                _ => throw new ArgumentException("zero not allowed")
            };
            Console.WriteLine(describe(5));                    // positive
        }
    }

    /*

    //1. A throw expression has no type of its own - it adapts to whatever the other side needs.
    //   That's why 'var x = throw ...;' alone is a compile error.

    //2. For simple null checks in .NET 6+ prefer the built-in helpers:
    //       ArgumentNullException.ThrowIfNull(smtpHost);
    //   Throw expressions remain great when you want to assign AND validate in one step.

     */
}
