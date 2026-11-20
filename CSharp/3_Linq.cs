// =============================================================================
//  Feature     : LINQ (Language Integrated Query)
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    LINQ adds a uniform, strongly-typed way to query any data source - in-memory
//    collections, databases, XML - directly in C#. It comes in two syntaxes:
//    QUERY syntax (from/where/select, SQL-like) and METHOD syntax (extension
//    methods + lambdas). Both compile to the same calls. LINQ is the payoff of all
//    the C# 3 features working together: extension methods, lambdas, anonymous
//    types, var, object initializers and expression trees. Most operators use
//    DEFERRED execution: the query runs only when you enumerate it.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp
{
    public class Linq
    {
        public static void Run()
        {
            DemonstrateQueryVsMethodSyntax();
            DemonstrateCommonOperators();
            DemonstrateGrouping();
            DemonstrateJoin();
            DemonstrateDeferredExecution();
        }

        // Sample data used by all the demos below.
        public class Staff
        {
            public string Name { get; set; } = "";
            public string Department { get; set; } = "";
            public decimal Salary { get; set; }
            public int OfficeId { get; set; }
        }

        public class Office
        {
            public int Id { get; set; }
            public string City { get; set; } = "";
        }

        static List<Staff> GetStaff()
        {
            return new List<Staff>
            {
                new Staff { Name = "Alice", Department = "Engineering", Salary = 95000, OfficeId = 1 },
                new Staff { Name = "Bob",   Department = "Engineering", Salary = 85000, OfficeId = 2 },
                new Staff { Name = "Carol", Department = "Sales",       Salary = 60000, OfficeId = 1 },
                new Staff { Name = "Dave",  Department = "Sales",       Salary = 65000, OfficeId = 2 },
                new Staff { Name = "Eve",   Department = "HR",          Salary = 55000, OfficeId = 1 }
            };
        }

        static List<Office> GetOffices()
        {
            return new List<Office>
            {
                new Office { Id = 1, City = "London" },
                new Office { Id = 2, City = "Berlin" }
            };
        }

        // QUERY SYNTAX vs METHOD SYNTAX
        // - Query syntax (from ... where ... orderby ... select) reads like SQL.
        // - The compiler translates it into method calls: Where(...), OrderByDescending(...),
        //   Select(...). Both versions below are IDENTICAL after compilation.
        // - Method syntax exposes every operator; query syntax is nicer for joins/grouping.
        public static void DemonstrateQueryVsMethodSyntax()
        {
            var staff = GetStaff();

            var querySyntax =
                from s in staff
                where s.Salary > 60000
                orderby s.Salary descending
                select s.Name;

            var methodSyntax = staff
                .Where(s => s.Salary > 60000)
                .OrderByDescending(s => s.Salary)
                .Select(s => s.Name);

            Console.WriteLine($"Query:  {string.Join(", ", querySyntax)}");  // Alice, Bob, Dave
            Console.WriteLine($"Method: {string.Join(", ", methodSyntax)}"); // Alice, Bob, Dave
        }

        // COMMON OPERATORS
        // - Aggregation   : Count, Sum, Average, Min, Max
        // - Element       : First (throws if none), FirstOrDefault (returns null/default if none)
        // - Quantifiers   : Any (at least one matches?), All (every item matches?)
        // - Partitioning  : Take(n) first n items, Skip(n) everything after the first n
        // - Set           : Distinct (remove duplicates), Intersect (items in both sequences)
        // - Ordering      : OrderBy then ThenBy for secondary sort keys
        // - Flattening    : SelectMany turns a sequence of sequences into one flat sequence
        public static void DemonstrateCommonOperators()
        {
            var staff = GetStaff();
            int[] numbers = { 5, 3, 8, 1, 9, 2 };

            Console.WriteLine($"Count: {staff.Count()}, Total: {staff.Sum(s => s.Salary)}, Avg: {staff.Average(s => s.Salary)}");
            Console.WriteLine($"Min: {numbers.Min()}, Max: {numbers.Max()}");

            Console.WriteLine($"First in Sales: {staff.First(s => s.Department == "Sales").Name}");     // Carol
            Console.WriteLine($"First in IT: {staff.FirstOrDefault(s => s.Department == "IT")?.Name ?? "none"}"); // none

            Console.WriteLine($"Any > 90k: {staff.Any(s => s.Salary > 90000)}");  // True
            Console.WriteLine($"All > 50k: {staff.All(s => s.Salary > 50000)}");  // True

            var top2 = numbers.OrderByDescending(n => n).Take(2);
            Console.WriteLine($"Top 2: {string.Join(", ", top2)}");                // 9, 8
            Console.WriteLine($"Skip 4: {string.Join(", ", numbers.Skip(4))}");    // 9, 2

            int[] other = { 8, 9, 10 };
            Console.WriteLine($"Distinct depts: {string.Join(", ", staff.Select(s => s.Department).Distinct())}");
            Console.WriteLine($"Intersect: {string.Join(", ", numbers.Intersect(other))}"); // 8, 9

            var sorted = staff.OrderBy(s => s.Department).ThenBy(s => s.Name).Select(s => s.Name);
            Console.WriteLine($"Sorted: {string.Join(", ", sorted)}");
            var letters = new[] { "ab", "cd" }.SelectMany(w => w.ToCharArray());
            Console.WriteLine($"Flattened: {string.Join(",", letters)}");          // a,b,c,d
        }

        // GROUPING
        // - 'group s by s.Department into g' produces groups; each g has a Key (the department)
        //   and is itself a sequence of the Staff in that department.
        // - Aggregate per group (Count, Average) and project into an anonymous type.
        // - ToDictionary converts a sequence into a key -> value lookup (keys must be unique).
        public static void DemonstrateGrouping()
        {
            var staff = GetStaff();

            var byDepartment =
                from s in staff
                group s by s.Department into g
                orderby g.Key
                select new { Department = g.Key, Count = g.Count(), AvgSalary = g.Average(s => s.Salary) };

            foreach (var d in byDepartment)
                Console.WriteLine($"{d.Department}: {d.Count} people, avg {d.AvgSalary:N0}");
            // Engineering: 2 people, avg 90,000
            // HR: 1 people, avg 55,000
            // Sales: 2 people, avg 62,500

            Dictionary<string, decimal> salaryByName = staff.ToDictionary(s => s.Name, s => s.Salary);
            Console.WriteLine($"Bob earns {salaryByName["Bob"]}");
        }

        // JOIN
        // - Inner join two sequences on matching keys (like SQL INNER JOIN).
        // - Query syntax: 'join o in offices on s.OfficeId equals o.Id' (must use 'equals').
        // - Method syntax: Join(inner, outerKeySelector, innerKeySelector, resultSelector).
        public static void DemonstrateJoin()
        {
            var staff = GetStaff();
            var offices = GetOffices();

            var located =
                from s in staff
                join o in offices on s.OfficeId equals o.Id
                where o.City == "Berlin"
                select new { s.Name, o.City };

            foreach (var x in located)
                Console.WriteLine($"{x.Name} works in {x.City}"); // Bob, Dave in Berlin

            var count = staff.Join(offices, s => s.OfficeId, o => o.Id, (s, o) => o.City)
                             .Count(city => city == "London");
            Console.WriteLine($"London staff: {count}");          // 3
        }

        // DEFERRED vs IMMEDIATE EXECUTION
        // - Where/Select/OrderBy only DEFINE a query; nothing runs until it is enumerated.
        //   That's why the "[checking n]" output appears only when string.Join iterates it,
        //   and why 4 - added AFTER the query was defined - is still included.
        // - ToList()/ToArray()/Count()/First() execute IMMEDIATELY and capture a snapshot;
        //   later changes to the source (adding 100) aren't reflected.
        public static void DemonstrateDeferredExecution()
        {
            var numbers = new List<int> { 1, 2, 3 };

            var evens = numbers.Where(n =>
            {
                Console.Write($"[checking {n}] ");
                return n % 2 == 0;
            });

            numbers.Add(4);

            Console.WriteLine($"\nResult: {string.Join(", ", evens)}"); // 2, 4

            var snapshot = numbers.Where(n => n > 2).ToList();
            numbers.Add(100);
            Console.WriteLine($"Snapshot: {string.Join(", ", snapshot)}"); // 3, 4
        }
    }

    /*

    Providers:
    - LINQ to Objects: IEnumerable<T>, runs in memory using delegates.
    - LINQ to SQL / Entity Framework: IQueryable<T>, translates expression trees to SQL.

    //1. Multiple enumeration: iterating a deferred query twice runs it twice (e.g. two DB calls).
    //   Call ToList() once if you need the results more than once.

    //2. First() throws on empty; FirstOrDefault() returns default. Single() throws on 0 or >1 matches.

    //3. With EF, calling a C# method the provider can't translate inside Where() throws at runtime.

    //4. Avoid Count() > 0 to test emptiness - use Any().

     */
}
