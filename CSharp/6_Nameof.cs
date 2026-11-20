// =============================================================================
//  Feature     : nameof Operator
//  C# Version  : C# 6.0
//  .NET        : .NET Framework 4.6
//  Released    : 2015 (Visual Studio 2015)
// -----------------------------------------------------------------------------
//  Summary:
//    nameof(x) returns the NAME of a variable, type or member as a string
//    constant, evaluated at compile time. Hard-coded strings like "customerId"
//    silently go stale when code is renamed; nameof is checked by the compiler
//    and updated by IDE rename refactorings. Typical uses: argument validation
//    messages, INotifyPropertyChanged, logging and ASP.NET action names.
// =============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CSharp
{
    public class Nameof
    {
        public static void Run()
        {
            DemonstrateBasics();
            DemonstrateArgumentValidation();
            DemonstratePropertyChanged();
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Email { get; set; } = "";
            public void Save() { }
        }

        // NAMEOF BASICS
        // - Works on locals, types, members, methods and namespaces-qualified types.
        // - Only the LAST identifier is returned: nameof(customer.Email) is "Email".
        // - Type arguments are dropped: nameof(List<int>) is "List".
        // - The result is a compile-time CONSTANT, so it can initialize a 'const' and has
        //   zero runtime cost (no reflection involved).
        public static void DemonstrateBasics()
        {
            int orderCount = 5;
            var customer = new Customer();

            Console.WriteLine(nameof(orderCount));         // orderCount
            Console.WriteLine(nameof(Customer));           // Customer
            Console.WriteLine(nameof(customer.Email));     // Email
            Console.WriteLine(nameof(Customer.Save));      // Save
            Console.WriteLine(nameof(System.Console));     // Console
            Console.WriteLine(nameof(List<int>));          // List

            const string label = nameof(orderCount);
            Console.WriteLine(label);                      // orderCount
        }

        // USE CASE #1: ARGUMENT VALIDATION
        // - ArgumentException / ArgumentNullException take the parameter NAME.
        // - nameof(to) keeps that name correct if the parameter is ever renamed;
        //   a string literal "to" would silently become wrong.
        static void SendEmail(string to, string subject)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Recipient is required", nameof(to));

            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            Console.WriteLine($"Sending '{subject}' to {to}");
        }

        // The caught exception's ParamName shows the value produced by nameof.
        // Modern .NET helpers do this automatically via [CallerArgumentExpression] (C# 10):
        //   ArgumentNullException.ThrowIfNull(subject);
        //   ArgumentException.ThrowIfNullOrWhiteSpace(to);
        public static void DemonstrateArgumentValidation()
        {
            try
            {
                SendEmail("", "Hello");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ParamName = {ex.ParamName}");   // ParamName = to
            }
        }

        // USE CASE #2: INotifyPropertyChanged (WPF, MAUI, Blazor data binding)
        // - Bindings identify properties by NAME string. nameof(Title) produces "Title" and is
        //   updated automatically when the property is renamed.
        public class ViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private string _title = "";
            public string Title
            {
                get => _title;
                set
                {
                    _title = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                }
            }
        }

        // Subscribing shows the property name that the view model reports.
        public static void DemonstratePropertyChanged()
        {
            var vm = new ViewModel();
            vm.PropertyChanged += (sender, e) => Console.WriteLine($"Property changed: {e.PropertyName}");
            vm.Title = "Dashboard";                         // Property changed: Title
        }
    }

    /*

    Other places nameof shines:
    - Logging:          _logger.LogInformation("{Method} started", nameof(ProcessOrder));
    - ASP.NET:          RedirectToAction(nameof(Index));
    - Attributes:       [NotNullIfNotNull(nameof(input))]

    //1. It's not reflection - it never looks at the runtime value or type.

    Later improvements:
    - C# 11: nameof of a method PARAMETER is allowed inside attributes on that method.
    - C# 14: nameof works on unbound generic types:  nameof(List<>) == "List"

     */
}
