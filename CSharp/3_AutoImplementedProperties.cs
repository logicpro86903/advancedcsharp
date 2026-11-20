// =============================================================================
//  Feature     : Auto-Implemented Properties
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    '{ get; set; }' declares a property whose private backing field is generated
//    by the compiler. It removes the boilerplate of writing a field plus a
//    get/set pair for every simple property, while keeping the benefits of a
//    property (you can switch to a full property later without breaking callers).
//    Accessor modifiers still work: '{ get; private set; }'.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class AutoImplementedProperties
    {
        // USING AUTO-PROPERTIES
        // - Auto-properties are read and written exactly like hand-written properties.
        // - IsPublished can be read from outside but only changed via Publish(), because
        //   its setter is private.
        public static void Run()
        {
            var book = new Book();
            book.Title = "C# in Depth";
            book.Pages = 528;
            book.Publish();

            Console.WriteLine($"{book.Title}, {book.Pages} pages, published: {book.IsPublished}");
            // C# in Depth, 528 pages, published: True
            // book.IsPublished = false;   // compile error: setter is private
        }

        // BEFORE C# 3: every property needed an explicit private backing field and
        // hand-written get/set bodies, even when they did nothing but read/write the field.
        public class BookOld
        {
            private string _title = "";
            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }
        }

        // C# 3 AUTO-IMPLEMENTED PROPERTIES
        // - '{ get; set; }' : the compiler generates the hidden backing field and the accessors.
        // - '{ get; private set; }' : publicly readable, only writable inside the class.
        // - The '= ""' initializer is a later addition (C# 6); in C# 3 you'd set it in a constructor.
        public class Book
        {
            public string Title { get; set; } = "";
            public int Pages { get; set; }
            public bool IsPublished { get; private set; }

            public void Publish() { IsPublished = true; }
        }
    }

    /*

    Evolution:
    C# 3   { get; set; }  / { get; private set; }
    C# 6   { get; }  get-only, and initializers  = value  (see 6_AutoPropertyInitializers.cs)
    C# 9   { get; init; } (see 9_InitOnlySetters.cs)
    C# 11  required (see 11_RequiredMembers.cs)
    C# 14  'field' keyword to access the generated backing field (see 13_BackingFields.cs)

     */
}
