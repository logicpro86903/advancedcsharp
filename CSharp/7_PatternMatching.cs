using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.7 / C# 7.0 (2017)
    public class PatternMatching
    {
        public static void Run()
        {
            ProcessValue("hello");     // String: HELLO
            ProcessValue(42);          // Number: 84
            ProcessValue(DateTime.Now); // Date: 2024-12-09


            Console.WriteLine(AnalyzeNumber(5));  // Single digit positive
            Console.WriteLine(AnalyzeNumber(-5)); // Negative

            var p = new Person { Name = "John", Age = 30, City = "New York", IsEmployed = true };
            Console.WriteLine(DescribePerson(p));

        }

        public static string DescribePerson(Person person) => person switch
        {
            { Age: < 18 } => "Minor",
            { Age: >= 65, IsEmployed: false } => "Retired",
            { City: "New York", IsEmployed: true } => "Working New Yorker",
            { Name: "John", Age: > 21 } => "Adult John",
            { IsEmployed: false } => "Unemployed",
            _ => "Regular person"
        };

        public static string AnalyzeNumber(int number) => number switch
        {
            0 => "Zero",
            1 => "One",
            -1 => "Negative one",
            > 0 and < 10 => "Single digit positive",
            < 0 => "Negative",
            _ => "Large number"
        };

        public static void ProcessValue(object value)
        {
            // Type pattern with variable declaration
            if (value is string text)
            {
                Console.WriteLine($"String: {text.ToUpper()}");
            }
            else if (value is int number)
            {
                Console.WriteLine($"Number: {number * 2}");
            }
            else if (value is DateTime date)
            {
                Console.WriteLine($"Date: {date:yyyy-MM-dd}");
            }
        }


        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public bool IsEmployed { get; set; }
        }
    }
}
