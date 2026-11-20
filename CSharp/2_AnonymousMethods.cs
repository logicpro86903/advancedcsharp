// =============================================================================
//  Feature     : Anonymous Methods and Method Group Conversion
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    C# 2 made delegates far less verbose. METHOD GROUP CONVERSION lets you
//    assign a method name directly to a delegate (g = SayHello) without
//    'new Greeter(...)'. ANONYMOUS METHODS let you write the delegate's body
//    inline with the 'delegate' keyword, and they can CAPTURE local variables
//    from the surrounding method (closures). .NET 2.0 also shipped ready-made
//    generic delegates: Predicate<T>, Comparison<T>, Converter<TIn,TOut>.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class AnonymousMethods
    {
        public static void Run()
        {
            DemonstrateMethodGroupConversion();
            DemonstrateAnonymousMethods();
            DemonstrateClosures();
            DemonstrateGenericDelegates();
        }

        public delegate void Greeter(string name);

        static void SayHello(string name) { Console.WriteLine($"Hello, {name}"); }

        // METHOD GROUP CONVERSION
        // - C# 1 required wrapping the method explicitly: new Greeter(SayHello).
        // - C# 2 lets you just write the method name; the compiler creates the delegate.
        //   Both lines below produce exactly the same thing.
        public static void DemonstrateMethodGroupConversion()
        {
            Greeter g1 = new Greeter(SayHello);
            Greeter g2 = SayHello;

            g1("C# 1");
            g2("C# 2");
        }

        // ANONYMOUS METHODS
        // - 'delegate (params) { body }' defines a method INLINE, with no name, right where
        //   it is needed - no separate named method required.
        // - If the body doesn't use the parameters, the parameter list can be omitted
        //   entirely ('delegate { ... }') and it still converts to any delegate signature.
        // - Very handy with List<T> methods that accept delegates: FindAll (filter) and
        //   Sort (custom comparison - here descending order).
        public static void DemonstrateAnonymousMethods()
        {
            Greeter shout = delegate (string name)
            {
                Console.WriteLine($"HEY {name.ToUpper()}!");
            };
            shout("Alice");

            Greeter ignore = delegate { Console.WriteLine("(ignoring the name)"); };
            ignore("Bob");

            List<int> numbers = new List<int>(new int[] { 5, 12, 8, 21, 3 });
            List<int> big = numbers.FindAll(delegate (int n) { return n > 7; });
            Console.WriteLine($"Greater than 7: {string.Join(", ", big)}"); // 12, 8, 21

            numbers.Sort(delegate (int a, int b) { return b.CompareTo(a); });
            Console.WriteLine($"Sorted desc: {string.Join(", ", numbers)}");   // 21, 12, 8, 5, 3
        }

        public delegate int Counter();

        // CLOSURES (CAPTURED VARIABLES)
        // - The anonymous method uses 'count', a LOCAL variable of MakeCounter.
        // - The compiler moves 'count' into a hidden heap object so it outlives the method
        //   call and lives as long as the returned delegate does.
        // - Each call to MakeCounter creates a NEW captured 'count', so counters are independent.
        static Counter MakeCounter()
        {
            int count = 0;
            return delegate { count++; return count; };
        }

        public static void DemonstrateClosures()
        {
            Counter counterA = MakeCounter();
            Counter counterB = MakeCounter();

            Console.WriteLine(counterA()); // 1
            Console.WriteLine(counterA()); // 2
            Console.WriteLine(counterA()); // 3
            Console.WriteLine(counterB()); // 1
        }

        // BUILT-IN GENERIC DELEGATES (.NET 2.0)
        // - Predicate<T>          : T -> bool        (a yes/no test, used by Exists/FindAll)
        // - Comparison<T>         : (T, T) -> int    (ordering, used by Sort)
        // - Converter<TIn, TOut>  : TIn -> TOut      (projection, used by ConvertAll)
        // You rarely need to declare your own delegate types anymore. Func<> and Action<>
        // (.NET 3.5) later became the general-purpose standard.
        public static void DemonstrateGenericDelegates()
        {
            Predicate<string> isLong = delegate (string s) { return s.Length > 5; };
            Comparison<string> byLength = delegate (string a, string b) { return a.Length.CompareTo(b.Length); };
            Converter<string, int> toLength = delegate (string s) { return s.Length; };

            List<string> words = new List<string>(new string[] { "banana", "fig", "cherry", "kiwi" });
            Console.WriteLine($"Any long word: {words.Exists(isLong)}");     // True

            words.Sort(byLength);
            Console.WriteLine(string.Join(", ", words));                     // fig, kiwi, banana, cherry

            List<int> lengths = words.ConvertAll(toLength);
            Console.WriteLine(string.Join(", ", lengths));                   // 3, 4, 6, 6
        }
    }

    /*

    //1. Closures capture VARIABLES, not values. Classic loop bug (pre C# 5 foreach, still true for 'for'):
    List<Action> actions = new List<Action>();
    for (int i = 0; i < 3; i++)
        actions.Add(delegate { Console.WriteLine(i); });
    foreach (Action a in actions) a();   // prints 3 3 3, not 0 1 2
    // Fix: copy to a local inside the loop:  int copy = i;

    //2. Captured variables extend object lifetimes -> possible memory leaks.

    C# 3 lambdas ( x => x > 7 ) replaced anonymous methods almost everywhere - see 3_LambdaExpressions.cs.

     */
}
