// =============================================================================
//  Feature     : Init-Only Setters ({ get; init; })
//  C# Version  : C# 9.0
//  .NET        : .NET 5
//  Released    : 2020 (Visual Studio 2019 v16.8)
// -----------------------------------------------------------------------------
//  Summary:
//    An 'init' accessor is a setter that can only be called during object
//    construction - in the constructor or in an OBJECT INITIALIZER. After that,
//    the property is read-only. This finally lets you build IMMUTABLE objects
//    with the friendly 'new T { Prop = value }' syntax, instead of requiring a
//    constructor with a long parameter list. Records use init accessors for
//    their positional properties.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class InitOnlySetters
    {
        // SETTING INIT-ONLY PROPERTIES
        // - Assigning them inside the object initializer is allowed.
        // - Once the object is constructed they are frozen: 'person.FirstName = "Jane"' is a
        //   compile error (CS8852: init-only property can only be assigned in an object
        //   initializer, or on 'this' / 'base' in a constructor or init accessor).
        public static void Run()
        {
            var person = new Person2
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                BirthDate = new DateTime(1993, 5, 15)
            };

            // person.FirstName = "Jane";   // compile error CS8852
        }
    }

    // DECLARING INIT-ONLY PROPERTIES
    // - '{ get; init; }' replaces '{ get; set; }'. Compared with '{ get; }' (get-only), init
    //   properties can be set from an object initializer, not just from a constructor.
    public class Person2
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; init; }
        public DateTime BirthDate { get; init; }
    }
}

/*

    //1. 'init' doesn't force callers to set the property - add 'required' (C# 11) for that
    //   (see 11_RequiredMembers.cs).
    //2. Immutability is shallow: an init-only List<T> property can't be replaced, but the list's
    //   contents can still be changed.
    //3. 'with' expressions (9_WithExpressions.cs) work by copying and then calling init accessors.

 */
