// =============================================================================
//  Feature     : Primary Constructors for Classes
//  C# Version  : C# 12.0
//  .NET        : .NET 8
//  Released    : 2023 (Visual Studio 2022 v17.8)
// -----------------------------------------------------------------------------
//  Summary:
//    Constructor parameters can be declared right after the class name:
//        class OrderService(ILogger logger, IRepo repo) { ... }
//    Those parameters are in scope in the WHOLE class body - usable from methods,
//    properties and field initializers. Records had this since C# 9; C# 12
//    brought it to every class and struct (structs: 12_PrimaryConstructorsForStructs.cs).
//    The most common class in modern .NET is a service receiving dependencies via
//    dependency injection, which used to need fields + constructor + assignments.
//    Primary constructors remove almost all of that boilerplate.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class PrimaryConstructorsForClasses
    {
        public static void Run()
        {
            DemonstrateDependencyInjection();
            DemonstrateInitializingMembers();
            DemonstrateValidationAndChaining();
            DemonstrateInheritance();
        }

        // Simple dependencies used by the demos below.
        public interface IClock { DateTime Now { get; } }
        public class FixedClock : IClock { public DateTime Now => new(2025, 1, 15, 9, 30, 0); }

        public interface ILog { void Write(string message); }
        public class ConsoleLog : ILog { public void Write(string message) => Console.WriteLine($"  [log] {message}"); }

        // BEFORE C# 12: the classic DI service - private readonly fields, a constructor, and an
        // assignment per dependency, before writing any actual logic.
        public class GreetingServiceOld
        {
            private readonly IClock _clock;
            private readonly ILog _log;

            public GreetingServiceOld(IClock clock, ILog log)
            {
                _clock = clock;
                _log = log;
            }

            public string Greet(string name)
            {
                _log.Write($"Greeting {name}");
                return _clock.Now.Hour < 12 ? $"Good morning, {name}" : $"Hello, {name}";
            }
        }

        // C# 12 PRIMARY CONSTRUCTOR: same behaviour, a fraction of the code.
        // - '(IClock clock, ILog log)' after the class name declares the constructor.
        // - The parameters are used DIRECTLY inside members (log.Write, clock.Now); the compiler
        //   captures them in hidden fields only when a member needs them.
        public class GreetingService(IClock clock, ILog log)
        {
            public string Greet(string name)
            {
                log.Write($"Greeting {name}");
                return clock.Now.Hour < 12 ? $"Good morning, {name}" : $"Hello, {name}";
            }
        }

        // Both versions are constructed and behave identically. In ASP.NET Core the DI container
        // calls the primary constructor for you: builder.Services.AddScoped<GreetingService>();
        public static void DemonstrateDependencyInjection()
        {
            var clock = new FixedClock();
            var log = new ConsoleLog();

            Console.WriteLine(new GreetingServiceOld(clock, log).Greet("Alice"));
            Console.WriteLine(new GreetingService(clock, log).Greet("Bob"));
        }

        // INITIALIZING FIELDS AND PROPERTIES FROM THE PARAMETERS
        // - Parameters can be used in property and field INITIALIZERS (Owner = owner, ...).
        // - Unlike records, a plain class does NOT get public properties generated: 'owner' is
        //   only a parameter, so outside code can't read account.owner - expose it explicitly
        //   (here via the Owner property).
        public class BankAccount(string owner, decimal openingBalance)
        {
            public string Owner { get; } = owner;
            public decimal Balance { get; private set; } = openingBalance;
            public List<string> History { get; } = new() { $"Opened with {openingBalance}" };

            public void Deposit(decimal amount)
            {
                Balance += amount;
                History.Add($"Deposit {amount}");
            }
        }

        public static void DemonstrateInitializingMembers()
        {
            var account = new BankAccount("Carol", 100m);
            account.Deposit(50m);
            Console.WriteLine($"{account.Owner}: {account.Balance} ({string.Join(" | ", account.History)})");
            // Carol: 150 (Opened with 100 | Deposit 50)
        }

        // VALIDATION AND ADDITIONAL CONSTRUCTORS
        // - There's no constructor BODY, so validate inside an initializer - here with a throw
        //   expression (C# 7) that rejects temperatures below absolute zero.
        // - Any extra constructor MUST chain to the primary one with ': this(...)'.
        public class Temperature(double celsius)
        {
            public double Celsius { get; } = celsius >= -273.15
                ? celsius
                : throw new ArgumentOutOfRangeException(nameof(celsius), "Below absolute zero");

            public double Fahrenheit => Celsius * 9 / 5 + 32;

            public Temperature() : this(20) { }
        }

        public static void DemonstrateValidationAndChaining()
        {
            Console.WriteLine($"Default: {new Temperature().Celsius}°C");          // 20°C
            Console.WriteLine($"Boiling: {new Temperature(100).Fahrenheit}°F");     // 212°F
            try
            {
                _ = new Temperature(-300);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Rejected -300°C");
            }
        }

        // INHERITANCE
        // - A base class can have a primary constructor too (Repository<T>(ILog log)).
        // - The derived class passes its own parameters to the base right in the declaration:
        //   'class ProductRepository(ILog log, string category) : Repository<string>(log)'.
        public abstract class Repository<T>(ILog log)
        {
            protected readonly List<T> Items = new();
            public void Add(T item)
            {
                Items.Add(item);
                log.Write($"{GetType().Name} added {item}");
            }
        }

        public class ProductRepository(ILog log, string category) : Repository<string>(log)
        {
            public string Summary() => $"{category}: {string.Join(", ", Items)}";
        }

        public static void DemonstrateInheritance()
        {
            var repo = new ProductRepository(new ConsoleLog(), "Electronics");
            repo.Add("Laptop");
            repo.Add("Phone");
            Console.WriteLine(repo.Summary());                  // Electronics: Laptop, Phone
        }
    }

    /*

    Class vs record primary constructors:
        record Person(string Name)   -> generates a public init-only PROPERTY 'Name', Equals, ToString...
        class  Person(string name)   -> just a PARAMETER in scope; nothing public is generated

    //1. Primary constructor parameters are NOT readonly. Code inside the class can reassign them:
    //       log = null!;   // compiles!
    //   If that matters, copy into a readonly field:  private readonly ILog _log = log;

    //2. Don't BOTH capture a parameter (use it in a method) AND copy it into a field/property -
    //   you'd end up with two separate copies that can drift apart. The compiler warns (CS9124).

    //3. Naming: parameters use camelCase (clock, log), not _underscore field style.

     */
}
