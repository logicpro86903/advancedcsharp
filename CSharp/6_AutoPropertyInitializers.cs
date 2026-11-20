// =============================================================================
//  Feature     : Auto-Property Initializers
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    Auto-implemented properties can be given an initial value directly in their
//    declaration: 'public bool IsActive { get; set; } = true;'. Previously the
//    default had to be assigned in a constructor. C# 6 also added GET-ONLY
//    auto-properties ('{ get; }'), which can be set only by an initializer or in
//    the constructor - an easy way to build immutable types.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class AutoPropertyInitializers
    {
        // USING THE INITIALIZED PROPERTIES
        // - Name/Age were set by the constructor; FirstName, LastName, CurrentAge, IsActive and
        //   CreatedDate got their values from their initializers - no constructor code needed.
        public static void Run()
        {
            var emp = new Employee();
            Console.WriteLine($"Name: {emp.Name}, Age: {emp.Age}");
            Console.WriteLine($"FirstName: {emp.FirstName}, LastName: {emp.LastName}, CurrentAge: {emp.CurrentAge}, IsActive: {emp.IsActive}, CreatedDate: {emp.CreatedDate}");
        }
    }

    public class Employee
    {
        // BEFORE C# 6: defaults for auto-properties had to be assigned in the constructor,
        // separating each property from its initial value.
        public string Name { get; set; }
        public int Age { get; set; }

        public Employee()
        {
            Name = "Tom";
            Age = 28;
        }

        // C# 6 AUTO-PROPERTY INITIALIZERS
        // - '= value' after the accessor list sets the initial value of the backing field.
        // - Initializers run BEFORE the constructor body, in declaration order, so a constructor
        //   can still override them.
        // - Any expression is allowed (DateTime.Now, string.Empty, new List<T>()), but it cannot
        //   reference 'this' or other instance members.
        public string FirstName { get; set; } = "John";
        public string LastName { get; set; } = string.Empty;
        public int CurrentAge { get; set; } = 33;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

/*

    Get-only auto-properties (also C# 6) pair naturally with initializers:
        public Guid Id { get; } = Guid.NewGuid();   // read-only after construction

    //1. Initializers are evaluated once PER INSTANCE - DateTime.Now gives each object its own time.

 */
