// =============================================================================
//  Feature     : The New Lock Type (System.Threading.Lock)
//  C# Version  : C# 13.0
//  .NET        : .NET 9
//  Released    : 2024 (Visual Studio 2022 v17.12)
// -----------------------------------------------------------------------------
//  Summary:
//    .NET 9 introduced System.Threading.Lock, a dedicated type for mutual
//    exclusion, and C# 13 made the 'lock' statement recognize it. Locking on a
//    Lock instance compiles to its faster Lock.EnterScope() API instead of the
//    classic Monitor.Enter/Exit used for 'lock (object)'. It also makes intent
//    explicit: the field exists only to be locked on. Simply change the field's
//    type from 'object' to 'Lock' - the lock statements stay the same.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class LockType
    {
        // LOCK FIELDS
        // - Old way: any reference object can be a lock; by convention a private 'new object()'.
        // - C# 13 / .NET 9: a 'Lock' instance - purpose-built, lighter and faster.
        private readonly object _lock = new object();

        private readonly Lock _lockT = new();

        // THE lock STATEMENT
        // - 'lock (_lock)' on an object compiles to Monitor.Enter / Monitor.Exit in try/finally.
        // - 'lock (_lockT)' on a Lock compiles to 'using (_lockT.EnterScope()) { ... }', which is
        //   released automatically at the end of the block, even if an exception is thrown.
        // - Only one thread at a time can be inside a critical section guarded by the same lock.
        public void Run()
        {
            lock (_lock)
            {
                // Critical section
            }

            lock (_lockT)
            {
                // Critical section
            }
        }
    }

    /*

    //1. Don't cast a Lock to object (or store it in an 'object' field) and lock on that - you'd
    //   silently fall back to Monitor-based locking. The compiler warns (CS9216).
    //2. As before: never lock on 'this', a Type, or a string - keep the lock object private.
    //3. 'lock' can't contain 'await'. For async code use SemaphoreSlim.WaitAsync instead.

     */
}
