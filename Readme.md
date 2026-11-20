# C# and .NET Features by Version (with Examples)

## .NET Framework 4.0 / C# 4.0 (2010)
**Features & Examples:**

- **Dynamic typing**
    ```csharp
    dynamic val = "Hello";
    Console.WriteLine(val.ToUpper());
    ```

- **Named and optional parameters**
    ```csharp
    void Greet(string name = "World") => Console.WriteLine($"Hello {name}");
    Greet(); Greet(name: "Alice");
    ```

- **COM Interop improvements**  
(Example: not meaningful in pure C#, mostly about integration.)

---

## .NET 4.5 / C# 5.0 (2012)
**Features & Examples:**

- **async/await**
    ```csharp
    async Task SayHi() { await Task.Delay(100); Console.WriteLine("Hi"); }
    SayHi().Wait();
    ```

- **Caller info attributes**
    ```csharp
    void Log([CallerMemberName] string m = "") => Console.WriteLine(m);
    Log(); // Logs "Log"
    ```

---

## .NET 4.6 / C# 6.0 (2015)
**Features & Examples:**

- **String interpolation**
    ```csharp
    var name = "Bob";
    Console.WriteLine($"Hello {name}");
    ```

- **Null-conditional operator (?.)**
    ```csharp
    string s = null;
    Console.WriteLine(s?.Length);
    ```

- **Expression-bodied members**
    ```csharp
    int Add(int a, int b) => a + b;
    Console.WriteLine(Add(2,3));
    ```

- **Auto-property initializers**
    ```csharp
    class P { public int X { get; set; } = 4; }
    Console.WriteLine(new P().X);
    ```

- **Using static directives**
    ```csharp
    using static System.Math;
    Console.WriteLine(Sqrt(16));
    ```

- **Exception filters**
    ```csharp
    try { throw new Exception("Oof"); }
    catch (Exception e) when(e.Message.Contains("Oof"))
    { Console.WriteLine("Filtered!"); }
    ```

---

## .NET 4.7/4.8 / C# 7.0–7.3 (2017–2018)
**Features & Examples:**

- **Tuples & deconstruction**
    ```csharp
    var (a, b) = (1, 2);
    Console.WriteLine(a + b); // 3
    ```

- **Pattern matching (`is`, `switch`)**
    ```csharp
    object o = 5;
    if (o is int x) Console.WriteLine(x + 1);
    ```

- **Local functions**
    ```csharp
    void PrintHi() { Console.WriteLine("Hi"); }
    PrintHi();
    ```

- **`out` variables anywhere**
    ```csharp
    if (int.TryParse("5", out int result)) Console.WriteLine(result);
    ```

- **Discards (`_`)**
    ```csharp
    if (int.TryParse("4", out _)) Console.WriteLine("Parsed!");
    (_, int y) = (7, 8); Console.WriteLine(y);
    ```

- **Ref returns and locals**
    ```csharp
    ref int Pick(ref int a, ref int b) => ref a;
    int n = 5, m = 8; ref int r = ref Pick(ref n, ref m); r = 99; Console.Write(n);
    ```

- **Digit separators**
    ```csharp
    int billion = 1_000_000_000;
    Console.WriteLine(billion);
    ```

- **default literal expressions**
    ```csharp
    string s = default;
    Console.WriteLine(s ?? "null");
    ```

---

## .NET Core 3.x / .NET 5 / C# 8.0 (2019)
**Features & Examples:**

- **Nullable reference types**
    ```csharp
    string? s = null;
    if (s == null) Console.WriteLine("Null!");
    ```

- **Async streams (`await foreach`)**
    ```csharp
    async IAsyncEnumerable<int> Get() { yield return 5; }
    await foreach (var i in Get()) Console.WriteLine(i);
    ```

- **Switch expressions**
    ```csharp
    int x = 3;
    var res = x switch { 1 => "One", 3 => "Three", _ => "Other" };
    Console.WriteLine(res);
    ```

- **Indices and ranges (`^`, `..`)**
    ```csharp
    var arr = new[] {1,2,3};
    Console.WriteLine(arr[^1]); // prints 3
    ```

- **Readonly members**
    ```csharp
    struct S { public readonly int X; public S(int x) => X = x; }
    ```

- **Default interface methods**
    ```csharp
    interface I { void A() => Console.WriteLine("Default"); }
    class C : I {} // Uses default implementation
    ```

---

## .NET 5 / C# 9.0 (2020)
**Features & Examples:**

- **Records**
    ```csharp
    record Point(int X, int Y);
    var p = new Point(1,2);
    ```

- **Init-only setters**
    ```csharp
    class P { public int X { get; init; } }
    var p = new P { X = 3 };
    ```

- **Top-level statements**
    ```csharp
    Console.WriteLine("Hello World!");
    ```

- **Pattern matching enhancements**
    ```csharp
    object o = 5;
    if (o is > 4 and < 10) Console.WriteLine("Match!");
    ```

- **with-expressions**
    ```csharp
    var u1 = new Point(1,2);
    var u2 = u1 with { Y = 5 };
    Console.WriteLine(u2);
    ```

---

## .NET 6 / C# 10.0 (2021)
**Features & Examples:**

- **Global using directives**  
    ```csharp
    // global using System;   (in GlobalUsings.cs)
    Console.WriteLine("Global using!");
    ```

- **File-scoped namespaces**
    ```csharp
    namespace Demo;
    class C { }
    ```

- **Record structs**
    ```csharp
    public record struct Point(int X, int Y);
    ```

- **Lambda improvements**
    ```csharp
    var f = (int x) => x + 1;
    Console.WriteLine(f(3));
    ```

---

## .NET 7 / C# 11.0 (2022)
**Features & Examples:**

- **Raw string literals**
    ```csharp
    var s = """
    Line1
    Line2
    """;
    Console.WriteLine(s);
    ```

- **List patterns**
    ```csharp
    var data = new[] {1, 2, 3};
    if (data is [1, .., 3]) Console.WriteLine("Pattern match!");
    ```

- **Generic attributes**
    ```csharp
    class My<T> : Attribute {}
    [My<int>] class C {}
    ```

- **Required members**
    ```csharp
    class D { public required int X { get; set; } }
    // var d = new D(); // Error: X required
    ```

---

## .NET 8 / C# 12.0 (2023)
**Features & Examples:**

- **Collection expressions**
    ```csharp
    var list = [1, 2, 3];
    Console.WriteLine(list[1]);
    ```

- **Primary constructors for structs**
    ```csharp
    struct S(int x) { }
    ```

- **Default lambda parameters**
    ```csharp
    var add = (int x = 1, int y = 2) => x + y;
    Console.WriteLine(add());
    ```

- **Inline arrays**
    ```csharp
    [System.Runtime.CompilerServices.InlineArray(3)]
    struct Buffer { public int e0, e1, e2; }
    var buf = new Buffer(); buf[0] = 10;
    ```

---

## .NET 9 / C# 13.0 (2024)
**Features & Examples:**  
_(Some features based on previews/specs as of end of 2024.)_

- **Parameter null-checking (`!!`)**
    ```csharp
    void Say(string msg!!) => Console.WriteLine(msg.Length);
    Say("hi");
    ```

- **Pattern matching enhancements**
    ```csharp
    object o = 7;
    if (o is int x and >= 5 and <= 10) Console.WriteLine("In range!");
    ```

- **Required parameters**
    ```csharp
    void Foo(string name!) => Console.WriteLine(name);
    Foo("Bar");
    ```

---

## .NET 10 / C# 14.0 (2025)
**Features & Examples:**  
_(Many features are previews/speculated from available sources.)_

- **Primary constructors for all types**
    ```csharp
    class Emp(string name, int age) {
        void Show() => Console.WriteLine($"{name}, {age}");
    }
    new Emp("Dan", 33).Show();
    ```

- **AI-powered features, further simplifications**  
(No concrete code—varies by implementation.)

---

## Discards Summary Example (C# 7+)
- **In out variables:**  
    ```csharp
    if (int.TryParse("8", out _)) Console.WriteLine("Parsed!");
    ```

- **Tuple deconstruction:**
    ```csharp
    (_, int y) = (1, 2);
    ```

- **Pattern and match ignore:**
    ```csharp
    if (o is string _) Console.WriteLine("Was string!");
    ```

- **Ignore return value:**
    ```csharp
    _ = DoSomething();
    ```

---

**References:**
- [C# language history](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history)  
- [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)  
- [.NET docs](https://learn.microsoft.com/en-us/dotnet/core/)