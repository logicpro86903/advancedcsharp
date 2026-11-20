using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 9 / C# 13.0 (2024)
    public class ParamsCollectioncs
    {
        public void Run()
        {

            Print1("one", "two", "three");  // Works with all overloads
        }

        // Before C# 13: Only arrays
        //void Print(params string[] items) { }

        // C# 13: Multiple collection types
        void Print1(params IEnumerable<string> items) { }
        void Print2(params List<string> items) { }
        void Print(params Span<string> items) { }
        void Print(params ReadOnlySpan<string> items) { }
    }
}
