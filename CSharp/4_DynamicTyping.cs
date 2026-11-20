// =============================================================================
//  Feature     : Dynamic Typing (dynamic keyword)
//  C# Version  : C# 4.0
//  .NET        : .NET Framework 4.0
//  Released    : 2010 (Visual Studio 2010)
// -----------------------------------------------------------------------------
//  Summary:
//    A variable declared as 'dynamic' skips compile-time type checking. Member
//    access, method calls and operators on it are resolved at RUNTIME by the
//    Dynamic Language Runtime (DLR). This simplifies interop with dynamic
//    languages (IronPython, JavaScript), COM automation (Office/Excel), and
//    loosely-structured data, at the cost of IntelliSense, compile-time safety
//    and some performance. Mistakes show up as RuntimeBinderException.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public static class DynamicTyping
    {
        // DYNAMIC VARIABLES
        // - 'dyn' can hold a string, then an int, then a List<int>: its type is whatever
        //   the current value is, decided at runtime (unlike 'var', which is fixed at compile time).
        // - Console.WriteLine(dyn) picks the right overload at RUNTIME based on the actual type.
        // - foreach over dyn works because, at runtime, the value is an IEnumerable.
        // - Any member call compiles - even one that doesn't exist - and only fails when executed.
        public static void Run()
        {
            dynamic dyn = "Hello, World!";
            Console.WriteLine(dyn);

            dyn = 42;
            Console.WriteLine(dyn);

            dyn = new List<int> { 1, 2, 3 };
            foreach (var item in dyn)
            {
                Console.WriteLine(item);
            }
        }
    }
}


/*

  //No Compile-Time Type Checking:
  dynamic obj = "Hello";
  obj.NonExistentMethod(); // Compiles fine, but throws RuntimeBinderException at runtime

  //Implicit conversion is checked at runtime too:
  dynamic value = GetSomeValue();
  string result = value;   // works if value is actually a string
  int number = value;      // RuntimeBinderException if value isn't convertible to int

  Common use cases:
  //1. COM interop - Excel automation without casts:
  dynamic excel = new Application();
  dynamic worksheet = excel.Workbooks.Add().ActiveSheet;
  worksheet.Cells[1, 1].Value = "Hello Excel";

  //2. Simpler reflection:
  dynamic instance = Activator.CreateInstance(someType);
  var result = instance.SomeMethod("parameter");

  //3. Loosely-typed JSON (Newtonsoft):
  dynamic json = JsonConvert.DeserializeObject(jsonString);
  var name = json.user.name;

  Limitations: no IntelliSense, errors only at runtime, slower than static typing (the DLR
  caches binding decisions, but there is still overhead), harder to debug.

 */
