// =============================================================================
//  Feature     : Properties and Indexers
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    PROPERTIES look like fields to callers (account.Balance = 10) but are really
//    a pair of get/set methods, so you can add validation, computed values or
//    change notification without breaking callers. INDEXERS use the same idea
//    to let an object be accessed with [] like an array or dictionary
//    (week[0], week["Wed"]). Both are core to C#'s encapsulation story.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PropertiesAndIndexers
    {
        public static void Run()
        {
            DemonstrateProperties();
            DemonstrateIndexers();
        }

        // PROPERTIES
        // - In C# 1 every property needs a private BACKING FIELD to store its data.
        // - 'get' runs when the property is read; 'set' runs when it is assigned, with the
        //   assigned value available through the implicit 'value' keyword.
        // - The setter can VALIDATE input before storing it.
        // - A property with only a getter is READ-ONLY; it can compute its value on the fly.
        public class BankAccount
        {
            private decimal _balance;

            public decimal Balance
            {
                get { return _balance; }
                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("value", "Balance cannot be negative");
                    _balance = value;
                }
            }

            public bool IsEmpty
            {
                get { return _balance == 0; }
            }
        }

        // INDEXERS
        // - Declared with 'this[...]', they let instances be used with square brackets.
        // - Have get/set accessors just like properties.
        // - Can be OVERLOADED by parameter type: here one indexer takes an int position and
        //   another takes a day name (string) and returns its position.
        public class WeekDays
        {
            private string[] _days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            public string this[int index]
            {
                get { return _days[index]; }
                set { _days[index] = value; }
            }

            public int this[string day]
            {
                get { return Array.IndexOf(_days, day); }
            }
        }

        // USING PROPERTIES
        // - Assigning calls the setter; reading calls the getter - same syntax as a field.
        // - Assigning an invalid value triggers the setter's validation and throws.
        public static void DemonstrateProperties()
        {
            BankAccount account = new BankAccount();
            account.Balance = 250m;
            Console.WriteLine($"Balance: {account.Balance}");          // Balance: 250
            Console.WriteLine($"Is empty: {account.IsEmpty}");         // Is empty: False

            try
            {
                account.Balance = -10m;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Rejected: {ex.Message}");
            }
        }

        // USING INDEXERS
        // - week[0] calls the int indexer's getter; week[6] = ... calls its setter.
        // - week["Wed"] calls the string overload.
        public static void DemonstrateIndexers()
        {
            WeekDays week = new WeekDays();
            Console.WriteLine(week[0]);       // Mon
            week[6] = "Sunday";
            Console.WriteLine(week[6]);       // Sunday
            Console.WriteLine(week["Wed"]);   // 2
        }
    }

    /*

    Properties compile to get_X / set_X methods. Never expose public fields on classes:
    switching a field to a property later is a breaking (binary) change for callers.

    C# 3 later added auto-implemented properties (see 3_AutoImplementedProperties.cs),
    and C# 14 added the 'field' keyword (see 13_BackingFields.cs).

     */
}
