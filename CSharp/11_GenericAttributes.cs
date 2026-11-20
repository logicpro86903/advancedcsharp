// =============================================================================
//  Feature     : Generic Attributes
//  C# Version  : C# 11.0
//  .NET        : .NET 7
//  Released    : 2022 (Visual Studio 2022 v17.4)
// -----------------------------------------------------------------------------
//  Summary:
//    Attribute classes can now be GENERIC: 'class TypedAttribute<T> : Attribute'
//    used as [Typed<int>(42)]. Before C# 11, attributes that needed a type had
//    to take a System.Type argument - [Typed(typeof(int))] - which couldn't
//    constrain the type or type-check the other arguments against it. Generic
//    attributes give compile-time type safety: the constructor argument must
//    actually be an int when T is int.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class GenericAttributes
    {
        public static void Run()
        {
            DemonstrateGenericAttributes();
        }

        // READING GENERIC ATTRIBUTES
        // - Each class below is decorated with one or more TypedAttribute<T> instances.
        // - PrintTypedAttributes reads them with reflection and prints the type argument and value.
        public static void DemonstrateGenericAttributes()
        {
            Console.WriteLine("=== Generic Attributes Demo ===\n");

            Console.WriteLine("StringProcessor attributes:");
            PrintTypedAttributes(typeof(StringProcessor));

            Console.WriteLine("\nNumberProcessor attributes:");
            PrintTypedAttributes(typeof(NumberProcessor));

            Console.WriteLine("\nDoubleProcessor attributes:");
            PrintTypedAttributes(typeof(DoubleProcessor));

            Console.WriteLine("\nMultiTypedService attributes:");
            PrintTypedAttributes(typeof(MultiTypedService));
        }

        // REFLECTING OVER CLOSED GENERIC ATTRIBUTE TYPES
        // - At runtime each use is a CLOSED type: TypedAttribute<string>, TypedAttribute<int>...
        // - Pattern matching ('attr is TypedAttribute<int> intAttr') identifies which closed type
        //   it is and gives strongly-typed access to DefaultValue (an int, not an object).
        private static void PrintTypedAttributes(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(Attribute), false);

            foreach (var attr in attributes)
            {
                if (attr is TypedAttribute<string> stringAttr)
                {
                    Console.WriteLine($"  TypedAttribute<string>");
                    Console.WriteLine($"    TargetType: {stringAttr.TargetType.Name}");
                    Console.WriteLine($"    DefaultValue: \"{stringAttr.DefaultValue}\"");
                }
                else if (attr is TypedAttribute<int> intAttr)
                {
                    Console.WriteLine($"  TypedAttribute<int>");
                    Console.WriteLine($"    TargetType: {intAttr.TargetType.Name}");
                    Console.WriteLine($"    DefaultValue: {intAttr.DefaultValue}");
                }
                else if (attr is TypedAttribute<double> doubleAttr)
                {
                    Console.WriteLine($"  TypedAttribute<double>");
                    Console.WriteLine($"    TargetType: {doubleAttr.TargetType.Name}");
                    Console.WriteLine($"    DefaultValue: {doubleAttr.DefaultValue}");
                }
            }
        }

        // DECLARING A GENERIC ATTRIBUTE
        // - Derives from Attribute like any attribute, but declares a type parameter T.
        // - The constructor parameter is typed as T, so [Typed<int>("text")] would NOT compile.
        // - AllowMultiple = true lets several instances (even with different T) decorate one class.
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class TypedAttribute<T> : Attribute
        {
            public Type TargetType { get; }
            public T? DefaultValue { get; }

            public TypedAttribute(T? defaultValue = default)
            {
                TargetType = typeof(T);
                DefaultValue = defaultValue;
            }
        }

        // APPLYING GENERIC ATTRIBUTES
        // - The type argument is written in angle brackets: [Typed<string>("default")].
        // - The value must match T (a string for <string>, an int for <int>, ...).
        // - Multiple generic attributes can be stacked on the same class.
        [Typed<string>("default")]
        public class StringProcessor { }

        [Typed<int>(42)]
        public class NumberProcessor { }

        [Typed<double>(3.14)]
        public class DoubleProcessor { }

        [Typed<string>("config")]
        [Typed<int>(100)]
        public class MultiTypedService { }
    }

    /*

    Before C# 11 (still common in older libraries):
        public class TypedAttribute : Attribute { public TypedAttribute(Type t, object value) { ... } }
        [Typed(typeof(int), "oops")]   // compiles - no check that the value is an int

    //1. Type arguments must be fully known at compile time: [Typed<T>] inside a generic class
    //   is not allowed, and neither are dynamic, nullable reference annotations (string?) or tuples.

     */
}
