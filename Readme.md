# C# and .NET Features by Version (with Examples)

Each feature links to its demo file in [`CSharp/`](CSharp). Versions are listed from oldest to newest.

## .NET Framework 1.0 / C# 1.0 (2002)
**Features & Examples:**

- **Classes and structs (reference vs value types)** — [`1_ClassesAndStructs.cs`](CSharp/1_ClassesAndStructs.cs)
    ```csharp
    struct P { public int X; }
    P a = new P(); P b = a; b.X = 5;
    Console.WriteLine(a.X); // 0 - structs are copied
    ```

- **Inheritance, polymorphism and interfaces** — [`1_InheritanceAndInterfaces.cs`](CSharp/1_InheritanceAndInterfaces.cs)
    ```csharp
    abstract class Shape { public abstract double Area(); }
    class Square : Shape { public override double Area() { return 4; } }
    Shape s = new Square(); Console.WriteLine(s.Area()); // 4
    ```

- **Properties and indexers** — [`1_PropertiesAndIndexers.cs`](CSharp/1_PropertiesAndIndexers.cs)
    ```csharp
    private int _age;
    public int Age { get { return _age; } set { _age = value; } }
    public string this[int i] { get { return _items[i]; } }
    ```

- **Delegates and events** — [`1_DelegatesAndEvents.cs`](CSharp/1_DelegatesAndEvents.cs)
    ```csharp
    delegate int MathOp(int a, int b);
    MathOp op = new MathOp(Add);
    Console.WriteLine(op(2, 3)); // 5
    ```

- **Enums (and [Flags])** — [`1_Enums.cs`](CSharp/1_Enums.cs)
    ```csharp
    enum Status { Pending, Shipped, Delivered }
    Status s = Status.Shipped; Console.WriteLine((int)s); // 1
    ```

- **Exception handling (try / catch / finally)** — [`1_ExceptionHandling.cs`](CSharp/1_ExceptionHandling.cs)
    ```csharp
    try { int.Parse("abc"); }
    catch (FormatException) { Console.WriteLine("Not a number"); }
    finally { Console.WriteLine("Always runs"); }
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
    void Double(ref int n) { n *= 2; }
    int Sum(params int[] nums) { /* ... */ }
    int x = 21; Double(ref x); Console.WriteLine(x); // 42
    ```

- **Operator overloading** — [`1_OperatorOverloading.cs`](CSharp/1_OperatorOverloading.cs)
    ```csharp
    public static Money operator +(Money a, Money b)
    {
        return new Money(a.Amount + b.Amount);
    }
    ```

- **Attributes** — [`1_Attributes.cs`](CSharp/1_Attributes.cs)
    ```csharp
    [Obsolete("Use NewMethod instead")]
    void OldMethod() { }
    ```

---

## .NET Framework 2.0 / C# 2.0 (2005)
**Features & Examples:**

- **Generics** — [`2_Generics.cs`](CSharp/2_Generics.cs)
    ```csharp
    T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;
    List<int> numbers = new List<int>(); // type-safe, no boxing
    ```

- **Nullable value types (`T?`) and `??`** — [`2_NullableValueTypes.cs`](CSharp/2_NullableValueTypes.cs)
    ```csharp
    int? age = null;
    Console.WriteLine(age ?? 18); // 18
    ```

- **Iterators (`yield return`)** — [`2_Iterators.cs`](CSharp/2_Iterators.cs)
    ```csharp
    IEnumerable<int> Evens() { for (int i = 0; i <= 6; i += 2) yield return i; }
    foreach (int n in Evens()) Console.Write(n); // 0246
    ```

- **Anonymous methods** — [`2_AnonymousMethods.cs`](CSharp/2_AnonymousMethods.cs)
    ```csharp
    Predicate<int> isBig = delegate (int n) { return n > 7; };
    Console.WriteLine(isBig(10)); // True
    ```

- **Partial types** — [`2_PartialTypes.cs`](CSharp/2_PartialTypes.cs)
    ```csharp
    partial class Order { public int Id; }          // Order.Generated.cs
    partial class Order { public void Save() { } }  // Order.cs
    ```

- **Static classes** — [`2_StaticClasses.cs`](CSharp/2_StaticClasses.cs)
    ```csharp
    static class Converter { public static double ToF(double c) { return c * 9 / 5 + 32; } }
    Console.WriteLine(Converter.ToF(100)); // 212
    ```

- **Property accessor accessibility** — [`2_PropertyAccessorAccessibility.cs`](CSharp/2_PropertyAccessorAccessibility.cs)
    ```csharp
    public decimal Total { get { return _total; } private set { _total = value; } }
    ```

---

## .NET Framework 3.5 / C# 3.0 (2007)
**Features & Examples:**

- **Implicitly typed locals (`var`)** — [`3_ImplicitlyTypedLocals.cs`](CSharp/3_ImplicitlyTypedLocals.cs)
    ```csharp
    var scores = new Dictionary<string, int>(); // type inferred at compile time
    ```

- **Auto-implemented properties** — [`3_AutoImplementedProperties.cs`](CSharp/3_AutoImplementedProperties.cs)
    ```csharp
    class Book { public string Title { get; set; } public bool IsPublished { get; private set; } }
    ```

- **Object and collection initializers** — [`3_ObjectAndCollectionInitializers.cs`](CSharp/3_ObjectAndCollectionInitializers.cs)
    ```csharp
    var c = new Customer { Name = "Bob", Age = 25 };
    var nums = new List<int> { 1, 2, 3 };
    ```

- **Anonymous types** — [`3_AnonymousTypes.cs`](CSharp/3_AnonymousTypes.cs)
    ```csharp
    var person = new { Name = "Alice", Age = 30 };
    Console.WriteLine(person); // { Name = Alice, Age = 30 }
    ```

- **Extension methods** — [`3_ExtensionMethods.cs`](CSharp/3_ExtensionMethods.cs)
    ```csharp
    static class Ext { public static bool IsEven(this int n) => n % 2 == 0; }
    Console.WriteLine(4.IsEven()); // True
    ```

- **Lambda expressions (`Func<>`, `Action<>`)** — [`3_LambdaExpressions.cs`](CSharp/3_LambdaExpressions.cs)
    ```csharp
    Func<int, int> square = x => x * x;
    Console.WriteLine(square(5)); // 25
    ```

- **LINQ** — [`3_Linq.cs`](CSharp/3_Linq.cs)
    ```csharp
    var names = from s in staff where s.Salary > 60000 select s.Name;
    var same  = staff.Where(s => s.Salary > 60000).Select(s => s.Name);
    ```

- **Expression trees** — [`3_ExpressionTrees.cs`](CSharp/3_ExpressionTrees.cs)
    ```csharp
    Expression<Func<int, bool>> expr = x => x > 5;
    Console.WriteLine(expr);                // x => (x > 5)
    Console.WriteLine(expr.Compile()(10));  // True
    ```

- **Partial methods** — [`3_PartialMethods.cs`](CSharp/3_PartialMethods.cs)
    ```csharp
    partial void OnNameChanged(string newName);  // call removed if never implemented
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
    void Greet(string name = "World") => Console.WriteLine($"Hello {name}");
    Greet(); Greet(name: "Alice");
    ```

- **Generic covariance and contravariance (`out` / `in`)** — [`4_GenericVariance.cs`](CSharp/4_GenericVariance.cs)
    ```csharp
    IEnumerable<Animal> animals = new List<Dog>();      // covariance
    Action<Dog> feedDog = (Action<Animal>)(a => { });   // contravariance
    ```

---

## .NET Framework 4.5 / C# 5.0 (2012)
**Features & Examples:**

- **async/await** — [`5_AsyncAwait.cs`](CSharp/5_AsyncAwait.cs)
    ```csharp
    async Task SayHi() { await Task.Delay(100); Console.WriteLine("Hi"); }
    await SayHi();
    ```

- **Caller info attributes** — [`5_CallerInfo.cs`](CSharp/5_CallerInfo.cs)
    ```csharp
    void Log([CallerMemberName] string m = "") => Console.WriteLine(m);
    Log(); // prints the calling method's name
    ```

---

## .NET Framework 4.6 / C# 6.0 (2015)
**Features & Examples:**

- **String interpolation** — [`6_StringInterpolation.cs`](CSharp/6_StringInterpolation.cs)
    ```csharp
    var name = "Bob"; var price = 9.5m;
    Console.WriteLine($"Hello {name}, you owe {price:C}");
    ```

- **Null-conditional operator (`?.`)** — [`6_NullConditionalOperator.cs`](CSharp/6_NullConditionalOperator.cs)
    ```csharp
    string s = null;
    Console.WriteLine(s?.Length); // no exception, prints empty
    ```

- **Expression-bodied members** — [`6_ExpressionBodiedMembers.cs`](CSharp/6_ExpressionBodiedMembers.cs)
    ```csharp
    int Add(int a, int b) => a + b;
    Console.WriteLine(Add(2, 3));
    ```

- **Auto-property initializers** — [`6_AutoPropertyInitializers.cs`](CSharp/6_AutoPropertyInitializers.cs)
    ```csharp
    class P { public int X { get; set; } = 4; }
    Console.WriteLine(new P().X);
    ```

- **Using static directives** — [`6_StaticDirectives.cs`](CSharp/6_StaticDirectives.cs)
    ```csharp
    using static System.Math;
    Console.WriteLine(Sqrt(16));
    ```

- **Exception filters** — [`6_ExceptionFilters.cs`](CSharp/6_ExceptionFilters.cs)
    ```csharp
    try { throw new Exception("Oof"); }
    catch (Exception e) when (e.Message.Contains("Oof"))
    { Console.WriteLine("Filtered!"); }
    ```

- **nameof operator** — [`6_Nameof.cs`](CSharp/6_Nameof.cs)
    ```csharp
    void Send(string to)
    {
        if (to == null) throw new ArgumentNullException(nameof(to)); // "to"
    }
    ```

---

## .NET Framework 4.7 / .NET Core 2.x / C# 7.0–7.3 (2017–2018)
**Features & Examples:**

- **Tuples & deconstruction** (7.0) — [`7_TuplesAndDeconstruction.cs`](CSharp/7_TuplesAndDeconstruction.cs)
    ```csharp
    (int Sum, int Product) Calc(int a, int b) => (a + b, a * b);
    var (sum, product) = Calc(2, 3);
    Console.WriteLine(sum + product); // 11
    ```

- **Pattern matching (`is`, `switch`)** (7.0) — [`7_PatternMatching.cs`](CSharp/7_PatternMatching.cs)
    ```csharp
    object o = 5;
    if (o is int x) Console.WriteLine(x + 1);
    ```

- **Local functions** (7.0) — [`7_LocalFunctions.cs`](CSharp/7_LocalFunctions.cs)
    ```csharp
    void Run()
    {
        int Add(int a, int b) => a + b; // declared inside a method
        Console.WriteLine(Add(2, 3));
    }
    ```

- **`out` variables anywhere** (7.0) — [`7_OutVariablesAnywhere.cs`](CSharp/7_OutVariablesAnywhere.cs)
    ```csharp
    if (int.TryParse("5", out int result)) Console.WriteLine(result);
    ```

- **Discards (`_`)** (7.0) — [`7_Discards.cs`](CSharp/7_Discards.cs)
    ```csharp
    if (int.TryParse("4", out _)) Console.WriteLine("Parsed!");
    (_, int y) = (7, 8);          // ignore a tuple element
    _ = DoSomething();            // ignore a return value
    ```

- **Ref returns and locals** (7.0) — [`7_RefReturnsAndLocals.cs`](CSharp/7_RefReturnsAndLocals.cs)
    ```csharp
    ref int First(int[] arr) => ref arr[0];
    int[] nums = { 1, 2 }; ref int r = ref First(nums); r = 99;
    Console.WriteLine(nums[0]); // 99
    ```

- **Digit separators and binary literals** (7.0) — [`7_DigitSeparators.cs`](CSharp/7_DigitSeparators.cs)
    ```csharp
    int billion = 1_000_000_000;
    byte mask = 0b_0000_0101;
    ```

- **throw expressions** (7.0) — [`7_ThrowExpressions.cs`](CSharp/7_ThrowExpressions.cs)
    ```csharp
    _name = name ?? throw new ArgumentNullException(nameof(name));
    ```

- **Generalized async return types (`ValueTask<T>`)** (7.0) — [`7_ValueTask.cs`](CSharp/7_ValueTask.cs)
    ```csharp
    ValueTask<decimal> GetPrice(string sku) =>
        _cache.TryGetValue(sku, out var p) ? new(p) : new(LoadAsync(sku)); // no allocation on cache hit
    ```

- **default literal expressions** (7.1) — [`7_DefaultLiteralExpressions.cs`](CSharp/7_DefaultLiteralExpressions.cs)
    ```csharp
    string s = default;   // instead of default(string)
    Console.WriteLine(s ?? "null");
    ```

- **readonly struct and `in` parameters** (7.2) — [`7_ReadOnlyStructsAndInParams.cs`](CSharp/7_ReadOnlyStructsAndInParams.cs)
    ```csharp
    readonly struct Vector3 { public double X { get; } /* ... */ }
    double Length(in Vector3 v) => Math.Sqrt(v.X * v.X); // passed by read-only reference
    ```

- **Span<T> and ref structs** (7.2) — [`7_SpanAndRefStructs.cs`](CSharp/7_SpanAndRefStructs.cs)
    ```csharp
    int[] numbers = { 0, 1, 2, 3, 4 };
    Span<int> middle = numbers.AsSpan(1, 3); // a view, no copy
    middle[0] = 100; Console.WriteLine(numbers[1]); // 100
    ```

---

## .NET Core 3.0 / C# 8.0 (2019)
**Features & Examples:**

- **Nullable reference types** — [`8_NullableReferenceTypes.cs`](CSharp/8_NullableReferenceTypes.cs)
    ```csharp
    string? s = null;           // may be null
    Console.WriteLine(s.Length); // compiler warning CS8602
    ```

- **Async streams (`await foreach`)** — [`8_AsyncStreams.cs`](CSharp/8_AsyncStreams.cs)
    ```csharp
    async IAsyncEnumerable<int> Get() { await Task.Delay(10); yield return 5; }
    await foreach (var i in Get()) Console.WriteLine(i);
    ```

- **Switch expressions** — [`8_SwitchExpressions.cs`](CSharp/8_SwitchExpressions.cs)
    ```csharp
    int x = 3;
    var res = x switch { 1 => "One", 3 => "Three", _ => "Other" };
    Console.WriteLine(res);
    ```

- **Indices and ranges (`^`, `..`)** — [`8_InidicesAndRanges.cs`](CSharp/8_InidicesAndRanges.cs)
    ```csharp
    var arr = new[] { 1, 2, 3, 4 };
    Console.WriteLine(arr[^1]);                     // 4
    Console.WriteLine(string.Join(",", arr[1..3])); // 2,3
    ```

- **Null-coalescing assignment (`??=`)** — [`8_NullCoalescingAssignment.cs`](CSharp/8_NullCoalescingAssignment.cs)
    ```csharp
    List<int>? list = null;
    list ??= new List<int>(); // assigned only if null
    ```

- **using declarations and `await using`** — [`8_UsingDeclarations.cs`](CSharp/8_UsingDeclarations.cs)
    ```csharp
    using var reader = new StreamReader("file.txt"); // disposed at end of scope
    ```

- **Readonly members** — [`8_ReadOnlyMembers.cs`](CSharp/8_ReadOnlyMembers.cs)
    ```csharp
    struct Point
    {
        public double X { get; set; }
        public readonly double Doubled() => X * 2; // can't modify the struct
    }
    ```

- **Default interface methods** — [`8_DefaultInterfaceMethods.cs`](CSharp/8_DefaultInterfaceMethods.cs)
    ```csharp
    interface ILogger { void Log(string m) => Console.WriteLine(m); }
    class C : ILogger { }  // uses the default implementation
    ((ILogger)new C()).Log("Default");
    ```

---

## .NET 5 / C# 9.0 (2020)
**Features & Examples:**

- **Records** — [`9_Records.cs`](CSharp/9_Records.cs)
    ```csharp
    record Point(int X, int Y);
    Console.WriteLine(new Point(1, 2) == new Point(1, 2)); // True - value equality
    ```

- **Init-only setters** — [`9_InitOnlySetters.cs`](CSharp/9_InitOnlySetters.cs)
    ```csharp
    class P { public int X { get; init; } }
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
    Console.WriteLine("Hello World!");
    ```

- **Pattern matching enhancements (relational, `and` / `or` / `not`)** — [`9_PatternMatchingEnhancements.cs`](CSharp/9_PatternMatchingEnhancements.cs)
    ```csharp
    int score = 85;
    if (score is >= 80 and < 90) Console.WriteLine("Good job!");
    if (name is not null) Console.WriteLine(name);
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
    global using System.Text;   // now available in every file
    ```

- **File-scoped namespaces** — [`10_FileScopedNamespaces.cs`](CSharp/10_FileScopedNamespaces.cs)
    ```csharp
    namespace Demo;
    class C { }
    ```

- **Record structs** — [`10_RecordStructs.cs`](CSharp/10_RecordStructs.cs)
    ```csharp
    public readonly record struct Point(int X, int Y);
    ```

- **Lambda improvements (natural type)** — [`10_LambdaImprovements.cs`](CSharp/10_LambdaImprovements.cs)
    ```csharp
    var f = (int x) => x + 1; // inferred as Func<int, int>
    Console.WriteLine(f(3));
    ```

- **Extended property patterns** — [`10_ExtendedPropertyPatterns.cs`](CSharp/10_ExtendedPropertyPatterns.cs)
    ```csharp
    if (order is { Customer.Address.City: "Paris" }) Console.WriteLine("Paris order");
    ```

---

## .NET 7 / C# 11.0 (2022)
**Features & Examples:**

- **Raw string literals** — [`11_RawStringLiterals.cs`](CSharp/11_RawStringLiterals.cs)
    ```csharp
    var json = """
        { "name": "Alice", "path": "C:\temp" }
        """;
    Console.WriteLine(json);
    ```

- **List patterns** — [`11_ListPatterns.cs`](CSharp/11_ListPatterns.cs)
    ```csharp
    var data = new[] { 1, 2, 3 };
    if (data is [1, .., 3]) Console.WriteLine("Pattern match!");
    ```

- **Generic attributes** — [`11_GenericAttributes.cs`](CSharp/11_GenericAttributes.cs)
    ```csharp
    class TypedAttribute<T> : Attribute { }
    [Typed<int>] class C { }
    ```

- **Required members** — [`11_RequiredMembers.cs`](CSharp/11_RequiredMembers.cs)
    ```csharp
    class User { public required string Name { get; set; } }
    // var u = new User(); // Error CS9035: Name must be set
    var u = new User { Name = "Ann" };
    ```

- **Generic math (static abstract interface members)** — [`11_GenericMath.cs`](CSharp/11_GenericMath.cs)
    ```csharp
    T Sum<T>(IEnumerable<T> values) where T : INumber<T>
    {
        T total = T.Zero;
        foreach (var v in values) total += v;
        return total;
    }
    Console.WriteLine(Sum(new[] { 1.5, 2.5 })); // 4
    ```

---

## .NET 8 / C# 12.0 (2023)
**Features & Examples:**

- **Collection expressions** — [`12_CollectionExpressions.cs`](CSharp/12_CollectionExpressions.cs)
    ```csharp
    int[] first = [1, 2, 3];
    List<int> all = [0, .. first, 4]; // spread operator
    Console.WriteLine(all.Count);     // 5
    ```

- **Primary constructors for classes** — [`12_PrimaryConstructorsForClasses.cs`](CSharp/12_PrimaryConstructorsForClasses.cs)
    ```csharp
    class GreetingService(ILog log)
    {
        public void Greet(string name) => log.Write($"Hello {name}");
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
    Console.WriteLine(add()); // 3
    ```

- **Inline arrays** — [`12_InlineArrays.cs`](CSharp/12_InlineArrays.cs)
    ```csharp
    [System.Runtime.CompilerServices.InlineArray(3)]
    struct Buffer { private int _element0; } // exactly one field, repeated 3 times
    var buf = new Buffer(); buf[0] = 10;
    ```

---

## .NET 9 / C# 13.0 (2024)
**Features & Examples:**

- **params collections** — [`13_ParamsCollectioncs.cs`](CSharp/13_ParamsCollectioncs.cs)
    ```csharp
    void Print(params ReadOnlySpan<string> items) { } // no array allocation
    Print("one", "two", "three");
    ```

- **New `Lock` type** — [`13_LockType.cs`](CSharp/13_LockType.cs)
    ```csharp
    private readonly Lock _sync = new();
    lock (_sync) { /* critical section - uses Lock.EnterScope() */ }
    ```

- **Pattern matching in practice** (no new syntax in C# 13; combines C# 8–9 patterns) — [`13_PatternMatchingEnhancements.cs`](CSharp/13_PatternMatchingEnhancements.cs)
    ```csharp
    string Classify(int age) => age switch
    {
        < 0 => "Invalid", <= 12 => "Child", <= 17 => "Teenager", _ => "Adult"
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
            public bool IsBlank => string.IsNullOrWhiteSpace(text); // extension PROPERTY
        }
    }
    Console.WriteLine("   ".IsBlank); // True
    ```

- **Null-conditional assignment (`?.` on the left side)** — [`14_NullConditionalAssignment.cs`](CSharp/14_NullConditionalAssignment.cs)
    ```csharp
    customer?.Name = "Alice"; // assigns only if customer isn't null
    ```

---

**References:**
- [C# language history](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history)  
- [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)  
- [.NET docs](https://learn.microsoft.com/en-us/dotnet/core/)
