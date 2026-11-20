using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 6 / C# 10.0 (2021)
    public class RecordStructs
    {
        public record struct Point(int X, int Y);
        public static void Run()
        {
            var point1 = new Point(10, 20);
            var point2 = new Point(10, 20);

            Console.WriteLine(point1 == point2); // True - value equality
            Console.WriteLine(point1); // Point { X = 10, Y = 20 }
        }

    }
}


    /*
     Record structs combine the value semantics and immutability benefits of records 
    with the performance characteristics of structs, providing value-based equality 
    and copy semantics without heap allocation
    */
