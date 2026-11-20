// =============================================================================
//  Feature     : Property Accessor Accessibility
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    The get and set accessors of a property (or indexer) can have DIFFERENT
//    access levels, e.g. 'public get; private set;'. This makes "publicly
//    readable, privately writable" state easy to express. In C# 1 both accessors
//    shared the property's access level, so you needed a read-only property plus
//    direct writes to the backing field.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PropertyAccessorAccessibility
    {
        // USING THE CART FROM OUTSIDE
        // - Total and ItemCount can be READ by anyone.
        // - They can only be CHANGED through AddItem(); assigning Total directly from here
        //   is a compile error because its setter is private.
        public static void Run()
        {
            ShoppingCart cart = new ShoppingCart();
            cart.AddItem(10m);
            cart.AddItem(25m);

            Console.WriteLine($"Items: {cart.ItemCount}, Total: {cart.Total}"); // Items: 2, Total: 35
            // cart.Total = 0;   // compile error: the setter is private
        }

        // ASYMMETRIC ACCESSOR ACCESSIBILITY
        // - 'private set'   : only code inside ShoppingCart may assign Total.
        // - 'protected set' : ShoppingCart and classes DERIVED from it may assign ItemCount.
        // - Inside the class, the property is used normally (Total += price), so any setter
        //   logic is still applied - unlike writing to the backing field directly.
        public class ShoppingCart
        {
            private decimal _total;
            private int _itemCount;

            public decimal Total
            {
                get { return _total; }
                private set { _total = value; }
            }

            public int ItemCount
            {
                get { return _itemCount; }
                protected set { _itemCount = value; }
            }

            public void AddItem(decimal price)
            {
                Total += price;
                ItemCount++;
            }
        }
    }

    /*

    Rules:
    //1. Only ONE accessor may have a modifier, and it must be more restrictive than the property.
    //2. Both accessors must exist to restrict one of them.

    With C# 3 auto-properties this becomes:  public decimal Total { get; private set; }
    C# 9 added init-only setters (see 9_InitOnlySetters.cs).

     */
}
