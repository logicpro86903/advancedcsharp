using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 5 / C# 9.0 (2020)
    public class Records
    {
        // Simple record with positional parameters
        public record Person(string FirstName, string LastName);
        public record Employee(string Name, string Department, decimal Salary);

        public static void Run()
        {
            // Using the record
            var person1 = new Person("John", "Doe");
            var person2 = new Person("John", "Doe");

            Console.WriteLine(person1); // Person { FirstName = John, LastName = Doe }
            Console.WriteLine(person1 == person2); // True (value equality)
            Console.WriteLine(ReferenceEquals(person1, person2)); // False (different instances)


            var emp1 = new Employee("Alice", "Engineering", 75000);
            var emp2 = emp1 with { Salary = 80000 }; // Create new instance with modified salary
            var emp3 = emp1 with { Department = "Management", Salary = 90000 }; // Multiple changes

            Console.WriteLine(emp1); // Employee { Name = Alice, Department = Engineering, Salary = 75000 }
            Console.WriteLine(emp2); // Employee { Name = Alice, Department = Engineering, Salary = 80000 }
            Console.WriteLine(emp3); // Employee { Name = Alice, Department = Management, Salary = 90000 }

        }
    }

    /*
     Records are a reference type introduced in C# 9.0 that provide a concise way to create immutable 
    data types with value-based equality semantics. 
    They're perfect for representing data that shouldn't change after creation.
     */
}
