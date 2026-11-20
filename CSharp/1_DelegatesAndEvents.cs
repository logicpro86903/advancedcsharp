// =============================================================================
//  Feature     : Delegates and Events
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    A DELEGATE is a type-safe reference to a method (a "function pointer" that
//    knows the exact signature). It lets you pass behavior around as a value,
//    store it, and call it later. Delegates are MULTICAST: one delegate can hold
//    a list of methods that all run when it is invoked.
//    An EVENT is a delegate wrapped with access restrictions: outsiders can only
//    subscribe (+=) or unsubscribe (-=); only the declaring class can raise it.
//    This is the foundation of the publisher/subscriber (observer) pattern in .NET.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class DelegatesAndEvents
    {
        public static void Run()
        {
            DemonstrateDelegates();
            DemonstrateMulticast();
            DemonstrateEvents();
        }

        // DELEGATE TYPE DECLARATIONS
        // - A delegate declaration defines a new TYPE describing a method signature.
        // - MathOperation can point to ANY method taking (int, int) and returning int,
        //   whether static or instance, regardless of its name.
        public delegate int MathOperation(int a, int b);
        public delegate void Notify(string message);

        static int Add(int a, int b) { return a + b; }
        static int Multiply(int a, int b) { return a * b; }

        // Delegates as parameters: the caller decides WHAT operation to run (strategy pattern).
        static int Calculate(int a, int b, MathOperation op) { return op(a, b); }

        // BASIC DELEGATE USAGE
        // - In C# 1 you must explicitly construct a delegate: new MathOperation(Add).
        //   (C# 2 allows the shorter "op = Add;" - method group conversion.)
        // - Invoking the delegate, op(3, 4), calls whichever method it currently points to.
        // - The same variable can be re-pointed to a different compatible method.
        public static void DemonstrateDelegates()
        {
            MathOperation op = new MathOperation(Add);
            Console.WriteLine($"Add: {op(3, 4)}");                              // Add: 7

            op = new MathOperation(Multiply);
            Console.WriteLine($"Multiply: {op(3, 4)}");                         // Multiply: 12

            Console.WriteLine($"Calculate: {Calculate(10, 5, new MathOperation(Add))}"); // Calculate: 15
        }

        static void LogToConsole(string message) { Console.WriteLine("[Console] " + message); }
        static void LogToAudit(string message) { Console.WriteLine("[Audit]   " + message); }

        // MULTICAST DELEGATES
        // - Every delegate holds an "invocation list". += appends a method, -= removes it.
        // - Invoking the delegate calls every method in the list, in the order added.
        // - Removing the last method leaves the delegate NULL, so check before invoking.
        // - For non-void delegates only the LAST method's return value is kept.
        public static void DemonstrateMulticast()
        {
            Notify? notify = new Notify(LogToConsole);
            notify += new Notify(LogToAudit);
            notify("User logged in");          // [Console] ... then [Audit] ...

            notify -= new Notify(LogToAudit);
            if (notify != null)
                notify("User logged out");     // [Console] only
        }

        // EVENT PUBLISHER
        // - 'event' keyword: outside code may only += or -= handlers; it cannot invoke the
        //   event or overwrite the subscriber list with '='. Only Thermostat can raise it.
        // - Standard .NET event pattern:
        //     * handler signature (object sender, XxxEventArgs e)
        //     * a protected virtual On<EventName>(e) method that raises the event, so derived
        //       classes can override/extend the raising logic
        //     * copy the delegate to a local before the null check so another thread can't
        //       unsubscribe between the check and the call
        public class Thermostat
        {
            public delegate void TemperatureChangedHandler(object sender, TemperatureEventArgs e);
            public event TemperatureChangedHandler? TemperatureChanged;

            private int _temperature;

            public int Temperature
            {
                get { return _temperature; }
                set
                {
                    if (_temperature == value) return;
                    _temperature = value;
                    OnTemperatureChanged(new TemperatureEventArgs(value));
                }
            }

            protected virtual void OnTemperatureChanged(TemperatureEventArgs e)
            {
                TemperatureChangedHandler? handler = TemperatureChanged;
                if (handler != null)
                    handler(this, e);
            }
        }

        // Event data class: by convention derives from EventArgs and carries the details.
        public class TemperatureEventArgs : EventArgs
        {
            public readonly int NewTemperature;
            public TemperatureEventArgs(int newTemperature) { NewTemperature = newTemperature; }
        }

        // Subscriber method - matches the TemperatureChangedHandler signature.
        static void OnTempChanged(object sender, TemperatureEventArgs e)
        {
            Console.WriteLine($"Temperature changed to {e.NewTemperature}°C");
            if (e.NewTemperature > 30)
                Console.WriteLine("  -> Turning on the AC");
        }

        // SUBSCRIBING TO AN EVENT
        // - The subscriber attaches a handler with +=.
        // - Setting the same temperature twice raises no event (the setter checks for change).
        // - The commented lines show what the 'event' keyword forbids from outside the class.
        public static void DemonstrateEvents()
        {
            Thermostat thermostat = new Thermostat();
            thermostat.TemperatureChanged += new Thermostat.TemperatureChangedHandler(OnTempChanged);

            thermostat.Temperature = 22;
            thermostat.Temperature = 22;   // unchanged -> no event
            thermostat.Temperature = 35;

            // thermostat.TemperatureChanged(...);   // Compile error: only Thermostat can raise it
            // thermostat.TemperatureChanged = null; // Compile error: outsiders can only += / -=
        }
    }

    /*

    Pitfalls:
    //1. Forgetting to unsubscribe (-=) keeps the subscriber alive -> memory leaks.
    //2. Raising an event with no subscribers throws NullReferenceException (check for null,
    //   or in C# 6+ use TemperatureChanged?.Invoke(this, e)).

    Later versions simplified this a lot: C# 2 method group conversion + anonymous methods,
    C# 3 lambdas, and the built-in Func<>/Action<>/EventHandler<T> delegate types.

     */
}
