// =============================================================================
//  Feature     : Records (record class)
//  C# Version  : C# 9.0
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    A record is a reference type designed for DATA. One line such as
//    'record Person(string FirstName, string LastName);' generates a constructor,
//    init-only properties, VALUE-BASED equality (Equals, GetHashCode, ==, !=),
//    a readable ToString(), a Deconstruct method, and support for non-destructive
//    copies with 'with'. Records are ideal for DTOs, messages, events and other
//    immutable data that is compared by content rather than identity.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class Records
    {
        // POSITIONAL RECORDS
        // - The parameter list is the "primary constructor". The compiler generates:
        //     * public init-only properties FirstName and LastName
        //     * a constructor taking those values
        //     * Equals/GetHashCode/==/!= comparing ALL properties
        //     * ToString() -> "Person { FirstName = John, LastName = Doe }"
        //     * Deconstruct(out string FirstName, out string LastName)
        // - You can still add a body with extra methods or properties.
        public record Person(string FirstName, string LastName);
        public record Employee(string Name, string Department, decimal Salary);

        // VALUE EQUALITY AND NON-DESTRUCTIVE MUTATION
        // - person1 == person2 is TRUE because their property values match, even though they
        //   are two different objects (ReferenceEquals is false). A normal class would compare
        //   references and return false.
        // - 'emp1 with { Salary = 80000 }' creates a COPY of emp1 with only the listed
        //   properties changed; emp1 itself is untouched. Several properties can change at once.
        public static void Run()
        {
            var person1 = new Person("John", "Doe");
            var person2 = new Person("John", "Doe");

            Console.WriteLine(person1); // Person { FirstName = John, LastName = Doe }
            Console.WriteLine(person1 == person2); // True
            Console.WriteLine(ReferenceEquals(person1, person2)); // False

            var emp1 = new Employee("Alice", "Engineering", 75000);
            var emp2 = emp1 with { Salary = 80000 };
            var emp3 = emp1 with { Department = "Management", Salary = 90000 };

            Console.WriteLine(emp1); // Employee { Name = Alice, Department = Engineering, Salary = 75000 }
            Console.WriteLine(emp2); // Employee { Name = Alice, Department = Engineering, Salary = 80000 }
            Console.WriteLine(emp3); // Employee { Name = Alice, Department = Management, Salary = 90000 }
        }
    }

    /*

    More record capabilities:
        var (first, last) = person1;                  // generated Deconstruct
        public record Manager(string Name, string Department, decimal Salary, int Reports)
            : Employee(Name, Department, Salary);     // records support inheritance
        public record Product { public string Name { get; init; } = ""; }  // nominal (non-positional) form

    //1. Equality is SHALLOW: a List<T> property is compared by reference, not by its contents.
    //2. Records are immutable by convention, not by force - you can declare { get; set; } properties.

    C# 10 added 'record struct' (value-type records) - see 10_RecordStructs.cs.

     */
}
