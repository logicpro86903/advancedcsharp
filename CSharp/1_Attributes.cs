// =============================================================================
//  Feature     : Attributes
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    Attributes attach declarative METADATA to code elements (classes, methods,
//    properties, parameters, assemblies...). They are written in [square brackets]
//    and stored in the compiled assembly. On their own they do nothing - code
//    (the compiler, the runtime, or your own reflection code) has to read them.
//    Frameworks rely on them heavily: ASP.NET [HttpGet], EF [Key], xUnit [Fact].
// =============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CSharp
{
    public class Attributes
    {
        public static void Run()
        {
            DemonstrateCustomAttributes();
            DemonstrateBuiltInAttributes();
        }

        // CUSTOM ATTRIBUTE DEFINITION
        // - A custom attribute is just a class that derives from System.Attribute.
        // - By convention the class name ends with "Attribute"; when you USE it you can drop
        //   the suffix ([Author] instead of [AuthorAttribute]).
        // - [AttributeUsage] restricts WHERE it may be applied (here: classes and methods)
        //   and whether it can appear more than once on the same element (AllowMultiple).
        // - Constructor parameters become POSITIONAL arguments: [Author("Alice")].
        // - Public fields/properties become NAMED arguments:   [Author("Alice", Version = 2)].
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
        public class AuthorAttribute : Attribute
        {
            public string Name;
            public int Version;

            public AuthorAttribute(string name)
            {
                Name = name;
            }
        }

        [Author("Alice", Version = 2)]
        public class InvoiceService
        {
            [Author("Bob")]
            public void Generate() { }

            public void Send() { }
        }

        // READING ATTRIBUTES WITH REFLECTION
        // - Attributes are passive metadata; nothing happens until code asks for them.
        // - Attribute.GetCustomAttribute(member, attributeType) returns the instance (or null),
        //   constructed with the arguments written in the [ ... ] declaration.
        // - Here we read the class-level attribute, then loop over the declared public
        //   instance methods and check each one for an [Author].
        public static void DemonstrateCustomAttributes()
        {
            Type type = typeof(InvoiceService);

            AuthorAttribute? classAuthor =
                (AuthorAttribute?)Attribute.GetCustomAttribute(type, typeof(AuthorAttribute));
            Console.WriteLine($"Class author: {classAuthor!.Name} v{classAuthor.Version}"); // Class author: Alice v2

            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                AuthorAttribute? author =
                    (AuthorAttribute?)Attribute.GetCustomAttribute(method, typeof(AuthorAttribute));
                Console.WriteLine($"{method.Name}: {(author != null ? author.Name : "(no author)")}");
            }
            // Generate: Bob
            // Send: (no author)
        }

        // BUILT-IN ATTRIBUTES (understood by the compiler / runtime)
        // - [Obsolete("msg")] : the compiler emits warning CS0618 wherever the member is used
        //                       (pass true as 2nd arg to make it a compile ERROR instead).
        // - [Conditional("DEBUG")] : every CALL to the method is removed by the compiler unless
        //                       the DEBUG symbol is defined, so it costs nothing in Release builds.
        // Other common ones: [Serializable], [Flags], [DllImport], [AttributeUsage].
        [Obsolete("Use NewMethod instead")]
        static void OldMethod() { }

        [System.Diagnostics.Conditional("DEBUG")]
        static void DebugOnlyLog(string message)
        {
            Console.WriteLine("[DEBUG] " + message);
        }

        // USING BUILT-IN ATTRIBUTES
        // - Calling OldMethod() would produce a compiler warning, so it's left commented out.
        // - DebugOnlyLog prints only in Debug configuration.
        // - IsDefined(...) is a quick reflection check for "does this member carry attribute X?".
        public static void DemonstrateBuiltInAttributes()
        {
            // OldMethod();   // warning CS0618: 'OldMethod' is obsolete: 'Use NewMethod instead'
            DebugOnlyLog("Only printed in Debug configuration");

            bool isObsolete = typeof(Attributes)
                .GetMethod("OldMethod", BindingFlags.NonPublic | BindingFlags.Static)!
                .IsDefined(typeof(ObsoleteAttribute), false);
            Console.WriteLine($"OldMethod is marked obsolete: {isObsolete}"); // True
        }
    }

    /*

    Related features:
    - C# 5: caller info attributes [CallerMemberName] etc. (see 5_CallerInfo.cs)
    - C# 11: generic attributes [Validator<T>]          (see 11_GenericAttributes.cs)

     */
}
