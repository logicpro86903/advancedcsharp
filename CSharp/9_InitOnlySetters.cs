using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 5 / C# 9.0 (2020)
    public class InitOnlySetters
    {
        public static void Run()
        {
            // Object initialization - allowed
            var person = new Person2
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                BirthDate = new DateTime(1993, 5, 15)
            };

            // This would cause a compile error after initialization:
           // person.FirstName = "Jane"; // Error: Init-only property can only be assigned in object initializer
        }
    }

    public class Person2
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; init; }
        public DateTime BirthDate { get; init; }
    }
}
