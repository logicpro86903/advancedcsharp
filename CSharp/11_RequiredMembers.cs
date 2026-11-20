// =============================================================================
//  Feature     : Required Members (required modifier)
//  C# Version  : C# 11.0
//  .NET        : .NET 7
//  Released    : 2022 (Visual Studio 2022 v17.4)
// -----------------------------------------------------------------------------
//  Summary:
//    Marking a property or field 'required' forces every caller to set it in the
//    OBJECT INITIALIZER when creating the object; forgetting one is a compile
//    error (CS9035). This gives the safety of constructor parameters while
//    keeping the readable 'new T { Prop = value }' syntax, and works nicely with
//    nullable reference types: a 'required string Name' doesn't need a dummy
//    default like = "" to avoid nullable warnings.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class RequiredMembers
    {
        // CREATING OBJECTS WITH REQUIRED MEMBERS
        // - user1 sets all three required members (Name, Email, Age) -> compiles.
        // - user2 additionally sets an optional property (PhoneNumber) -> compiles.
        // - 'new User { Name = "Bob" }' would NOT compile: Email and Age are required
        //   (error CS9035: Required member 'User.Email' must be set in the object initializer).
        public static void Run()
        {
            var user1 = new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Age = 30
            };

            var user2 = new User
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Age = 25,
                PhoneNumber = "+1-555-0123"
            };

            // var user3 = new User { Name = "Bob" };   // compile error CS9035

            Console.WriteLine($"User: {user1.Name} ({user1.Email})");
        }
    }

    // DECLARING REQUIRED MEMBERS
    // - 'required' goes before the type: public required string Name { get; set; }
    // - Works with { get; set; } and { get; init; } (init + required = must set once, then immutable).
    // - Members without 'required' stay optional and can have default values.
    public class User
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int Age { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

    /*

    //1. A constructor that sets the required members can say so with [SetsRequiredMembers];
    //   callers using that constructor then don't have to set them again.
    //2. The check is compile-time only - reflection or deserializers may bypass it
    //   (System.Text.Json does honor 'required' since .NET 7).
    //3. 'required' doesn't validate the VALUE - Name = "" still compiles.

     */
}
