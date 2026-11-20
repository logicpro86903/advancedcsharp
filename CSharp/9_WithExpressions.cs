using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 5 / C# 9.0 (2020)
    public class WithExpressions
    {
        public record Person(string FirstName, string LastName, int Age, string Email);

        public static void Run()
        {
            var original = new Person("John", "Doe", 30, "john@example.com");

            // Create new instances with modified properties
            var olderPerson = original with { Age = 31 };
            var marriedPerson = original with { LastName = "Smith" };
            var updatedContact = original with { Email = "john.smith@newcompany.com", LastName = "Smith" };

            Console.WriteLine(original);      // Person { FirstName = John, LastName = Doe, Age = 30, Email = john@example.com }
            Console.WriteLine(olderPerson);   // Person { FirstName = John, LastName = Doe, Age = 31, Email = john@example.com }
            Console.WriteLine(marriedPerson); // Person { FirstName = John, LastName = Smith, Age = 30, Email = john@example.com }
            Console.WriteLine(updatedContact);// Person { FirstName = John, LastName = Smith, Age = 30, Email = john.smith@newcompany.com }

            // Original remains unchanged
            Console.WriteLine($"Original age still: {original.Age}"); // 30
        }
    }
}
