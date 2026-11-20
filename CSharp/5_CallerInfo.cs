// =============================================================================
//  Feature     : Caller Info Attributes
//  C# Version  : C# 5.0
//  .NET        : .NET Framework 4.5
//  Released    : 2012 (Visual Studio 2012)
// -----------------------------------------------------------------------------
//  Summary:
//    [CallerMemberName], [CallerFilePath] and [CallerLineNumber] are applied to
//    OPTIONAL parameters. When the caller omits those arguments, the COMPILER
//    fills them in with the calling method's name, source file path and line
//    number. This gives logging, tracing and INotifyPropertyChanged code exact
//    caller context with zero runtime cost (no stack walking or reflection).
// =============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharp
{
    public static class CallerInfo
    {
        // CALLING THE METHOD
        // - Only 'message' is passed; the compiler supplies the remaining three arguments
        //   based on where this call is written (member "Run", this file, this line).
        public static void Run()
        {
            LogMessage("This is a test message.");
        }

        // DECLARING CALLER INFO PARAMETERS
        // - Each attribute must be on an optional parameter with a default value.
        // - [CallerMemberName] : name of the calling method/property ("Run").
        // - [CallerFilePath]   : full path of the source file at COMPILE time.
        // - [CallerLineNumber] : line number of the call.
        // - If a caller passes a value explicitly, that value is used instead.
        public static void LogMessage(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            Console.WriteLine($"Message: {message}");
            Console.WriteLine($"Called from: {memberName}");
            Console.WriteLine($"File: {Path.GetFileName(sourceFilePath)}");
            Console.WriteLine($"Line: {sourceLineNumber}");
            Console.WriteLine();
        }
    }
}

/*

    Classic use - INotifyPropertyChanged without hard-coded property names:
        protected void OnPropertyChanged([CallerMemberName] string name = "") => ...
        set { _title = value; OnPropertyChanged(); }   // name = "Title"

    C# 10 added [CallerArgumentExpression("param")], which captures the source TEXT of an
    argument - used by ArgumentNullException.ThrowIfNull(x) to report "x".

 */
