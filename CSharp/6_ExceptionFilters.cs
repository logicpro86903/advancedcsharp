// =============================================================================
//  Feature     : Exception Filters (catch ... when)
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    'catch (SomeException ex) when (condition)' adds a boolean filter to a catch
//    block. The block handles the exception only if the type matches AND the
//    condition is true; otherwise the runtime keeps searching for another
//    handler. Because the filter runs BEFORE the stack unwinds, a non-matching
//    exception propagates with its original stack trace intact - unlike catching
//    and rethrowing. This allows precise handling based on exception details.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ExceptionFilters
    {
        // FILTERING ON EXCEPTION DETAILS
        // - All three catch blocks catch the same type, HttpRequestException.
        // - The 'when' clauses are evaluated top to bottom; the first one whose condition is
        //   true handles the exception (timeout, then 404).
        // - The final unfiltered catch is the fallback for any other HTTP error.
        // - Before C# 6 you had to catch once and use if/else inside, rethrowing anything you
        //   didn't want - which was clumsier and lost stack information.
        public void HttpRequestExample()
        {
            try
            {
                var client = new HttpClient();
                var response = client.GetAsync("https://api.example.com/data").Result;
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("timeout"))
            {
                Console.WriteLine("Request timed out - retrying...");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                Console.WriteLine("Resource not found");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Other HTTP error: {ex.Message}");
            }
        }

        // FILTERING ON EXTERNAL STATE
        // - The filter can be ANY boolean expression, not just properties of the exception.
        // - Here the same DivideByZeroException is handled differently depending on the time of
        //   day; if the filter is false, the next matching catch block takes over.
        public void NumericOperationExample()
        {
            try
            {
                int i = 0;
                int result = 10 / i;
            }
            catch (DivideByZeroException ex) when (DateTime.Now.Hour < 12)
            {
                Console.WriteLine("Division by zero in the morning - logging for investigation");
                LogMorningError(ex);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Division by zero in afternoon/evening - standard handling");
                HandleStandardError(ex);
            }
        }

        private void LogMorningError(Exception ex) => Console.WriteLine($"Morning error logged: {ex.Message}");
        private void HandleStandardError(Exception ex) => Console.WriteLine($"Standard error: {ex.Message}");
    }

    /*

    Logging trick - log every exception without actually catching it:
        catch (Exception ex) when (Log(ex)) { }   // Log returns false -> exception keeps propagating

    //1. Keep filters side-effect free (apart from logging) and make sure they can't throw.

     */
}
