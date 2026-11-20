// =============================================================================
//  Feature     : Inheritance, Polymorphism and Interfaces
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    A class can INHERIT from one base class, reusing its members and overriding
//    'virtual'/'abstract' members to change behavior. Calls through a base-type
//    reference are dispatched to the runtime type's override (POLYMORPHISM).
//    An INTERFACE is a pure contract (members without implementation); a class
//    can implement MANY interfaces. Abstract classes share implementation,
//    'sealed' blocks further inheritance, and 'is'/'as' perform runtime type checks.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class InheritanceAndInterfaces
    {
        public static void Run()
        {
            DemonstratePolymorphism();
            DemonstrateInterfaces();
        }

        // INTERFACE: declares WHAT a type can do, never HOW (no fields, no bodies - until
        // C# 8 default interface methods). Any implementing type must provide every member.
        public interface IShape
        {
            double Area();
            string Name { get; }
        }

        // ABSTRACT BASE CLASS
        // - Cannot be instantiated directly (new Shape() is a compile error).
        // - 'abstract' members have no body and MUST be overridden by derived classes.
        // - 'virtual' members have a default body and MAY be overridden.
        // - Non-virtual members (Describe) are shared, reusable behavior for all subclasses.
        public abstract class Shape : IShape
        {
            public abstract double Area();
            public virtual string Name { get { return "Shape"; } }

            public string Describe()
            {
                return Name + " with area " + Area().ToString("F2");
            }
        }

        // DERIVED CLASSES: ': Shape' inherits; 'override' supplies the specific implementation.
        public class Circle : Shape
        {
            private double _radius;
            public Circle(double radius) { _radius = radius; }

            public override double Area() { return Math.PI * _radius * _radius; }
            public override string Name { get { return "Circle"; } }
        }

        public class Rectangle : Shape
        {
            protected double Width;     // 'protected' = visible to this class and derived classes only
            protected double Height;

            public Rectangle(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public override double Area() { return Width * Height; }
            public override string Name { get { return "Rectangle"; } }
        }

        // SEALED CLASS + BASE CONSTRUCTOR CALL
        // - 'sealed' prevents anyone from deriving from Square.
        // - ': base(side, side)' chains to the Rectangle constructor before Square's body runs.
        public sealed class Square : Rectangle
        {
            public Square(double side) : base(side, side) { }
            public override string Name { get { return "Square"; } }
        }

        // MULTIPLE INTERFACES + EXPLICIT INTERFACE IMPLEMENTATION
        // - C# has single class inheritance, but a class can implement any number of interfaces.
        // - 'int IComparable.CompareTo(...)' is an EXPLICIT implementation: the method is not
        //   part of Report's public surface and is only callable through an IComparable reference.
        //   Useful to hide plumbing members or resolve name clashes between two interfaces.
        public interface IPrintable { void Print(); }

        public class Report : IPrintable, IComparable
        {
            public int Pages;
            public Report(int pages) { Pages = pages; }

            public void Print() { Console.WriteLine($"Printing report with {Pages} pages"); }

            int IComparable.CompareTo(object? obj)
            {
                return Pages.CompareTo(((Report)obj!).Pages);
            }
        }

        // POLYMORPHISM (VIRTUAL DISPATCH) AND TYPE CHECKS
        // - The array is typed as Shape[], but each element keeps its real runtime type.
        // - shape.Describe() calls Area()/Name, and the runtime picks the override belonging
        //   to the actual object (Circle, Rectangle or Square).
        // - 'is' tests whether an object is compatible with a type (true / false).
        // - 'as' attempts a cast and returns null on failure instead of throwing.
        public static void DemonstratePolymorphism()
        {
            Shape[] shapes = { new Circle(1), new Rectangle(2, 3), new Square(4) };

            foreach (Shape shape in shapes)
            {
                Console.WriteLine(shape.Describe());
            }
            // Circle with area 3.14
            // Rectangle with area 6.00
            // Square with area 16.00

            Shape s = shapes[2];
            if (s is Rectangle)
                Console.WriteLine("Square is a Rectangle");

            Circle? c = s as Circle;
            Console.WriteLine($"Square as Circle is null: {c == null}"); // True
        }

        // PROGRAMMING AGAINST INTERFACES
        // - A variable of interface type can hold any implementing object.
        // - Framework code also works through interfaces: Array.Sort uses IComparable.CompareTo
        //   to order the reports, even though it is implemented explicitly.
        public static void DemonstrateInterfaces()
        {
            IPrintable printable = new Report(12);
            printable.Print();

            Report[] reports = { new Report(30), new Report(5), new Report(12) };
            Array.Sort(reports);
            foreach (Report r in reports)
                Console.Write(r.Pages + " "); // 5 12 30
            Console.WriteLine();
        }
    }

    /*

    //1. Forgetting 'override' and writing 'new' (or nothing) HIDES the base member instead of
    //   overriding it - calls through a base reference will still run the base version.

    //2. Prefer interfaces for contracts and composition; use abstract classes when
    //   you need to share real implementation/state.

     */
}
