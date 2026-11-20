using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 7 / C# 11.0 (2022)
    public class RequiredMembers
    {
        public static void Run()
        {
            // Usage - must provide all required members
            var user1 = new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Age = 30
            }; // ✅ Valid - all required members provided

            var user2 = new User
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Age = 25,
                PhoneNumber = "+1-555-0123" // Optional
            }; // ✅ Valid

            // This would cause compile error:
            // var user3 = new User { Name = "Bob" }; // ❌ Error - Email and Age required

            Console.WriteLine($"User: {user1.Name} ({user1.Email})");
        }
    }

    // Basic required properties
    public class User
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int Age { get; set; }

        // Optional properties
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }




    /*
        Required members ensure that certain properties or fields must be initialized when creating an 
    object, providing compile-time safety for mandatory data without requiring constructor parameters
     */
}
