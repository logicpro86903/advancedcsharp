// =============================================================================
//  Feature     : Partial Methods
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    A partial method is DECLARED (signature only) in one part of a partial class
//    and optionally IMPLEMENTED in another part. If no implementation is
//    provided, the compiler removes the declaration AND every call to it -
//    including evaluation of the arguments - so unused hooks cost nothing.
//    Generated code uses this to offer optional extension points such as
//    OnNameChanged that hand-written code may choose to implement.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PartialMethods
    {
        // OBSERVING THE HOOKS
        // - Setting Name calls OnNameChanged, which IS implemented, so a message prints.
        // - Setting Price calls OnPriceChanged, which is NOT implemented anywhere, so the
        //   compiler has removed that call entirely - nothing happens.
        public static void Run()
        {
            var product = new TrackedProduct();
            product.Name = "Laptop";
            product.Price = 999m;
            Console.WriteLine($"{product.Name}: {product.Price}");
        }

        // "GENERATED" PART: declares hooks and calls them
        // - 'partial void OnXxx(...);' declares the hook with NO body.
        // - C# 3 rules: must return void, no 'out' parameters, implicitly private,
        //   and no access modifier may be written.
        // - The property setters invoke the hooks; whether the call survives compilation
        //   depends on whether an implementation exists.
        public partial class TrackedProduct
        {
            private string _name = "";
            private decimal _price;

            partial void OnNameChanged(string oldValue, string newValue);
            partial void OnPriceChanged(decimal newValue);

            public string Name
            {
                get { return _name; }
                set
                {
                    string old = _name;
                    _name = value;
                    OnNameChanged(old, value);
                }
            }

            public decimal Price
            {
                get { return _price; }
                set
                {
                    _price = value;
                    OnPriceChanged(value);
                }
            }
        }

        // HAND-WRITTEN PART: implements only the hooks it cares about.
        // - The implementation repeats the signature with a body.
        // - OnPriceChanged is deliberately left unimplemented.
        public partial class TrackedProduct
        {
            partial void OnNameChanged(string oldValue, string newValue)
            {
                Console.WriteLine($"Name changed from '{oldValue}' to '{newValue}'");
            }
        }
    }

    /*

    LINQ to SQL and EF designers used this for OnXxxChanging / OnXxxChanged.

    C# 9 extended them: with an explicit access modifier they may return values and have out
    params, but then an implementation is REQUIRED - this is how source generators work:
        [GeneratedRegex("\\d+")] private static partial Regex Digits();

     */
}
