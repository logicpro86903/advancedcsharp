// =============================================================================
//  Feature     : Generic Math (static abstract / static virtual interface members)
//  C# Version  : C# 11.0
//  .NET        : .NET 7
//  Released    : 2022 (Visual Studio 2022 v17.4)
// -----------------------------------------------------------------------------
//  Summary:
//    Interfaces can declare 'static abstract' (and 'static virtual') members.
//    A type implementing the interface must then provide those STATIC members -
//    including OPERATORS (+, -, *, ==, ...). .NET 7 used this to build "generic
//    math": INumber<T>, IAdditionOperators<,,>, etc. Before C# 11 you could NOT
//    write one generic Sum<T> for int, double and decimal ('a + b' doesn't compile
//    for an unconstrained T), so the same method was written once per numeric
//    type. Now a single generic method covers them all - and your own types too.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSharp
{
    public class GenericMath
    {
        public static void Run()
        {
            DemonstrateTheProblem();
            DemonstrateINumber();
            DemonstrateCustomStaticAbstract();
            DemonstrateCustomNumericType();
        }

        // THE PROBLEM GENERIC MATH SOLVES
        // - Before C# 11 you needed one copy per numeric type (SumInts, SumDoubles, ... decimal, long...).
        // - A generic version never compiled, because an unconstrained T has no '+' operator and
        //   no way to express "zero":
        //       static T Sum<T>(T[] values) { T total = 0; ... total += v; }   // error
        static int SumInts(int[] values) { int total = 0; foreach (var v in values) total += v; return total; }
        static double SumDoubles(double[] values) { double total = 0; foreach (var v in values) total += v; return total; }

        public static void DemonstrateTheProblem()
        {
            Console.WriteLine($"Old way: {SumInts(new[] { 1, 2, 3 })}, {SumDoubles(new[] { 1.5, 2.5 })}");
        }

        // ONE GENERIC METHOD FOR EVERY NUMERIC TYPE
        // - 'where T : INumber<T>' requires +, -, *, /, comparisons, T.Zero, T.One, parsing...
        //   all declared as STATIC members of the interface.
        // - T.Zero calls a static property ON THE TYPE PARAMETER - new in C# 11.
        // - T.CreateChecked(int) converts another number into T (throws on overflow).
        // - You can constrain to ONLY what you need: IAdditionOperators<T, T, T> just means
        //   "has a + operator returning T".
        static T Sum<T>(IEnumerable<T> values) where T : INumber<T>
        {
            T total = T.Zero;
            foreach (T v in values)
                total += v;
            return total;
        }

        static T Average<T>(IReadOnlyList<T> values) where T : INumber<T>
        {
            T sum = Sum(values);
            T count = T.CreateChecked(values.Count);
            return sum / count;
        }

        static T AddAll<T>(T first, params T[] rest) where T : IAdditionOperators<T, T, T>
        {
            T result = first;
            foreach (T r in rest) result = result + r;
            return result;
        }

        // USING THE GENERIC NUMERIC METHODS
        // - The same Sum works for int, double and decimal; Average<int> uses integer division.
        // - AddAll works for long and float (and BigInteger, or your own types - see Money below).
        // - Smaller interfaces expose other static members: IMinMaxValue<T>.MaxValue,
        //   IBinaryInteger<T>.IsPow2.
        public static void DemonstrateINumber()
        {
            Console.WriteLine($"Sum<int>:     {Sum(new[] { 1, 2, 3 })}");            // 6
            Console.WriteLine($"Sum<double>:  {Sum(new[] { 1.5, 2.5 })}");           // 4
            Console.WriteLine($"Sum<decimal>: {Sum(new[] { 0.1m, 0.2m })}");         // 0.3
            Console.WriteLine($"Average<double>: {Average(new[] { 2.0, 4.0, 9.0 })}"); // 5
            Console.WriteLine($"Average<int>:    {Average(new[] { 2, 4, 9 })}");       // 5

            Console.WriteLine($"AddAll<long>:  {AddAll(10L, 20L, 30L)}");                 // 60
            Console.WriteLine($"AddAll<float>: {AddAll(0.5f, 0.25f)}");                  // 0.75

            Console.WriteLine($"int.MaxValue via IMinMaxValue: {GetMax<int>()}");
            Console.WriteLine($"IsPow2(64): {IsPowerOfTwo(64)}, IsPow2(65): {IsPowerOfTwo(65)}");
        }

        static T GetMax<T>() where T : IMinMaxValue<T> => T.MaxValue;
        static bool IsPowerOfTwo<T>(T value) where T : IBinaryInteger<T> => T.IsPow2(value);

        // YOUR OWN STATIC ABSTRACT MEMBERS (not just math)
        // - 'static abstract' : every implementing type MUST provide this static member
        //   (here a static property ShapeName and a static factory Create).
        // - 'static virtual'  : has a default implementation that types MAY override (Describe).
        //   Circle overrides it with an explicit implementation 'static string IShape.Describe()'.
        // - This enables the "static factory / type metadata" pattern in generic code.
        public interface IShape
        {
            static abstract string ShapeName { get; }
            static abstract IShape Create(double size);
            static virtual string Describe() => "a shape";
            double Area { get; }
        }

        public class Square : IShape
        {
            public double Side { get; init; }
            public static string ShapeName => "Square";
            public static IShape Create(double size) => new Square { Side = size };
            public double Area => Side * Side;
        }

        public class Circle : IShape
        {
            public double Radius { get; init; }
            public static string ShapeName => "Circle";
            public static IShape Create(double size) => new Circle { Radius = size };
            static string IShape.Describe() => "a round shape";
            public double Area => Math.PI * Radius * Radius;
        }

        // CALLING STATIC MEMBERS THROUGH A TYPE PARAMETER
        // - T.Create(size), T.ShapeName and T.Describe() are resolved for the concrete type
        //   passed as T (Square or Circle) - no instance or reflection needed.
        static void Build<T>(double size) where T : IShape
        {
            IShape shape = T.Create(size);
            Console.WriteLine($"{T.ShapeName} ({T.Describe()}) of size {size}: area {shape.Area:F2}");
        }

        public static void DemonstrateCustomStaticAbstract()
        {
            Build<Square>(3);   // Square (a shape) of size 3: area 9.00
            Build<Circle>(1);   // Circle (a round shape) of size 1: area 3.14
        }

        // MAKING YOUR OWN TYPE WORK WITH GENERIC MATH
        // - Implement just the operator interfaces you need:
        //     IAdditionOperators<Money, Money, Money> -> requires 'operator +'
        //     IAdditiveIdentity<Money, Money>          -> requires a static AdditiveIdentity ("zero")
        // - Total<T> is constrained to exactly those two interfaces, so it works for int, double
        //   AND Money with no special-casing.
        public readonly record struct Money(decimal Amount)
            : IAdditionOperators<Money, Money, Money>,
              IAdditiveIdentity<Money, Money>
        {
            public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);
            public static Money AdditiveIdentity => new(0m);
            public override string ToString() => $"${Amount:F2}";
        }

        static T Total<T>(IEnumerable<T> items) where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            T total = T.AdditiveIdentity;
            foreach (var item in items) total += item;
            return total;
        }

        public static void DemonstrateCustomNumericType()
        {
            var cart = new List<Money> { new(19.99m), new(5.01m), new(75m) };
            Console.WriteLine($"Cart total: {Total(cart)}");           // Cart total: $100.00
            Console.WriteLine($"Int total:  {Total(new[] { 1, 2, 3 })}"); // Int total:  6
        }
    }

    /*

    Key interfaces in System.Numerics (pick the smallest one you need):
        INumber<T>                   general numbers (int, double, decimal, ...)
        IBinaryInteger<T>            integers only (bit ops, IsPow2, ...)
        IFloatingPoint<T>            float, double, Half, decimal
        IAdditionOperators<T,T,T>    just '+'      (also: ISubtractionOperators, IMultiplyOperators, ...)
        IAdditiveIdentity<T,T>       a "zero"      (IMultiplicativeIdentity = a "one")
        IMinMaxValue<T>              MinValue / MaxValue
        IParsable<T>, ISpanParsable<T>   T.Parse(string) for generic parsing

    //1. Interfaces with static abstract members can only be used as CONSTRAINTS, not as
    //   variable types:  INumber<int> x = 5;  // error (CS8920)

    //2. T.CreateChecked throws on overflow, CreateSaturating clamps, CreateTruncating wraps.

    //3. You mostly CONSUME generic math (write generic algorithms). Implementing the full INumber<T>
    //   on your own type is a lot of members - implement just the operator interfaces you need.

    Also added in C# 11 for this: checked user-defined operators, the unsigned right shift >>>,
    and relaxed shift operator requirements.

     */
}
