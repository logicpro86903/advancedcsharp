using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET Core 3.x / .NET 5 / C# 8.0 (2019)
    public class NullableReferenceTypes
    {
        public static void Run()
        {
            DemonstrateBasicUsage();    
            ProcessNames("John", null, "Doe");
        }

        // Non-nullable reference type
        public static string Name { get; set; } = string.Empty;

        // Nullable reference type
        public static string? NickName { get; set; }

        // Non-nullable collection
        public static List<string> Tags { get; set; } = new List<string>();

        // Nullable collection
        public static List<string>? OptionalTags { get; set; }

        public static void DemonstrateBasicUsage()
        {
            // This is safe - Name is non-nullable
            Console.WriteLine($"Name length: {Name.Length}");

            // This generates a warning without null check
            Console.WriteLine($"Nickname length: {NickName.Length}"); // Warning!

            // Proper null checking
            if (NickName != null)
            {
                Console.WriteLine($"Nickname length: {NickName.Length}");
            }

            // Using null-conditional operator
            Console.WriteLine($"Nickname length: {NickName?.Length ?? 0}");

            // Null-forgiving operator (use with caution)
            Console.WriteLine($"Nickname length: {NickName!.Length}"); // Tells compiler: "I know this isn't null"
        }

        public static void ProcessNames(string firstName, string? middleName, string lastName)
        {
            // firstName and lastName are non-nullable
            string fullName = $"{firstName} {lastName}";

            if (middleName != null)
            {
                fullName = $"{firstName} {middleName} {lastName}";
            }

            // Alternative using null-conditional
            string alternativeFullName = middleName != null
                ? $"{firstName} {middleName} {lastName}"
                : $"{firstName} {lastName}";

            Console.WriteLine($"Full name: {fullName}");
        }
    }

    /*
     //Nullable reference types, introduced in C# 8.0, help prevent null reference exceptions 
    //by making the nullable nature of reference types explicit in the type system. 
    //This feature allows the compiler to provide warnings about potential null reference 
    //issues at compile time
     */
}
