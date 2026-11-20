using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class AutoPropertyInitializers
    {
        public static void Run()
        {
            var emp = new Employee();
            Console.WriteLine($"Name: {emp.Name}, Age: {emp.Age}"); 
            Console.WriteLine($"FirstName: {emp.FirstName}, LastName: {emp.LastName}, CurrentAge: {emp.CurrentAge}, IsActive: {emp.IsActive}, CreatedDate: {emp.CreatedDate}");

        }
    }

    public class Employee
    {
        // Traditional approach (before C# 6.0)
        public string Name { get; set; }
        public int Age { get; set; }

        public Employee()
        {
            Name = "Tom";
            Age = 28;
        }

        // Auto Property Initializers (C# 6.0+)
        public string FirstName { get; set; } = "John";
        public string LastName { get; set; } = string.Empty;
        public int CurrentAge { get; set; } = 33;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
