# C# and .NET Features by Version (with Examples)

Each feature links to its demo file in [`CSharp/`](CSharp). Versions are listed from oldest to newest.

## .NET Framework 1.0 / C# 1.0 (2002)
**Features & Examples:**

- **Classes and structs (reference vs value types)** — [`1_ClassesAndStructs.cs`](CSharp/1_ClassesAndStructs.cs)
    ```csharp
    struct Point
    {
        public int X;
    }

    Point a = new Point();
    Point b = a;      // the whole value is copied
    b.X = 5;

    Console.WriteLine(a.X); // 0 - 'a' is unaffected
    ```

- **Boxing and unboxing** — [`1_ClassesAndStructs.cs`](CSharp/1_ClassesAndStructs.cs)
    ```csharp
    int number = 42;
    object boxed = number;      // boxing: value copied to the heap
    int unboxed = (int)boxed;   // unboxing: explicit cast copies it back

    number = 7;
    Console.WriteLine(boxed);   // 42 - the box is an independent copy
    ```

- **Inheritance, polymorphism and interfaces** — [`1_InheritanceAndInterfaces.cs`](CSharp/1_InheritanceAndInterfaces.cs)
    ```csharp
    abstract class Shape
    {
        public abstract double Area();
    }

    class Square : Shape
    {
        public override double Area()
        {
            return 4;
        }
    }

    Shape s = new Square();
    Console.WriteLine(s.Area()); // 4
    ```

- **Interfaces, `sealed`, `is` / `as`** — [`1_InheritanceAndInterfaces.cs`](CSharp/1_InheritanceAndInterfaces.cs)
    ```csharp
    interface IPrintable
    {
        void Print();
    }

    // one base class, many interfaces; 'sealed' blocks further inheritance
    sealed class Report : IPrintable, IComparable
    {
        public int Pages;

        public void Print()
        {
            Console.WriteLine("Printing...");
        }

        // explicit implementation - only visible through IComparable
        int IComparable.CompareTo(object obj)
        {
            return Pages.CompareTo(((Report)obj).Pages);
        }
    }

    object o = new Report();
    if (o is IPrintable)
    {
        Console.WriteLine("Report is printable");
    }

    Report r = o as Report; // null instead of an exception if the cast fails
    ```

- **Properties and indexers** — [`1_PropertiesAndIndexers.cs`](CSharp/1_PropertiesAndIndexers.cs)
    ```csharp
    private int _age;
    private string[] _items = { "a", "b" };

    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }

    public string this[int i]
    {
        get { return _items[i]; }
    }
    ```

- **Delegates (including multicast)** — [`1_DelegatesAndEvents.cs`](CSharp/1_DelegatesAndEvents.cs)
    ```csharp
    delegate int MathOp(int a, int b);
    delegate void Notify(string message);

    static int Add(int a, int b)
    {
        return a + b;
    }

    MathOp op = new MathOp(Add);
    Console.WriteLine(op(2, 3)); // 5

    // Multicast: one delegate, several methods (+= adds, -= removes)
    Notify notify = new Notify(LogToConsole);
    notify += new Notify(LogToAudit);
    notify("User logged in"); // both methods run, in order
    ```

- **Events** — [`1_DelegatesAndEvents.cs`](CSharp/1_DelegatesAndEvents.cs)
    ```csharp
    // Publisher
    class Thermostat
    {
        public event EventHandler TemperatureChanged; // outsiders can only += / -=

        private int _temperature;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnTemperatureChanged(EventArgs.Empty);
            }
        }

        // only the declaring class can raise the event
        protected virtual void OnTemperatureChanged(EventArgs e)
        {
            EventHandler handler = TemperatureChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    // Subscriber
    Thermostat thermostat = new Thermostat();
    thermostat.TemperatureChanged += new EventHandler(OnTempChanged);
    thermostat.Temperature = 35;          // raises the event

    // thermostat.TemperatureChanged(...); // compile error: can't raise from outside
    // thermostat.TemperatureChanged = null; // compile error: only += / -=
    ```

- **Enums (and [Flags])** — [`1_Enums.cs`](CSharp/1_Enums.cs)
    ```csharp
    enum Status
    {
        Pending,
        Shipped,
        Delivered
    }

    Status s = Status.Shipped;
    Console.WriteLine((int)s); // 1

    // [Flags]: each value is a bit, so values can be combined
    [Flags]
    enum Permissions
    {
        None    = 0,
        Read    = 1,
        Write   = 2,
        Execute = 4
    }

    Permissions p = Permissions.Read | Permissions.Write;
    Console.WriteLine(p);                                  // Read, Write
    Console.WriteLine((p & Permissions.Write) != 0);       // True
    ```

- **Exception handling (try / catch / finally)** — [`1_ExceptionHandling.cs`](CSharp/1_ExceptionHandling.cs)
    ```csharp
    try
    {
        int.Parse("abc");
    }
    catch (FormatException)
    {
        Console.WriteLine("Not a number");
    }
    finally
    {
        Console.WriteLine("Always runs");
    }

    // Custom exception with extra context
    class InsufficientFundsException : Exception
    {
        public decimal Requested;

        public InsufficientFundsException(decimal requested)
            : base("Requested " + requested + " but funds are too low")
        {
            Requested = requested;
        }
    }

    try
    {
        throw new InsufficientFundsException(250m);
    }
    catch (InsufficientFundsException ex)
    {
        Console.WriteLine(ex.Message);
        throw; // rethrow, keeping the original stack trace
    }
    ```

- **using statement and IDisposable** — [`1_UsingStatement.cs`](CSharp/1_UsingStatement.cs)
    ```csharp
    using (StreamReader reader = new StreamReader("file.txt"))
    {
        Console.WriteLine(reader.ReadLine());
    } // reader.Dispose() called automatically
    ```

- **ref, out and params parameters** — [`1_RefOutParams.cs`](CSharp/1_RefOutParams.cs)
    ```csharp
    void Double(ref int n)
    {
        n *= 2;
    }

    int Sum(params int[] nums)
    {
        int total = 0;
        foreach (int n in nums)
            total += n;
        return total;
    }

    int x = 21;
    Double(ref x);
    Console.WriteLine(x);          // 42
    Console.WriteLine(Sum(1, 2, 3)); // 6
    ```

- **Operator overloading** — [`1_OperatorOverloading.cs`](CSharp/1_OperatorOverloading.cs)
    ```csharp
    public static Money operator +(Money a, Money b)
    {
        return new Money(a.Amount + b.Amount);
    }

    // == and != must be overloaded together
    public static bool operator ==(Money a, Money b)
    {
        return a.Amount == b.Amount;
    }

    public static bool operator !=(Money a, Money b)
    {
        return !(a == b);
    }

    // User-defined conversions
    public static implicit operator Money(decimal amount)  // no cast needed
    {
        return new Money(amount);
    }

    public static explicit operator decimal(Money money)   // cast required
    {
        return money.Amount;
    }

    Money total = new Money(10m) + new Money(5m);
    Money fromDecimal = 5m;
    decimal raw = (decimal)fromDecimal;
    ```

- **Attributes** — [`1_Attributes.cs`](CSharp/1_Attributes.cs)
    ```csharp
    // Built-in attribute understood by the compiler
    [Obsolete("Use NewMethod instead")]
    void OldMethod()
    {
    }

    // Custom attribute
    [AttributeUsage(AttributeTargets.Class)]
    class AuthorAttribute : Attribute
    {
        public string Name;

        public AuthorAttribute(string name)
        {
            Name = name;
        }
    }

    [Author("Alice")]
    class InvoiceService
    {
    }

    // Attributes are metadata - read them with reflection
    AuthorAttribute author = (AuthorAttribute)Attribute.GetCustomAttribute(
        typeof(InvoiceService), typeof(AuthorAttribute));

    Console.WriteLine(author.Name); // Alice
    ```

---

## .NET Framework 2.0 / C# 2.0 (2005)
**Features & Examples:**

- **Generics** — [`2_Generics.cs`](CSharp/2_Generics.cs)
    ```csharp
    T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? a : b;
    }

    List<int> numbers = new List<int>(); // type-safe, no boxing
    Console.WriteLine(Max(3, 7));        // 7

    // Generic class
    class Box<T>
    {
        public T Value;

        public Box(T value)
        {
            Value = value;
        }
    }

    Box<string> box = new Box<string>("hello");

    // Generic collections (System.Collections.Generic)
    Dictionary<string, int> stock = new Dictionary<string, int>();
    stock["apples"] = 10;

    int count;
    if (stock.TryGetValue("apples", out count))
    {
        Console.WriteLine(count); // 10
    }

    Queue<string> queue = new Queue<string>();  // first in, first out
    Stack<string> stack = new Stack<string>();  // last in, first out
    ```

- **Nullable value types (`T?`) and `??`** — [`2_NullableValueTypes.cs`](CSharp/2_NullableValueTypes.cs)
    ```csharp
    int? age = null;
    Console.WriteLine(age ?? 18);                 // 18
    Console.WriteLine(age.HasValue);              // False
    Console.WriteLine(age.GetValueOrDefault(-1)); // -1

    // Lifted operators: null in -> null out
    int? x = 5;
    int? sum = x + age;
    Console.WriteLine(sum == null); // True
    ```

- **Iterators (`yield return`)** — [`2_Iterators.cs`](CSharp/2_Iterators.cs)
    ```csharp
    IEnumerable<int> Evens()
    {
        for (int i = 0; i <= 6; i += 2)
        {
            yield return i;
        }
    }

    foreach (int n in Evens())
    {
        Console.Write(n); // 0246
    }

    // yield break ends the sequence early
    IEnumerable<string> ReadUntilEmpty(string[] lines)
    {
        foreach (string line in lines)
        {
            if (line.Length == 0)
            {
                yield break;
            }

            yield return line;
        }
    }
    ```

- **Anonymous methods and closures** — [`2_AnonymousMethods.cs`](CSharp/2_AnonymousMethods.cs)
    ```csharp
    Predicate<int> isBig = delegate (int n)
    {
        return n > 7;
    };

    Console.WriteLine(isBig(10)); // True

    // Closure: the anonymous method captures the local variable 'count'
    delegate int Counter();

    Counter MakeCounter()
    {
        int count = 0;
        return delegate { count++; return count; };
    }

    Counter next = MakeCounter();
    next();
    Console.WriteLine(next()); // 2
    ```

- **Method group conversion and built-in generic delegates** — [`2_AnonymousMethods.cs`](CSharp/2_AnonymousMethods.cs)
    ```csharp
    Greeter g1 = new Greeter(SayHello); // C# 1
    Greeter g2 = SayHello;              // C# 2 - the compiler creates the delegate

    // .NET 2.0 generic delegates: Predicate<T>, Comparison<T>, Converter<TIn, TOut>
    List<string> words = new List<string> { "banana", "fig", "kiwi" };

    words.Sort(delegate (string a, string b)
    {
        return a.Length.CompareTo(b.Length);
    });

    List<int> lengths = words.ConvertAll(delegate (string s)
    {
        return s.Length;
    });
    ```

- **Partial types** — [`2_PartialTypes.cs`](CSharp/2_PartialTypes.cs)
    ```csharp
    // Order.Generated.cs
    partial class Order
    {
        public int Id;
    }

    // Order.cs
    partial class Order
    {
        public void Save()
        {
        }
    }
    ```

- **Static classes** — [`2_StaticClasses.cs`](CSharp/2_StaticClasses.cs)
    ```csharp
    static class Converter
    {
        public static double ToF(double c)
        {
            return c * 9 / 5 + 32;
        }
    }

    Console.WriteLine(Converter.ToF(100)); // 212

    // Static constructor: runs once, automatically, before first use
    static class AppConfig
    {
        public static readonly string Environment;

        static AppConfig()
        {
            Environment = "Development";
        }
    }
    ```

- **Property accessor accessibility** — [`2_PropertyAccessorAccessibility.cs`](CSharp/2_PropertyAccessorAccessibility.cs)
    ```csharp
    private decimal _total;

    public decimal Total
    {
        get { return _total; }
        private set { _total = value; } // only this class can set it
    }
    ```

---

## .NET Framework 3.5 / C# 3.0 (2007)
**Features & Examples:**

- **Implicitly typed locals (`var`)** — [`3_ImplicitlyTypedLocals.cs`](CSharp/3_ImplicitlyTypedLocals.cs)
    ```csharp
    var scores = new Dictionary<string, int>(); // type inferred at compile time
    var name = "Alice";                         // string
    ```

- **Auto-implemented properties** — [`3_AutoImplementedProperties.cs`](CSharp/3_AutoImplementedProperties.cs)
    ```csharp
    class Book
    {
        public string Title { get; set; }
        public bool IsPublished { get; private set; }
    }
    ```

- **Object and collection initializers** — [`3_ObjectAndCollectionInitializers.cs`](CSharp/3_ObjectAndCollectionInitializers.cs)
    ```csharp
    var customer = new Customer
    {
        Name = "Bob",
        Age = 25
    };

    var numbers = new List<int> { 1, 2, 3 };
    ```

- **Anonymous types** — [`3_AnonymousTypes.cs`](CSharp/3_AnonymousTypes.cs)
    ```csharp
    var person = new
    {
        Name = "Alice",
        Age = 30
    };

    Console.WriteLine(person); // { Name = Alice, Age = 30 }
    ```

- **Extension methods** — [`3_ExtensionMethods.cs`](CSharp/3_ExtensionMethods.cs)
    ```csharp
    static class IntExtensions
    {
        public static bool IsEven(this int n)
        {
            return n % 2 == 0;
        }
    }

    Console.WriteLine(4.IsEven()); // True
    ```

- **Lambda expressions (`Func<>`, `Action<>`)** — [`3_LambdaExpressions.cs`](CSharp/3_LambdaExpressions.cs)
    ```csharp
    Func<int, int> square = x => x * x;
    Action<string> print = text => Console.WriteLine(text);

    print(square(5).ToString()); // 25
    ```

- **LINQ** — [`3_Linq.cs`](CSharp/3_Linq.cs)
    ```csharp
    // Query syntax
    var names =
        from s in staff
        where s.Salary > 60000
        select s.Name;

    // Method syntax (same result)
    var same = staff
        .Where(s => s.Salary > 60000)
        .Select(s => s.Name);

    // Common operators
    var total   = staff.Sum(s => s.Salary);
    var anyRich = staff.Any(s => s.Salary > 90000);
    var top2    = staff.OrderByDescending(s => s.Salary).Take(2);

    // Grouping
    var byDepartment =
        from s in staff
        group s by s.Department into g
        select new { Department = g.Key, Count = g.Count() };

    // Join
    var located =
        from s in staff
        join o in offices on s.OfficeId equals o.Id
        select new { s.Name, o.City };

    // Deferred execution: the query runs when enumerated, ToList() runs it now
    var query = staff.Where(s => s.Salary > 60000); // nothing executed yet
    var snapshot = query.ToList();                  // executed here
    ```

- **Expression trees** — [`3_ExpressionTrees.cs`](CSharp/3_ExpressionTrees.cs)
    ```csharp
    Expression<Func<int, bool>> expr = x => x > 5;

    Console.WriteLine(expr);          // x => (x > 5)

    Func<int, bool> compiled = expr.Compile();
    Console.WriteLine(compiled(10));  // True
    ```

- **Partial methods** — [`3_PartialMethods.cs`](CSharp/3_PartialMethods.cs)
    ```csharp
    partial class Product
    {
        // Declaration - the call is removed if it's never implemented
        partial void OnNameChanged(string newName);
    }

    partial class Product
    {
        partial void OnNameChanged(string newName)
        {
            Console.WriteLine($"Name changed to {newName}");
        }
    }
    ```

---

## .NET Framework 4.0 / C# 4.0 (2010)
**Features & Examples:**

- **Dynamic typing** — [`4_DynamicTyping.cs`](CSharp/4_DynamicTyping.cs)
    ```csharp
    dynamic val = "Hello";
    Console.WriteLine(val.ToUpper()); // resolved at runtime
    ```

- **Named and optional parameters** — [`4_NamedAndOptionalParams.cs`](CSharp/4_NamedAndOptionalParams.cs)
    ```csharp
    void Greet(string name = "World")
    {
        Console.WriteLine($"Hello {name}");
    }

    Greet();              // Hello World
    Greet(name: "Alice"); // Hello Alice
    ```

- **Generic covariance and contravariance (`out` / `in`)** — [`4_GenericVariance.cs`](CSharp/4_GenericVariance.cs)
    ```csharp
    // Covariance: IEnumerable<out T>
    IEnumerable<Animal> animals = new List<Dog>();

    // Contravariance: Action<in T>
    Action<Animal> feedAnimal = a => Console.WriteLine($"Feeding {a.Name}");
    Action<Dog> feedDog = feedAnimal;

    // Your own variant interfaces
    interface IProducer<out T>
    {
        T Produce();         // T only in output positions
    }

    interface IConsumer<in T>
    {
        void Consume(T item); // T only in input positions
    }
    ```

---

## .NET Framework 4.5 / C# 5.0 (2012)
**Features & Examples:**

- **async/await** — [`5_AsyncAwait.cs`](CSharp/5_AsyncAwait.cs)
    ```csharp
    async Task SayHiAsync()
    {
        await Task.Delay(100);
        Console.WriteLine("Hi");
    }

    await SayHiAsync();
    ```

- **Caller info attributes** — [`5_CallerInfo.cs`](CSharp/5_CallerInfo.cs)
    ```csharp
    void Log([CallerMemberName] string memberName = "")
    {
        Console.WriteLine(memberName);
    }

    Log(); // prints the calling method's name
    ```

---

## .NET Framework 4.6 / C# 6.0 (2015)
**Features & Examples:**

- **String interpolation** — [`6_StringInterpolation.cs`](CSharp/6_StringInterpolation.cs)
    ```csharp
    var name = "Bob";
    var price = 9.5m;

    Console.WriteLine($"Hello {name}, you owe {price:C}");
    ```

- **Null-conditional operator (`?.`)** — [`6_NullConditionalOperator.cs`](CSharp/6_NullConditionalOperator.cs)
    ```csharp
    string s = null;
    Console.WriteLine(s?.Length); // no exception, prints empty

    string city = order?.Customer?.City ?? "N/A"; // chain + fallback
    var first = list?[0];                         // null-conditional indexer
    PropertyChanged?.Invoke(this, args);          // safe event raising
    ```

- **Expression-bodied members** — [`6_ExpressionBodiedMembers.cs`](CSharp/6_ExpressionBodiedMembers.cs)
    ```csharp
    int Add(int a, int b) => a + b;

    Console.WriteLine(Add(2, 3)); // 5
    ```

- **Auto-property initializers** — [`6_AutoPropertyInitializers.cs`](CSharp/6_AutoPropertyInitializers.cs)
    ```csharp
    class P
    {
        public int X { get; set; } = 4;
        public Guid Id { get; } = Guid.NewGuid(); // get-only auto-property
    }

    Console.WriteLine(new P().X); // 4
    ```

- **Using static directives** — [`6_StaticDirectives.cs`](CSharp/6_StaticDirectives.cs)
    ```csharp
    using static System.Math;

    Console.WriteLine(Sqrt(16)); // 4
    ```

- **Exception filters** — [`6_ExceptionFilters.cs`](CSharp/6_ExceptionFilters.cs)
    ```csharp
    try
    {
        throw new Exception("Oof");
    }
    catch (Exception e) when (e.Message.Contains("Oof"))
    {
        Console.WriteLine("Filtered!");
    }
    ```

- **nameof operator** — [`6_Nameof.cs`](CSharp/6_Nameof.cs)
    ```csharp
    void Send(string to)
    {
        if (to == null)
        {
            throw new ArgumentNullException(nameof(to)); // "to"
        }
    }
    ```

---

## .NET Framework 4.7 / .NET Core 2.x / C# 7.0–7.3 (2017–2018)
**Features & Examples:**

- **Tuples & deconstruction** (7.0) — [`7_TuplesAndDeconstruction.cs`](CSharp/7_TuplesAndDeconstruction.cs)
    ```csharp
    (int Sum, int Product) Calc(int a, int b)
    {
        return (a + b, a * b);
    }

    var (sum, product) = Calc(2, 3);
    Console.WriteLine(sum + product); // 11

    // Named tuple elements and tuples in collections
    var point = (X: 10, Y: 20);
    Console.WriteLine(point.X);

    var people = new List<(string Name, int Age)>
    {
        ("Alice", 25),
        ("Bob", 30)
    };
    ```

- **Pattern matching (`is`, `switch`)** (7.0) — [`7_PatternMatching.cs`](CSharp/7_PatternMatching.cs)
    ```csharp
    object o = 5;

    if (o is int x)
    {
        Console.WriteLine(x + 1); // 6
    }

    // Patterns with 'when' guards in a switch statement
    switch (shape)
    {
        case Circle c when c.Radius > 10:
            Console.WriteLine("Big circle");
            break;
        case Circle c:
            Console.WriteLine("Circle");
            break;
        case null:
            Console.WriteLine("No shape");
            break;
    }
    ```

- **Local functions** (7.0) — [`7_LocalFunctions.cs`](CSharp/7_LocalFunctions.cs)
    ```csharp
    void Run()
    {
        // declared inside a method, visible only here
        int Add(int a, int b) => a + b;

        Console.WriteLine(Add(2, 3)); // 5
    }
    ```

- **`out` variables anywhere** (7.0) — [`7_OutVariablesAnywhere.cs`](CSharp/7_OutVariablesAnywhere.cs)
    ```csharp
    if (int.TryParse("5", out int result))
    {
        Console.WriteLine(result); // 5
    }
    ```

- **Discards (`_`)** (7.0) — [`7_Discards.cs`](CSharp/7_Discards.cs)
    ```csharp
    // Ignore an out value
    if (int.TryParse("4", out _))
    {
        Console.WriteLine("Parsed!");
    }

    // Ignore a tuple element
    (_, int y) = (7, 8);

    // Ignore a return value
    _ = DoSomething();
    ```

- **Ref returns and locals** (7.0) — [`7_RefReturnsAndLocals.cs`](CSharp/7_RefReturnsAndLocals.cs)
    ```csharp
    ref int First(int[] arr)
    {
        return ref arr[0];
    }

    int[] nums = { 1, 2 };
    ref int r = ref First(nums);
    r = 99;

    Console.WriteLine(nums[0]); // 99
    ```

- **Digit separators and binary literals** (7.0) — [`7_DigitSeparators.cs`](CSharp/7_DigitSeparators.cs)
    ```csharp
    int billion = 1_000_000_000;
    byte mask = 0b_0000_0101;
    ```

- **throw expressions** (7.0) — [`7_ThrowExpressions.cs`](CSharp/7_ThrowExpressions.cs)
    ```csharp
    public EmailService(string? name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    // In a conditional branch
    int Percent(int value)
    {
        return value is >= 0 and <= 100
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value));
    }

    // In an expression-bodied member
    public void Delete(int id) => throw new NotSupportedException("Read-only");
    ```

- **Generalized async return types (`ValueTask<T>`)** (7.0) — [`7_ValueTask.cs`](CSharp/7_ValueTask.cs)
    ```csharp
    ValueTask<decimal> GetPriceAsync(string sku)
    {
        if (_cache.TryGetValue(sku, out var price))
        {
            return new ValueTask<decimal>(price); // no allocation on a cache hit
        }

        return new ValueTask<decimal>(LoadFromDatabaseAsync(sku));
    }
    ```

- **default literal expressions** (7.1) — [`7_DefaultLiteralExpressions.cs`](CSharp/7_DefaultLiteralExpressions.cs)
    ```csharp
    string s = default; // instead of default(string)
    int n = default;    // 0

    Console.WriteLine(s ?? "null");
    ```

- **readonly struct and `in` parameters** (7.2) — [`7_ReadOnlyStructsAndInParams.cs`](CSharp/7_ReadOnlyStructsAndInParams.cs)
    ```csharp
    readonly struct Vector3
    {
        public double X { get; }

        public Vector3(double x)
        {
            X = x;
        }
    }

    // passed by read-only reference - no copy, no modification
    double Length(in Vector3 v)
    {
        return Math.Abs(v.X);
    }
    ```

- **Span<T> and ref structs** (7.2) — [`7_SpanAndRefStructs.cs`](CSharp/7_SpanAndRefStructs.cs)
    ```csharp
    int[] numbers = { 0, 1, 2, 3, 4 };

    Span<int> middle = numbers.AsSpan(1, 3); // a view, no copy
    middle[0] = 100;

    Console.WriteLine(numbers[1]); // 100

    // Parsing without allocating substrings
    ReadOnlySpan<char> line = "2024-11-19,ORDER-1042".AsSpan();
    ReadOnlySpan<char> datePart = line.Slice(0, line.IndexOf(','));
    DateTime date = DateTime.Parse(datePart);

    // stackalloc: a temporary buffer on the stack (safe with Span)
    Span<int> squares = stackalloc int[5];

    // ref struct: can only live on the stack (like Span<T>)
    ref struct WordReader
    {
        private ReadOnlySpan<char> _remaining;
    }
    ```

---

## .NET Core 3.0 / C# 8.0 (2019)
**Features & Examples:**

- **Nullable reference types** — [`8_NullableReferenceTypes.cs`](CSharp/8_NullableReferenceTypes.cs)
    ```csharp
    string? nickname = null;           // may be null
    Console.WriteLine(nickname.Length); // compiler warning CS8602

    if (nickname != null)
    {
        Console.WriteLine(nickname.Length); // no warning
    }
    ```

- **Async streams (`await foreach`)** — [`8_AsyncStreams.cs`](CSharp/8_AsyncStreams.cs)
    ```csharp
    async IAsyncEnumerable<int> GetNumbersAsync()
    {
        await Task.Delay(10);
        yield return 5;
    }

    await foreach (var i in GetNumbersAsync())
    {
        Console.WriteLine(i);
    }
    ```

- **Switch expressions** — [`8_SwitchExpressions.cs`](CSharp/8_SwitchExpressions.cs)
    ```csharp
    int x = 3;

    var result = x switch
    {
        1 => "One",
        3 => "Three",
        _ => "Other"
    };

    Console.WriteLine(result); // Three

    // Property pattern
    decimal Discount(Order o) => o switch
    {
        { Total: > 1000 } => 0.1m,
        _                 => 0m
    };

    // Tuple pattern
    string Play(string a, string b) => (a, b) switch
    {
        ("rock", "scissors") => "rock wins",
        ("paper", "rock")    => "paper wins",
        _                    => "other"
    };
    ```

- **Indices and ranges (`^`, `..`)** — [`8_InidicesAndRanges.cs`](CSharp/8_InidicesAndRanges.cs)
    ```csharp
    var arr = new[] { 1, 2, 3, 4 };

    Console.WriteLine(arr[^1]);                      // 4
    Console.WriteLine(string.Join(",", arr[1..3]));  // 2,3
    ```

- **Null-coalescing assignment (`??=`)** — [`8_NullCoalescingAssignment.cs`](CSharp/8_NullCoalescingAssignment.cs)
    ```csharp
    List<int>? list = null;

    list ??= new List<int>(); // assigned only if null
    ```

- **using declarations and `await using`** — [`8_UsingDeclarations.cs`](CSharp/8_UsingDeclarations.cs)
    ```csharp
    void ReadFile()
    {
        using var reader = new StreamReader("file.txt");
        Console.WriteLine(reader.ReadLine());
    } // reader disposed here, at the end of the scope

    // await using: for IAsyncDisposable (calls and awaits DisposeAsync)
    class AsyncResource : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await Task.Delay(10);
        }
    }

    await using var resource = new AsyncResource();
    ```

- **Readonly members** — [`8_ReadOnlyMembers.cs`](CSharp/8_ReadOnlyMembers.cs)
    ```csharp
    struct Point
    {
        public double X { get; set; }

        // promises not to modify the struct
        public readonly double Doubled()
        {
            return X * 2;
        }
    }
    ```

- **Default interface methods** — [`8_DefaultInterfaceMethods.cs`](CSharp/8_DefaultInterfaceMethods.cs)
    ```csharp
    interface ILogger
    {
        void Log(string message)
        {
            Console.WriteLine(message); // default implementation
        }
    }

    class ConsoleLogger : ILogger
    {
    }

    ILogger logger = new ConsoleLogger();
    logger.Log("Default");
    ```

---

## .NET 5 / C# 9.0 (2020)
**Features & Examples:**

- **Records** — [`9_Records.cs`](CSharp/9_Records.cs)
    ```csharp
    record Point(int X, int Y);

    var a = new Point(1, 2);
    var b = new Point(1, 2);

    Console.WriteLine(a == b); // True - value equality
    Console.WriteLine(a);      // Point { X = 1, Y = 2 }

    var (x, y) = a;            // generated Deconstruct
    ```

- **Init-only setters** — [`9_InitOnlySetters.cs`](CSharp/9_InitOnlySetters.cs)
    ```csharp
    class P
    {
        public int X { get; init; }
    }

    var p = new P { X = 3 };
    // p.X = 4; // compile error
    ```

- **with-expressions** — [`9_WithExpressions.cs`](CSharp/9_WithExpressions.cs)
    ```csharp
    var p1 = new Point(1, 2);
    var p2 = p1 with { Y = 5 };

    Console.WriteLine(p2); // Point { X = 1, Y = 5 }
    ```

- **Top-level statements** — [`9_TopLevelStatments.cs`](CSharp/9_TopLevelStatments.cs)
    ```csharp
    // Entire Program.cs - no namespace, class or Main
    using System;

    Console.WriteLine("Hello World!");
    ```

- **Pattern matching enhancements (relational, `and` / `or` / `not`)** — [`9_PatternMatchingEnhancements.cs`](CSharp/9_PatternMatchingEnhancements.cs)
    ```csharp
    int score = 85;

    if (score is >= 80 and < 90)
    {
        Console.WriteLine("Good job!");
    }

    if (name is not null)
    {
        Console.WriteLine(name);
    }
    ```

- **Target-typed `new()`** — [`9_TargetTypedNew.cs`](CSharp/9_TargetTypedNew.cs)
    ```csharp
    private Dictionary<string, List<int>> _map = new();

    Point p = new(3, 4);
    ```

---

## .NET 6 / C# 10.0 (2021)
**Features & Examples:**

- **Global using directives** — [`10_GlobalUsings.cs`](CSharp/10_GlobalUsings.cs)
    ```csharp
    // GlobalUsings.cs
    global using System.Text; // now available in every file
    ```

- **File-scoped namespaces** — [`10_FileScopedNamespaces.cs`](CSharp/10_FileScopedNamespaces.cs)
    ```csharp
    namespace Demo;

    class C
    {
    }
    ```

- **Record structs** — [`10_RecordStructs.cs`](CSharp/10_RecordStructs.cs)
    ```csharp
    public readonly record struct Point(int X, int Y);

    var p = new Point(10, 20);
    Console.WriteLine(p); // Point { X = 10, Y = 20 }
    ```

- **Lambda improvements (natural type)** — [`10_LambdaImprovements.cs`](CSharp/10_LambdaImprovements.cs)
    ```csharp
    var addOne = (int x) => x + 1; // inferred as Func<int, int>

    Console.WriteLine(addOne(3)); // 4
    ```

- **Extended property patterns** — [`10_ExtendedPropertyPatterns.cs`](CSharp/10_ExtendedPropertyPatterns.cs)
    ```csharp
    if (order is { Customer.Address.City: "Paris" })
    {
        Console.WriteLine("Paris order");
    }
    ```

---

## .NET 7 / C# 11.0 (2022)
**Features & Examples:**

- **Raw string literals** — [`11_RawStringLiterals.cs`](CSharp/11_RawStringLiterals.cs)
    ```csharp
    var json = """
        {
            "name": "Alice",
            "path": "C:\temp"
        }
        """;

    Console.WriteLine(json);
    ```

- **List patterns** — [`11_ListPatterns.cs`](CSharp/11_ListPatterns.cs)
    ```csharp
    var data = new[] { 1, 2, 3 };

    if (data is [1, .., 3])
    {
        Console.WriteLine("Pattern match!");
    }
    ```

- **Generic attributes** — [`11_GenericAttributes.cs`](CSharp/11_GenericAttributes.cs)
    ```csharp
    class TypedAttribute<T> : Attribute
    {
    }

    [Typed<int>]
    class C
    {
    }
    ```

- **Required members** — [`11_RequiredMembers.cs`](CSharp/11_RequiredMembers.cs)
    ```csharp
    class User
    {
        public required string Name { get; set; }
    }

    // var u = new User(); // Error CS9035: Name must be set
    var u = new User { Name = "Ann" };
    ```

- **Generic math (static abstract interface members)** — [`11_GenericMath.cs`](CSharp/11_GenericMath.cs)
    ```csharp
    T Sum<T>(IEnumerable<T> values) where T : INumber<T>
    {
        T total = T.Zero;

        foreach (var v in values)
        {
            total += v;
        }

        return total;
    }

    Console.WriteLine(Sum(new[] { 1.5, 2.5 })); // 4

    // Your own static abstract members
    interface IShape
    {
        static abstract string ShapeName { get; }
        static abstract IShape Create(double size);
    }

    void Build<T>(double size) where T : IShape
    {
        IShape shape = T.Create(size);   // static member called through T
        Console.WriteLine(T.ShapeName);
    }
    ```

---

## .NET 8 / C# 12.0 (2023)
**Features & Examples:**

- **Collection expressions** — [`12_CollectionExpressions.cs`](CSharp/12_CollectionExpressions.cs)
    ```csharp
    int[] first = [1, 2, 3];
    List<int> all = [0, .. first, 4]; // spread operator

    Console.WriteLine(all.Count); // 5
    ```

- **Primary constructors for classes** — [`12_PrimaryConstructorsForClasses.cs`](CSharp/12_PrimaryConstructorsForClasses.cs)
    ```csharp
    class GreetingService(ILog log)
    {
        public void Greet(string name)
        {
            log.Write($"Hello {name}");
        }
    }

    // Initializing members, chaining constructors, and inheritance
    class BankAccount(string owner, decimal balance)
    {
        public string Owner { get; } = owner;
        public decimal Balance { get; private set; } = balance;

        public BankAccount() : this("Unknown", 0m) // must chain to the primary
        {
        }
    }

    class ProductRepository(ILog log, string category) : Repository<string>(log)
    {
    }
    ```

- **Primary constructors for structs** — [`12_PrimaryConstructorsForStructs.cs`](CSharp/12_PrimaryConstructorsForStructs.cs)
    ```csharp
    struct Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }
    ```

- **Default lambda parameters** — [`12_DefaultLambdaParameters.cs`](CSharp/12_DefaultLambdaParameters.cs)
    ```csharp
    var add = (int x = 1, int y = 2) => x + y;

    Console.WriteLine(add());     // 3
    Console.WriteLine(add(5));    // 7
    ```

- **Inline arrays** — [`12_InlineArrays.cs`](CSharp/12_InlineArrays.cs)
    ```csharp
    [System.Runtime.CompilerServices.InlineArray(3)]
    struct Buffer
    {
        private int _element0; // exactly one field, repeated 3 times
    }

    var buf = new Buffer();
    buf[0] = 10;
    ```

---

## .NET 9 / C# 13.0 (2024)
**Features & Examples:**

- **params collections** — [`13_ParamsCollectioncs.cs`](CSharp/13_ParamsCollectioncs.cs)
    ```csharp
    void Print(params ReadOnlySpan<string> items) // no array allocation
    {
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    Print("one", "two", "three");
    ```

- **New `Lock` type** — [`13_LockType.cs`](CSharp/13_LockType.cs)
    ```csharp
    private readonly Lock _sync = new();

    void Update()
    {
        lock (_sync)
        {
            // critical section - uses Lock.EnterScope()
        }
    }
    ```

- **Pattern matching in practice** (no new syntax in C# 13; combines C# 8–9 patterns) — [`13_PatternMatchingEnhancements.cs`](CSharp/13_PatternMatchingEnhancements.cs)
    ```csharp
    string Classify(int age) => age switch
    {
        < 0   => "Invalid",
        <= 12 => "Child",
        <= 17 => "Teenager",
        _     => "Adult"
    };
    ```

---

## .NET 10 / C# 14.0 (2025)
**Features & Examples:**

- **`field` keyword (field-backed properties)** (preview in C# 13) — [`13_BackingFields.cs`](CSharp/13_BackingFields.cs)
    ```csharp
    public string Name
    {
        get => field;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
    ```

- **Extension members (extension blocks)** — [`14_ExtensionMembers.cs`](CSharp/14_ExtensionMembers.cs)
    ```csharp
    static class TextExtensions
    {
        extension(string text)
        {
            // extension PROPERTY
            public bool IsBlank => string.IsNullOrWhiteSpace(text);

            // extension METHOD - no 'this' needed
            public string Truncate(int max) =>
                text.Length <= max ? text : text[..max] + "...";
        }

        // no receiver name = STATIC extensions, called on the type
        extension(string)
        {
            public static string Random(int length) => Guid.NewGuid().ToString("N")[..length];
        }
    }

    Console.WriteLine("   ".IsBlank);              // True
    Console.WriteLine("hello world".Truncate(5));  // hello...
    Console.WriteLine(string.Random(8));           // static extension
    ```

- **Null-conditional assignment (`?.` on the left side)** — [`14_NullConditionalAssignment.cs`](CSharp/14_NullConditionalAssignment.cs)
    ```csharp
    // assigns only if customer isn't null
    customer?.Name = "Alice";

    // also works with compound assignment and events
    customer?.LoginCount += 1;
    notifier?.Changed += OnChanged;

    // and with indexers
    scores?["Bob"] = 7;
    ```

---

**References:**
- [C# language history](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history)  
- [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)  
- [.NET docs](https://learn.microsoft.com/en-us/dotnet/core/)
