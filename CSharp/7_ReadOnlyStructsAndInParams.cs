// =============================================================================
//  Feature     : readonly struct and 'in' Parameters
//  C# Version  : C# 7.2
//  .NET        : .NET Core 2.1 / .NET Framework 4.7.2
//  Released    : 2017 (Visual Studio 2017 v15.5)
// -----------------------------------------------------------------------------
//  Summary:
//    'readonly struct' declares a struct whose state can never change after
//    construction; the compiler enforces it and can skip hidden "defensive
//    copies". An 'in' parameter passes a value by READ-ONLY reference: no copy
//    is made (fast, like 'ref') but the method can't modify it (safe, like
//    pass-by-value). Together they let large value types be passed around
//    efficiently without sacrificing immutability.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class ReadOnlyStructsAndInParams
    {
        public static void Run()
        {
            DemonstrateReadOnlyStruct();
            DemonstrateInParameters();
            DemonstrateDefensiveCopies();
        }

        // READONLY STRUCT
        // - Every field must be 'readonly' and every auto-property get-only (or init, C# 9);
        //   the compiler rejects anything that could mutate the struct after construction.
        // - "Changing" operations return a NEW value instead (Scale returns a new Vector3).
        // - A method like 'void Reset() { X = 0; }' would be a compile error.
        public readonly struct Vector3
        {
            public double X { get; }
            public double Y { get; }
            public double Z { get; }

            public Vector3(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

            public Vector3 Scale(double factor) => new Vector3(X * factor, Y * factor, Z * factor);

            public override string ToString() => $"({X}, {Y}, {Z})";
        }

        // USING A READONLY STRUCT: Scale produces a new value; the original 'v' is unchanged,
        // and assigning v.X directly does not compile.
        public static void DemonstrateReadOnlyStruct()
        {
            var v = new Vector3(1, 2, 2);
            var doubled = v.Scale(2);
            Console.WriteLine($"v = {v}, length {v.Length}");     // v = (1, 2, 2), length 3
            Console.WriteLine($"doubled = {doubled}");            // doubled = (2, 4, 4)
            // v.X = 5;                                          // compile error
        }

        // A deliberately "large" struct (8 doubles = 64 bytes) - copying it on every call adds up.
        public readonly struct Matrix2x4
        {
            public readonly double A, B, C, D, E, F, G, H;
            public Matrix2x4(double v) { A = B = C = D = E = F = G = H = v; }
            public double Sum => A + B + C + D + E + F + G + H;
        }

        // BY VALUE vs BY 'in'
        // - SumByValue: the whole 64-byte struct is COPIED into the method on every call.
        // - SumByIn:    only a reference (8 bytes) is passed; assigning to 'm' is a compile
        //   error because 'in' parameters are read-only.
        static double SumByValue(Matrix2x4 m) => m.Sum;

        static double SumByIn(in Matrix2x4 m)
        {
            // m = new Matrix2x4(0);     // compile error: 'in' parameters are read-only
            return m.Sum;
        }

        static double Distance(in Vector3 a, in Vector3 b)
        {
            double dx = a.X - b.X, dy = a.Y - b.Y, dz = a.Z - b.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // CALLING METHODS WITH 'in' PARAMETERS
        // - Writing 'in' at the call site is OPTIONAL (unlike ref/out); the argument is passed
        //   by reference either way.
        // - You can even pass a TEMPORARY value (new Vector3(...)); the compiler stores it in a
        //   hidden local variable and passes a reference to that.
        public static void DemonstrateInParameters()
        {
            var matrix = new Matrix2x4(1.5);
            Console.WriteLine($"By value: {SumByValue(matrix)}, by in: {SumByIn(in matrix)}"); // 12, 12

            Console.WriteLine($"Without 'in' keyword at call site: {SumByIn(matrix)}");

            var a = new Vector3(0, 0, 0);
            var b = new Vector3(3, 4, 0);
            Console.WriteLine($"Distance: {Distance(a, b)}");   // 5

            Console.WriteLine($"Distance to temp: {Distance(a, new Vector3(0, 0, 2))}"); // 2
        }

        // WHY 'readonly' MATTERS FOR 'in': HIDDEN DEFENSIVE COPIES
        // - MutableCounter is NOT readonly, so the compiler can't be sure Peek() won't modify it.
        // - To honor the "caller's value can't change" promise of 'in', the compiler silently
        //   COPIES 'c' before calling Peek() - which defeats the purpose of passing by reference.
        // - With a readonly struct (or 'readonly' members, C# 8) no copy is needed.
        public struct MutableCounter
        {
            public int Value;
            public int Peek() { return Value; }
        }

        static int ReadCounter(in MutableCounter c)
        {
            return c.Peek();
        }

        public static void DemonstrateDefensiveCopies()
        {
            var counter = new MutableCounter { Value = 7 };
            Console.WriteLine($"Read via in: {ReadCounter(counter)}"); // 7 - works, but with a hidden copy
        }
    }

    /*

    Guidelines:
    - Make value-like structs 'readonly struct' by default (Point, Money, DateRange...).
    - Use 'in' for LARGE readonly structs (> ~16 bytes) on hot paths. For small ones (int, a
      2-field struct) plain pass-by-value is just as fast or faster.
    - Only combine 'in' with readonly structs - otherwise you get defensive copies (see above).

    Related features:
    - C# 7.2 'ref readonly' returns:  ref readonly Vector3 GetOrigin()
    - C# 8 'readonly' members on non-readonly structs (see 8_ReadOnlyMembers.cs)
    - C# 10 'readonly record struct' (see 10_RecordStructs.cs)
    - C# 12 'ref readonly' parameters - like 'in' but the caller must pass a variable
    - Other C# 7.2 additions: 'private protected' access, non-trailing named arguments,
      leading underscore in numeric literals (0b_1010)

     */
}
