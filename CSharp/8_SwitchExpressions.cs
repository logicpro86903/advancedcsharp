using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class SwitchExpressions
    {
        public static void Run()
        {
            GetDayTypeTraditional(DayOfWeek.Wednesday);
            GetDayType(DayOfWeek.Sunday);
        }


        public static string GetDayTypeTraditional(DayOfWeek day)
        {
            string result;
            switch (day)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    result = "Weekday";
                    break;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    result = "Weekend";
                    break;
                default:
                    result = "Unknown";
                    break;
            }
            return result;
        }

        // Switch expression (C# 8.0+)
        public static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday
                or DayOfWeek.Thursday or DayOfWeek.Friday => "Weekday",
            DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
            _ => "Unknown"
        };

    }


    /*
     //Switch expressions, introduced in C# 8.0, provide a more concise and functional way to write 
    //switch statements. They return values directly and use a more compact syntax, 
    //making code more readable and expressive.
     */
}
