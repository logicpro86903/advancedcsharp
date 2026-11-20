// =============================================================================
//  Feature     : Default Interface Methods
//  C# Version  : C# 8.0
//  .NET        : .NET Core 3.0 (not supported on .NET Framework)
//  Released    : 2019 (Visual Studio 2019)
// -----------------------------------------------------------------------------
//  Summary:
//    Interface members can now include a DEFAULT IMPLEMENTATION (a method body).
//    Classes implementing the interface only have to implement the members
//    without a body; the rest are inherited from the interface. The main purpose
//    is INTERFACE EVOLUTION: library authors can add new members to a published
//    interface without breaking every existing implementation. It also enables a
//    form of "traits" - reusable behavior shared across unrelated classes.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class DefaultInterfaceMethods
    {
        // CALLING DEFAULT MEMBERS
        // - LogInfo isn't implemented by ConsoleLogger; the interface's default body runs.
        // - IMPORTANT: default members are only reachable through the INTERFACE type.
        //   That's why 'logger' is declared as ILogger - 'new ConsoleLogger().LogInfo(...)'
        //   would NOT compile.
        public static void Run()
        {
            ILogger logger = new ConsoleLogger();
            logger.LogInfo("This is an informational message.");
            logger.LogError("This is an error message.");
        }

        // INTERFACE WITH DEFAULT IMPLEMENTATIONS
        // - LogError / LogWarning have no body: every implementing class MUST provide them.
        // - LogInfo / LogDebug / LogException / IsDebugEnabled HAVE bodies: implementers get them
        //   for free and may override them by declaring their own public method.
        // - Default members can call other interface members (LogException calls LogError and
        //   LogDebug), so shared behavior can be composed from the abstract members.
        // - Interfaces still can't hold instance STATE (no instance fields).
        public interface ILogger
        {
            void LogError(string message);
            void LogWarning(string message);

            void LogInfo(string message)
            {
                Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            }

            void LogDebug(string message)
            {
                if (IsDebugEnabled())
                {
                    Console.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}");
                }
            }

            void LogException(Exception ex)
            {
                LogError($"Exception: {ex.Message}");
                LogDebug($"Stack trace: {ex.StackTrace}");
            }

            bool IsDebugEnabled() => false;
        }

        // IMPLEMENTING CLASS: provides only the required (body-less) members.
        public class ConsoleLogger : ILogger
        {
            public void LogError(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
                Console.ResetColor();
            }

            public void LogWarning(string message)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
                Console.ResetColor();
            }
        }
    }

    /*

    //1. Requires runtime support: works on .NET Core 3.0+ / .NET 5+, NOT on .NET Framework.

    //2. "Diamond" ambiguity: if a class implements two interfaces that both provide a default for
    //   the same inherited member, the class must implement it explicitly.

    Also new in C# 8 interfaces: static members, private members, and access modifiers.
    C# 11 added static abstract members (see 11_GenericMath.cs).

     */
}
