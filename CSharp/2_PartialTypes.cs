// =============================================================================
//  Feature     : Partial Types
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    The 'partial' modifier lets one class, struct or interface be split across
//    multiple declarations (usually in multiple files). The compiler merges all
//    parts into a single type. It exists so tool-GENERATED code (WinForms/WPF
//    designers, EF scaffolding, source generators) can live in its own file and
//    be regenerated without overwriting your HAND-WRITTEN code.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PartialTypes
    {
        // USING THE MERGED TYPE
        // - Callers see ONE Order class with members from both parts:
        //   the constructor and ToXml() from part 1, AddLine() and Summary() from part 2.
        public static void Run()
        {
            Order order = new Order(1001);
            order.AddLine("Keyboard", 49.99m);
            order.AddLine("Mouse", 19.99m);

            Console.WriteLine(order.Summary());   // Order 1001: 2 lines, total 69.98
            Console.WriteLine(order.ToXml());
        }

        // PART 1 - typically the "generated" file (e.g. Order.Generated.cs).
        // - Declares the fields, constructor and some methods.
        public partial class Order
        {
            private int _id;
            private List<string> _lines = new List<string>();
            private decimal _total;

            public Order(int id) { _id = id; }

            public string ToXml()
            {
                return "<order id=\"" + _id + "\" lines=\"" + _lines.Count + "\" />";
            }
        }

        // PART 2 - typically the hand-written file (e.g. Order.cs).
        // - Because both parts are the SAME class, this part can freely use the private
        //   fields (_lines, _total, _id) declared in part 1.
        public partial class Order
        {
            public void AddLine(string product, decimal price)
            {
                _lines.Add(product);
                _total += price;
            }

            public string Summary()
            {
                return $"Order {_id}: {_lines.Count} lines, total {_total}";
            }
        }
    }

    /*

    Today source generators use partial heavily (e.g. [GeneratedRegex], [JsonSerializable],
    CommunityToolkit.Mvvm).

    Rules:
    //1. All parts must use 'partial', be in the same assembly and namespace.
    //2. Modifiers must agree (can't be public in one part and internal in another).
    //3. Base class may be declared in one part; interfaces from all parts are combined.

    Related: partial methods (C# 3, see 3_PartialMethods.cs), partial properties (C# 13),
    partial constructors/events (C# 14).

     */
}
