using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class NullConditionalOperator
    {
        public static void Run()
        {
            Person3 person = null;

            // Without null conditional operator (old way)
            if (person != null)
            {
                Console.WriteLine(person.Name);
            }

            // With null conditional operator (new way)
            Console.WriteLine(person?.Name);

            // If person is null, the entire expression evaluates to null
            // If person is not null, it accesses the Name property
        }
    }

    public class Person3
    {
        public string Name { get; set; }
    }
}
