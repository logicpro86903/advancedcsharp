// =============================================================================
//  Feature     : Enums
//  C# Version  : C# 1.0
//  .NET        : .NET Framework 1.0
//  Released    : 2002 (Visual Studio .NET 2002)
// -----------------------------------------------------------------------------
//  Summary:
//    An enum is a value type that gives readable NAMES to a set of integral
//    constants (OrderStatus.Shipped instead of the magic number 1). The
//    underlying type is int by default but can be any integral type. With the
//    [Flags] attribute, enum values become bit flags that can be combined with
//    | and tested with &, which is ideal for permissions and option sets.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class Enums
    {
        public static void Run()
        {
            DemonstrateBasicEnums();
            DemonstrateFlags();
        }

        // BASIC ENUM DECLARATION
        // - Members are numbered automatically starting at 0 and increasing by 1:
        //   Pending = 0, Shipped = 1, Delivered = 2.
        // - You can assign explicit values to any member (Cancelled = 99).
        public enum OrderStatus
        {
            Pending,
            Shipped,
            Delivered,
            Cancelled = 99
        }

        // [FLAGS] ENUM DECLARATION
        // - Each member is a distinct power of two (one bit), so members can be OR-ed together.
        // - Always define None = 0 to represent "no flags set".
        // - Composite members (All) can be defined from the others.
        // - ": byte" changes the underlying storage type (default is int).
        // - [Flags] also makes ToString() print combinations as "Read, Write".
        [Flags]
        public enum FilePermissions : byte
        {
            None    = 0,
            Read    = 1,
            Write   = 2,
            Execute = 4,
            All     = Read | Write | Execute
        }

        // WORKING WITH ENUMS
        // - ToString() prints the member name; casting to int gives the underlying number.
        // - Enum.Parse converts a string name into the enum value; casting converts from int.
        // - Enum.GetNames lists every member name (useful for dropdowns, validation, etc.).
        // - Enums work naturally in switch statements, making intent very readable.
        public static void DemonstrateBasicEnums()
        {
            OrderStatus status = OrderStatus.Shipped;
            Console.WriteLine(status);              // Shipped
            Console.WriteLine((int)status);         // 1

            OrderStatus parsed = (OrderStatus)Enum.Parse(typeof(OrderStatus), "Delivered");
            OrderStatus fromInt = (OrderStatus)99;
            Console.WriteLine($"{parsed}, {fromInt}"); // Delivered, Cancelled

            foreach (string name in Enum.GetNames(typeof(OrderStatus)))
                Console.Write(name + " ");
            Console.WriteLine();                    // Pending Shipped Delivered Cancelled

            switch (status)
            {
                case OrderStatus.Pending:   Console.WriteLine("Waiting..."); break;
                case OrderStatus.Shipped:   Console.WriteLine("On the way!"); break;
                default:                    Console.WriteLine("Done"); break;
            }
        }

        // WORKING WITH [FLAGS]
        // - Combine flags with bitwise OR:      Read | Write
        // - Test a flag with bitwise AND:       (perms & Write) == Write
        // - Add a flag:                         perms |= Execute
        // - Remove a flag (AND with the NOT):   perms &= ~Write
        // - HasFlag (.NET 4+) is a more readable way to test a flag.
        public static void DemonstrateFlags()
        {
            FilePermissions perms = FilePermissions.Read | FilePermissions.Write;
            Console.WriteLine(perms);                                            // Read, Write

            bool canWrite = (perms & FilePermissions.Write) == FilePermissions.Write;
            Console.WriteLine($"Can write: {canWrite}");                         // Can write: True

            perms |= FilePermissions.Execute;
            perms &= ~FilePermissions.Write;
            Console.WriteLine(perms);                                            // Read, Execute
            Console.WriteLine($"Has execute: {perms.HasFlag(FilePermissions.Execute)}"); // True
        }
    }

    /*

    //1. Enums are NOT validated. Any integer can be cast to an enum:
    OrderStatus weird = (OrderStatus)42;   // compiles and runs fine
    // Use Enum.IsDefined(typeof(OrderStatus), value) when the value comes from outside.

    //2. [Flags] values must be powers of two, and always define None = 0.

     */
}
