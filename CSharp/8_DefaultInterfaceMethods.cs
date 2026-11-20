using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class DefaultInterfaceMethods
    {
        public static void Run()
        {
            ILogger logger = new ConsoleLogger();
            logger.LogInfo("This is an informational message.");
            logger.LogError("This is an error message.");


        }

        public interface ILogger
        {
            void LogError(string message);
            void LogWarning(string message);

            // Default implementation
            void LogInfo(string message)
            {
                Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            }

            // Default implementation with logic
            void LogDebug(string message)
            {
                if (IsDebugEnabled())
                {
                    Console.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}");
                }
            }

            // Default implementation that calls other methods
            void LogException(Exception ex)
            {
                LogError($"Exception: {ex.Message}");
                LogDebug($"Stack trace: {ex.StackTrace}");
            }

            // Virtual method with default implementation
            bool IsDebugEnabled() => false;
        }

        // Implementation that only provides required methods
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
     //Default interface methods, introduced in C# 8.0, allow you to provide default implementations 
    // for interface members. This enables interface evolution without breaking existing 
    //implementations and supports multiple inheritance of behavior.
     */
}
