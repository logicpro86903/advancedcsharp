using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharp
{
    //.NET FW 4.5 & C#5 2012
    public static class CallerInfo
    {
        public static void Run()
        {
            LogMessage("This is a test message.");
        }

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
