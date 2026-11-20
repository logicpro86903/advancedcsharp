// =============================================================================
//  Feature     : Static Classes
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    A 'static class' cannot be instantiated or inherited and may contain only
//    static members. It is the natural home for stateless helper functions and
//    constants (Math, Path, File, Convert) and is REQUIRED for extension methods.
//    Static constructors (C# 1) run once, automatically, before the type is
//    first used - handy for one-time initialization of static fields.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class StaticClasses
    {
        public static void Run()
        {
            DemonstrateStaticClass();
            DemonstrateStaticConstructor();
        }

        // STATIC CLASS
        // - The compiler enforces: no instances, no inheritance, no instance members.
        // - In C# 1 the workaround was a sealed class with a private constructor, which
        //   didn't stop someone from accidentally adding instance members.
        // - Members are accessed through the class name: TemperatureConverter.CelsiusToFahrenheit(...).
        public static class TemperatureConverter
        {
            public const double FreezingPointF = 32;

            public static double CelsiusToFahrenheit(double c) { return c * 9 / 5 + 32; }
            public static double FahrenheitToCelsius(double f) { return (f - 32) * 5 / 9; }
        }

        public static void DemonstrateStaticClass()
        {
            Console.WriteLine(TemperatureConverter.CelsiusToFahrenheit(100)); // 212
            Console.WriteLine(TemperatureConverter.FahrenheitToCelsius(32));  // 0
            // new TemperatureConverter();   // compile error: cannot create an instance of a static class
        }

        // STATIC CONSTRUCTOR
        // - Declared as 'static ClassName()' with no parameters and no access modifier.
        // - The runtime calls it exactly ONCE, automatically, just before the first access
        //   to any static member of the type. You can never call it yourself.
        // - Ideal for initializing static readonly fields that need logic to compute.
        public static class AppConfig
        {
            public static readonly string Environment;
            public static readonly DateTime LoadedAt;

            static AppConfig()
            {
                Console.WriteLine("  (static constructor running)");
                Environment = "Development";
                LoadedAt = DateTime.Now;
            }
        }

        // OBSERVING STATIC CONSTRUCTOR TIMING
        // - "Before first access" prints first; the static constructor runs at the first
        //   AppConfig.Environment read, and does NOT run again on the second read.
        public static void DemonstrateStaticConstructor()
        {
            Console.WriteLine("Before first access to AppConfig");
            Console.WriteLine($"Env: {AppConfig.Environment}");
            Console.WriteLine($"Env again: {AppConfig.Environment}");
        }
    }

    /*

    Extension methods MUST live in a static class - see 3_ExtensionMethods.cs.

    //1. Static mutable state is global state - hard to test and not thread-safe by default.
    //2. An exception in a static constructor makes the type unusable for the rest of the
    //   process (TypeInitializationException on every access).

     */
}
