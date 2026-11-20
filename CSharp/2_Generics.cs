// =============================================================================
//  Feature     : Generics
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    Generics let you write classes, interfaces, methods and delegates with TYPE
//    PARAMETERS (<T>) that the caller fills in. One implementation then works for
//    any type while staying fully type-safe, with no casts and no boxing of value
//    types. CONSTRAINTS (where T : ...) restrict which types are allowed and
//    unlock members on T. .NET 2.0 also introduced System.Collections.Generic
//    (List<T>, Dictionary<TKey,TValue>, Queue<T>, Stack<T>...).
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class Generics
    {
        public static void Run()
        {
            DemonstrateWhyGenerics();
            DemonstrateGenericClass();
            DemonstrateGenericMethods();
            DemonstrateConstraints();
            DemonstrateGenericCollections();
        }

        // THE PROBLEM GENERICS SOLVE
        // - ArrayList (C# 1) stores everything as 'object':
        //     * adding an int BOXES it (heap allocation)
        //     * reading requires a CAST
        //     * nothing stops you adding the wrong type; mistakes surface as runtime
        //       InvalidCastException instead of compile errors.
        // - List<int> is type-safe at compile time, stores ints directly, and needs no casts.
        public static void DemonstrateWhyGenerics()
        {
            ArrayList oldList = new ArrayList();
            oldList.Add(1);
            oldList.Add("two");
            int first = (int)oldList[0]!;
            // int second = (int)oldList[1];   // InvalidCastException at RUNTIME

            List<int> newList = new List<int>();
            newList.Add(1);
            // newList.Add("two");             // compile-time error
            int firstTyped = newList[0];

            Console.WriteLine($"ArrayList: {first}, List<int>: {firstTyped}");
        }

        // GENERIC CLASS
        // - Box<T> declares a type parameter T used for its field, constructor and property.
        // - Each use (Box<int>, Box<string>) creates a distinct, strongly-typed class.
        // - default(T) gives the "zero" value for T: null for reference types, 0/false for
        //   value types. EqualityComparer<T>.Default compares two T values correctly for any T.
        public class Box<T>
        {
            private T _value;

            public Box(T value) { _value = value; }

            public T Value { get { return _value; } }

            public bool HasDefaultValue()
            {
                return EqualityComparer<T>.Default.Equals(_value, default(T));
            }
        }

        // MULTIPLE TYPE PARAMETERS: separate them with commas. Naming convention is T or TName.
        public class Pair<TFirst, TSecond>
        {
            public TFirst First;
            public TSecond Second;

            public Pair(TFirst first, TSecond second)
            {
                First = first;
                Second = second;
            }
        }

        // USING GENERIC CLASSES: supply concrete type arguments in angle brackets.
        public static void DemonstrateGenericClass()
        {
            Box<int> intBox = new Box<int>(42);
            Box<string> stringBox = new Box<string>("hello");
            Box<int> emptyBox = new Box<int>(0);

            Console.WriteLine($"{intBox.Value}, {stringBox.Value}");                 // 42, hello
            Console.WriteLine($"emptyBox has default: {emptyBox.HasDefaultValue()}"); // True

            Pair<string, int> pair = new Pair<string, int>("Age", 30);
            Console.WriteLine($"{pair.First} = {pair.Second}");                       // Age = 30
        }

        // GENERIC METHOD: the type parameter belongs to the method, not the class.
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // CALLING GENERIC METHODS
        // - You can pass the type argument explicitly: Swap<int>(...).
        // - Usually the compiler INFERS T from the arguments: Swap(ref s1, ref s2) -> T = string.
        public static void DemonstrateGenericMethods()
        {
            int x = 1, y = 2;
            Swap<int>(ref x, ref y);
            Console.WriteLine($"x = {x}, y = {y}");   // x = 2, y = 1

            string s1 = "left", s2 = "right";
            Swap(ref s1, ref s2);
            Console.WriteLine($"s1 = {s1}, s2 = {s2}"); // s1 = right, s2 = left
        }

        // CONSTRAINTS
        // - Without constraints you can only use 'object' members on T.
        // - where T : IComparable<T> -> T must be comparable, so a.CompareTo(b) is allowed.
        // - where T : new()          -> T must have a public parameterless constructor, so new T() is allowed.
        // - where T : class          -> T must be a reference type.
        static T Max<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) >= 0 ? a : b;
        }

        static T Create<T>() where T : new()
        {
            return new T();
        }

        public class Repository<TEntity> where TEntity : class
        {
            private List<TEntity> _items = new List<TEntity>();
            public void Add(TEntity item) { _items.Add(item); }
            public int Count { get { return _items.Count; } }
        }

        public class Customer
        {
            public string Name = "New customer";
        }

        // USING CONSTRAINED GENERICS
        // - Max works for int and string because both implement IComparable<T>.
        // - Create<Customer>() works because Customer has a parameterless constructor.
        // - Repository<int> would NOT compile: int violates the 'class' constraint.
        public static void DemonstrateConstraints()
        {
            Console.WriteLine(Max(3, 7));             // 7
            Console.WriteLine(Max("apple", "pear"));  // pear

            Customer customer = Create<Customer>();
            Console.WriteLine(customer.Name);         // New customer

            Repository<Customer> repo = new Repository<Customer>();
            repo.Add(customer);
            Console.WriteLine($"Repo count: {repo.Count}"); // Repo count: 1
            // Repository<int> bad;                   // compile error: int is not a reference type
        }

        // GENERIC COLLECTIONS (System.Collections.Generic)
        // - Dictionary<TKey,TValue> : key/value lookup; TryGetValue avoids an exception for missing keys.
        // - Queue<T>                : first-in, first-out.
        // - Stack<T>                : last-in, first-out.
        // - HashSet<T> (.NET 3.5)   : unique values only, fast Contains.
        public static void DemonstrateGenericCollections()
        {
            Dictionary<string, int> stock = new Dictionary<string, int>();
            stock["apples"] = 10;
            stock["pears"] = 4;

            int count;
            if (stock.TryGetValue("apples", out count))
                Console.WriteLine($"apples: {count}");    // apples: 10

            Queue<string> queue = new Queue<string>();
            queue.Enqueue("first");
            queue.Enqueue("second");
            Console.WriteLine(queue.Dequeue());           // first

            Stack<string> stack = new Stack<string>();
            stack.Push("bottom");
            stack.Push("top");
            Console.WriteLine(stack.Pop());               // top

            HashSet<int> unique = new HashSet<int>(new int[] { 1, 2, 2, 3, 3, 3 });
            Console.WriteLine($"unique count: {unique.Count}"); // unique count: 3
        }
    }

    /*

    Constraint cheat sheet:
        where T : struct           value type
        where T : class            reference type
        where T : new()            parameterless constructor
        where T : BaseClass        derives from BaseClass
        where T : IInterface       implements IInterface
        where T : U                T derives from another type parameter U
    Later additions: notnull (C# 8), unmanaged / Enum / Delegate (C# 7.3),
    allows ref struct (C# 13).

    //1. Unlike Java, .NET generics are reified - List<int> is a real, separate type at runtime
    //   with no boxing. typeof(List<int>) != typeof(List<string>).

    //2. Static fields are per closed type: Box<int>.SomeStatic and Box<string>.SomeStatic are different.

     */
}
