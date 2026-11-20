// =============================================================================
//  Feature     : Exception Handling
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    Structured exception handling with try / catch / finally / throw. Errors are
//    represented as objects deriving from System.Exception, carrying a message,
//    a stack trace and an optional InnerException. 'catch' blocks handle specific
//    exception types, 'finally' always runs for cleanup, and you can define your
//    own exception classes to carry domain-specific error information.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ExceptionHandling
    {
        public static void Run()
        {
            DemonstrateTryCatchFinally();
            DemonstrateCustomException();
            DemonstrateRethrow();
        }

        // TRY / CATCH / FINALLY
        // - Code that may fail goes in 'try'.
        // - Multiple 'catch' blocks are checked TOP to BOTTOM; the first matching type wins,
        //   so order them from MOST specific to MOST general (Exception last). The compiler
        //   rejects a specific catch placed after a more general one.
        // - 'finally' ALWAYS runs: on success, after a handled exception, and even when the
        //   exception is not caught. Use it for cleanup (closing files, releasing locks).
        public static void DemonstrateTryCatchFinally()
        {
            string[] inputs = { "42", "abc", "99999999999" };

            foreach (string input in inputs)
            {
                try
                {
                    int value = int.Parse(input);
                    Console.WriteLine($"Parsed: {value}");
                }
                catch (FormatException)
                {
                    Console.WriteLine($"'{input}' is not a number");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine($"'{input}' overflow: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("  (finally block ran)");
                }
            }
        }

        // CUSTOM EXCEPTION CLASS
        // - Derive from Exception and give it a name ending in "Exception".
        // - Add properties/fields with extra context (here: requested vs available amount)
        //   so callers can react programmatically instead of parsing the message text.
        // - Pass a readable message to base(...), and provide a (message, inner) constructor
        //   so the exception can wrap another one.
        public class InsufficientFundsException : Exception
        {
            public decimal Requested;
            public decimal Available;

            public InsufficientFundsException(decimal requested, decimal available)
                : base($"Requested {requested} but only {available} available")
            {
                Requested = requested;
                Available = available;
            }

            public InsufficientFundsException(string message, Exception inner) : base(message, inner) { }
        }

        static void Withdraw(decimal balance, decimal amount)
        {
            if (amount > balance)
                throw new InsufficientFundsException(amount, balance);
            Console.WriteLine($"Withdrew {amount}");
        }

        // THROWING AND CATCHING A CUSTOM EXCEPTION
        // - 'throw new ...' raises the exception; the catch block can then read its custom
        //   properties (Requested, Available) in addition to the standard Message.
        public static void DemonstrateCustomException()
        {
            try
            {
                Withdraw(100m, 250m);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Error: {ex.Message} (short by {ex.Requested - ex.Available})");
            }
        }

        // WRAPPING: translate a low-level exception into a higher-level one, keeping the
        // original as InnerException so no diagnostic information is lost.
        static void LoadConfig()
        {
            try
            {
                throw new InvalidOperationException("config file missing");
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException("Startup failed", ex);
            }
        }

        // RETHROWING
        // - A plain 'throw;' inside a catch block rethrows the SAME exception and preserves
        //   its original stack trace (useful for "log and let it bubble up").
        // - The outer handler then inspects both the wrapper and its InnerException.
        public static void DemonstrateRethrow()
        {
            try
            {
                try
                {
                    LoadConfig();
                }
                catch (ApplicationException)
                {
                    Console.WriteLine("Logging and rethrowing...");
                    throw;
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"{ex.Message} <- {ex.InnerException!.Message}");
                // Startup failed <- config file missing
            }
        }
    }

    /*

    //1. 'throw ex;' RESETS the stack trace. Use 'throw;' to rethrow.
    catch (Exception ex) { throw ex; }  // BAD
    catch (Exception ex) { throw; }     // GOOD

    //2. Don't use exceptions for normal control flow - prefer int.TryParse over catching FormatException.

    //3. Don't swallow exceptions with an empty catch { }.

    See 6_ExceptionFilters.cs for 'catch ... when (...)' (C# 6).

     */
}
