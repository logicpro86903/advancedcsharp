using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{

    //.NET FW 4 & C#4 2010
    public static class DynamicTyping
    {
        public static void Run()
        {
            //Dynamic typing in C# allows you to bypass compile-time type checking and defer type resolution
            //until runtime. This is achieved using the dynamic keyword, which was introduced in C# 4.0.
            //Interop with Dynamic Languages, COM Interop, Reflection, JSON and XML.
        

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


//Type Inference from Assignment:
dynamic value = GetSomeValue();
string result = value; // If value is actually a string, this works
int number = value;    // RuntimeBinderException if value isn't convertible to int

//Common Use Cases

//1. Interop with Dynamic Languages:
// Working with JavaScript objects, Python objects, etc.
dynamic jsObject = GetJavaScriptObject();
var name = jsObject.name;
var result = jsObject.calculate(10, 20);

//2. COM Interop:
// Excel automation without complex casting
dynamic excel = new Application();
dynamic workbook = excel.Workbooks.Add();
dynamic worksheet = workbook.ActiveSheet;
worksheet.Cells[1, 1].Value = "Hello Excel";

//3. Reflection Scenarios:
// Simpler than traditional reflection
dynamic instance = Activator.CreateInstance(someType);
var result = instance.SomeMethod("parameter");


//4. Working with JSON/XML:
dynamic json = JsonConvert.DeserializeObject(jsonString);
var name = json.user.name;
var email = json.user.email;

//Performance Considerations
//Dynamic operations are significantly slower than static operations because:

//Runtime type resolution
//Method binding overhead
//Caching mechanisms (though the DLR does cache binding decisions)

//Limitations
//No IntelliSense: IDEs can't provide autocomplete for dynamic variables
//Runtime Errors: Type-related errors only surface at runtime
//Performance: Slower than static typing
//Debugging: More difficult to debug type-related issues

 
 */
