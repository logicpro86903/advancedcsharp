// =============================================================================
//  Feature     : Classes and Structs (Reference vs Value Types)
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    C# has two kinds of user-defined types. A CLASS is a REFERENCE type: the
//    variable holds a reference to an object on the heap, so assignment copies
//    the reference and both variables share one object. A STRUCT is a VALUE type:
//    the variable holds the data itself, so assignment copies all the fields.
//    Converting a value type to 'object' (or an interface) is called BOXING and
//    allocates a copy on the heap; casting it back is UNBOXING.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ClassesAndStructs
    {
        public static void Run()
        {
            DemonstrateValueVsReference();
            DemonstrateBoxing();
        }

        // CLASS = reference type. Instances live on the heap and are garbage collected.
        public class PointClass
        {
            public int X;
            public int Y;

            public PointClass(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        // STRUCT = value type. Instances are stored inline (on the stack, or inside the
        // containing object/array). They cannot inherit from other types (but can implement
        // interfaces) and always have a default value with all fields zeroed.
        public struct PointStruct
        {
            public int X;
            public int Y;

            public PointStruct(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        // VALUE vs REFERENCE SEMANTICS
        // - Class: c2 = c1 copies the REFERENCE. Both variables point to the same object, so
        //   changing c2.X is visible through c1.
        // - Struct: s2 = s1 copies the whole VALUE. s1 and s2 are independent, so changing
        //   s2.X leaves s1 untouched.
        public static void DemonstrateValueVsReference()
        {
            PointClass c1 = new PointClass(1, 2);
            PointClass c2 = c1;
            c2.X = 100;
            Console.WriteLine($"Class:  c1.X = {c1.X}, c2.X = {c2.X}"); // Class:  c1.X = 100, c2.X = 100

            PointStruct s1 = new PointStruct(1, 2);
            PointStruct s2 = s1;
            s2.X = 100;
            Console.WriteLine($"Struct: s1.X = {s1.X}, s2.X = {s2.X}"); // Struct: s1.X = 1, s2.X = 100
        }

        // BOXING AND UNBOXING
        // - Boxing: assigning a value type to 'object' copies the value into a NEW heap object.
        // - Unboxing: an explicit cast (int)boxed copies the value back out.
        // - Because the box is a copy, changing the original variable afterwards does not
        //   affect the boxed value.
        public static void DemonstrateBoxing()
        {
            int number = 42;
            object boxed = number;
            int unboxed = (int)boxed;

            number = 7;
            Console.WriteLine($"number = {number}, boxed = {boxed}, unboxed = {unboxed}"); // number = 7, boxed = 42, unboxed = 42
        }
    }

    /*

    //1. Mutable structs are confusing - modifying a copy does nothing to the original
    List<PointStruct> points = ...;
    // points[0].X = 5;  // Compile error: points[0] returns a copy

    //2. Boxing allocates. Old non-generic collections (ArrayList, Hashtable) box every
    //   value type you add - one of the main reasons generics were added in C# 2.

    //3. Use a struct only for small, immutable, value-like data (Point, Money, DateTime).

     */
}
