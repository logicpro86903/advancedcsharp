using static System.Console;
using static System.Math;
using static System.String;

namespace CSharp
{
    //.NET 4.6 / C# 6.0 (2015)
    public class StaticDirectives
    {
        public static void Run()
        {
            // Without using static (traditional way)
            System.Console.WriteLine("Hello");
            double result = System.Math.Sqrt(16);
            bool isEmpty = System.String.IsNullOrEmpty("test");

            // With using static (cleaner syntax)
            WriteLine("Hello World!");          // Console.WriteLine
            double sqrtResult = Sqrt(25);       // Math.Sqrt
            bool isNull = IsNullOrEmpty("");    // String.IsNullOrEmpty

            WriteLine($"Square root of 25 is {sqrtResult}");
        }
    }

    //Static using directives (also called "using static") were introduced in C# 6.0
    //and allow you to import static members of a class directly, 
    //so you can use them without qualifying them with the class name.


}
