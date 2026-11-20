// =============================================================================
//  Feature     : Expression Trees
//  C# Version  : C# 3.0
//  .NET        : .NET Framework 3.5
//  Released    : 2007 (Visual Studio 2008)
// -----------------------------------------------------------------------------
//  Summary:
//    A lambda assigned to Expression<TDelegate> is NOT compiled into executable
//    code; instead the compiler builds a tree of objects DESCRIBING the code
//    (parameters, operators, constants...). Libraries can inspect that tree and
//    translate it into something else - Entity Framework turns
//    'x => x.Age > 18' into SQL. Trees can also be built by hand at runtime and
//    compiled into fast delegates with Compile().
// =============================================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CSharp
{
    public class ExpressionTrees
    {
        public static void Run()
        {
            DemonstrateDelegateVsExpression();
            DemonstrateInspectingATree();
            DemonstrateBuildingATree();
        }

        // DELEGATE vs EXPRESSION
        // - The SAME lambda text produces two very different things depending on target type:
        //     Func<int,bool>              -> compiled IL; you can only CALL it.
        //     Expression<Func<int,bool>>  -> a data structure you can READ; ToString() shows the code.
        // - Compile() turns an expression tree into a callable delegate at runtime.
        public static void DemonstrateDelegateVsExpression()
        {
            Func<int, bool> asDelegate = x => x > 5;
            Expression<Func<int, bool>> asExpression = x => x > 5;

            Console.WriteLine(asDelegate(10));                    // True
            Console.WriteLine(asExpression);                      // x => (x > 5)

            Func<int, bool> compiled = asExpression.Compile();
            Console.WriteLine(compiled(3));                       // False
        }

        // INSPECTING A TREE
        // - expr.Body is the root node: for 'a * b + 2' it is an Add BinaryExpression whose
        //   Left is the Multiply node (a * b) and whose Right is a ConstantExpression (2).
        // - Every node has a NodeType (Add, Multiply, GreaterThanOrEqual, ...).
        // - Walking nodes like this is exactly how query providers (EF, LINQ to SQL) translate
        //   C# into SQL. The second half builds a tiny "WHERE age >= 18" to illustrate.
        public static void DemonstrateInspectingATree()
        {
            Expression<Func<int, int, int>> expr = (a, b) => a * b + 2;

            var add = (BinaryExpression)expr.Body;
            var multiply = (BinaryExpression)add.Left;
            var constant = (ConstantExpression)add.Right;

            Console.WriteLine($"Body node type: {add.NodeType}");          // Add
            Console.WriteLine($"Left: {multiply} ({multiply.NodeType})");   // (a * b) (Multiply)
            Console.WriteLine($"Right: {constant.Value}");                  // 2
            Console.WriteLine($"Parameters: {string.Join(", ", expr.Parameters)}"); // a, b

            Expression<Func<int, bool>> filter = age => age >= 18;
            var body = (BinaryExpression)filter.Body;
            Console.WriteLine($"WHERE {((ParameterExpression)body.Left).Name} {ToSql(body.NodeType)} {((ConstantExpression)body.Right).Value}");
            // WHERE age >= 18
        }

        static string ToSql(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.Equal: return "=";
                default: return type.ToString();
            }
        }

        // BUILDING A TREE BY HAND
        // - The static factory methods on Expression (Parameter, Add, Multiply, Property,
        //   Lambda...) construct nodes programmatically. Here we build (x, y) => x * x + y.
        // - Compile() produces a real delegate that runs at full speed.
        // - Practical use: BuildPropertyGetter creates a fast getter for a property chosen by
        //   NAME at runtime - much faster than calling reflection's PropertyInfo.GetValue
        //   repeatedly (compile once, call many times).
        public static void DemonstrateBuildingATree()
        {
            ParameterExpression x = Expression.Parameter(typeof(int), "x");
            ParameterExpression y = Expression.Parameter(typeof(int), "y");

            BinaryExpression body = Expression.Add(Expression.Multiply(x, x), y);
            Expression<Func<int, int, int>> lambda = Expression.Lambda<Func<int, int, int>>(body, x, y);

            Console.WriteLine(lambda);                            // (x, y) => ((x * x) + y)
            Func<int, int, int> fn = lambda.Compile();
            Console.WriteLine(fn(3, 4));                          // 13

            Func<string, int> getLength = BuildPropertyGetter<string, int>("Length");
            Console.WriteLine(getLength("hello"));                // 5
        }

        static Func<T, TResult> BuildPropertyGetter<T, TResult>(string propertyName)
        {
            ParameterExpression obj = Expression.Parameter(typeof(T), "obj");
            MemberExpression property = Expression.Property(obj, propertyName);
            return Expression.Lambda<Func<T, TResult>>(property, obj).Compile();
        }
    }

    /*

    Where you meet expression trees:
    - IQueryable / Entity Framework: Where(x => x.Age > 18) becomes SQL
    - Mocking libraries: mock.Setup(x => x.GetUser(It.IsAny<int>()))
    - Strongly-typed property selectors: builder.Property(p => p.Name).IsRequired()

    //1. Only expression-bodied lambdas convert - no statement bodies { ... }, and some newer
    //   syntax (e.g. ?. , pattern matching, tuple literals) isn't allowed in expression trees.

    //2. Compile() is expensive - cache the compiled delegate.

     */
}
