using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 9 / C# 13.0 (2024)
    public class LockType
    {
        // Old way with Monitor
        private readonly object _lock = new object();

        // C# 13 with new Lock type
        private readonly Lock _lockT = new();

        public void Run()
        {

            // Old way with Monitor
            lock (_lock)
            {
            // Critical section
            }

            // C# 13 with new Lock type
            lock (_lockT)
            {
                // Critical section - uses Lock.EnterScope() automatically
            }
        }
    }
}
