using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class ExceptionFilters
    {
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
                // Handle timeout specifically
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                Console.WriteLine("Resource not found");
                // Handle 404 errors
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Other HTTP error: {ex.Message}");
                // Handle other HTTP errors
            }
        }

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

    //Exception filters were introduced in C# 6.0 and allow you to specify additional conditions
    //for when a catch block should handle an exception using the when keyword.
    //This provides more granular control over exception handling without modifying the stack trace.
}
