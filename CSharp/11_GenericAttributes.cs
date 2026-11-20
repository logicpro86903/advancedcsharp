using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 7 / C# 11.0 (2022)
    public class GenericAttributes
    {
        public static void Run()
        {
            DemonstrateGenericAttributes();
        }

        public static void DemonstrateGenericAttributes()
        {
            Console.WriteLine("=== Generic Attributes Demo ===\n");

            // Demonstrate StringProcessor
            Console.WriteLine("StringProcessor attributes:");
            PrintTypedAttributes(typeof(StringProcessor));

            // Demonstrate NumberProcessor
            Console.WriteLine("\nNumberProcessor attributes:");
            PrintTypedAttributes(typeof(NumberProcessor));

            // Demonstrate DoubleProcessor
            Console.WriteLine("\nDoubleProcessor attributes:");
            PrintTypedAttributes(typeof(DoubleProcessor));

            // Demonstrate MultiTypedService (multiple attributes)
            Console.WriteLine("\nMultiTypedService attributes:");
            PrintTypedAttributes(typeof(MultiTypedService));
        }

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


        // Usage with different types
        [Typed<string>("default")]
        public class StringProcessor { }

        [Typed<int>(42)]
        public class NumberProcessor { }

        [Typed<double>(3.14)]
        public class DoubleProcessor { }

        // Multiple generic attributes
        [Typed<string>("config")]
        [Typed<int>(100)]
        public class MultiTypedService { }



    }

    /*
       Generic attributes allow you to create attribute classes that can accept type parameters, 
    enabling more flexible and type-safe attribute-based programming patterns.
     */
}
